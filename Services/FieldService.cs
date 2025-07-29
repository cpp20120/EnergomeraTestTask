using EnergomeraTestTask.Models;
using EnergomeraTestTask.Utils;
using NetTopologySuite.Geometries;
using SharpKml.Dom;
using SharpKml.Engine;
using Coordinate = NetTopologySuite.Geometries.Coordinate;
using KmlPoint = SharpKml.Dom.Point;
using KmlPolygon = SharpKml.Dom.Polygon;

namespace EnergomeraTestTask.Services;

public class FieldService : IFieldService
{
    private readonly Dictionary<string, FieldData> _fields = new();
    private readonly GeometryFactory _geometryFactory;

    public FieldService()
    {
        _geometryFactory = new(new(), 4326);
        Load();
    }

    public void Reload() => Load();

    public IEnumerable<object> GetAll() =>
        _fields.Values.Select(f => new {
            f.Id,
            f.Name,
            Size = GetSize(f.Id),
            Locations = new {
                Center = new[] { f.Center.Y, f.Center.X },
                Polygon = f.Polygon.Coordinates.Select(c => new[] { c.Y, c.X })
            }
        });

    public double GetSize(string id)
    {
        if (!_fields.TryGetValue(id, out var field)) return -1;
        return GeometryTransform.ToMeterSquare(field.Polygon);
    }

    public double GetDistance(string id, double lat, double lng)
    {
        if (!_fields.TryGetValue(id, out var field)) return -1;
        return Haversine(field.Center.Y, field.Center.X, lat, lng);
    }

    public object Contains(double lat, double lng)
    {
        var point = _geometryFactory.CreatePoint(new Coordinate(lng, lat));
        foreach (var f in _fields.Values.Where(f => f.Polygon.Contains(point)))
        {
            return new { f.Id, f.Name };
        }
        return false;
    }

    private void Load()
    {
        _fields.Clear();

        var centroids = LoadCentroids(Path.Combine("Kml", "centroids.kml"));
        foreach (var (id, field) in LoadPolygons(Path.Combine("Kml", "fields.kml")))
        {
            if (centroids.TryGetValue(id, out var center))
                field.Center = center;
            _fields[id] = field;
        }
    }

    private static Dictionary<string, Coordinate> LoadCentroids(string path)
    {
        var doc = KmlFile.Load(File.OpenRead(path));
        var placemarks = doc.Root.Flatten().OfType<Placemark>();
        return placemarks.ToDictionary(p => p.Name, p =>
        {
            var c = ((KmlPoint)p.Geometry).Coordinate;
            return new Coordinate(c.Longitude, c.Latitude);
        });
    }

    private Dictionary<string, FieldData> LoadPolygons(string path)
    {
        var doc = KmlFile.Load(File.OpenRead(path));
        var placemarks = doc.Root.Flatten().OfType<Placemark>();
        return placemarks.ToDictionary(p => p.Name, p =>
        {
            var coords = ((KmlPolygon)p.Geometry).OuterBoundary.LinearRing.Coordinates;
            var poly = _geometryFactory.CreatePolygon(coords.Select(c => new Coordinate(c.Longitude, c.Latitude)).ToArray());
            return new FieldData { Id = p.Name, Name = p.Name, Polygon = poly };
        });
    }

    private static double Haversine(double lat1, double lon1, double lat2, double lon2)
    {
        const double r = 6371000;
        var dLat = ToRad(lat2 - lat1);
        var dLon = ToRad(lon2 - lon1);
        lat1 = ToRad(lat1);
        lat2 = ToRad(lat2);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1) * Math.Cos(lat2) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return r * c;
    }

    private static double ToRad(double deg) => deg * Math.PI / 180;
}
