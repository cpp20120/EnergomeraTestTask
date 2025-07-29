using NetTopologySuite.Geometries;
using NetTopologySuite.Geometries.Utilities;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;

namespace EnergomeraTestTask.Utils
{
    public static class GeometryTransform
    {
        public static double ToMeterSquare(Geometry geom)
        {
            var wgs84 = GeographicCoordinateSystem.WGS84;
            var projected = ProjectedCoordinateSystem.WGS84_UTM(36, true); 

            var transform = new CoordinateTransformationFactory()
                .CreateFromCoordinateSystems(wgs84, projected)
                .MathTransform;

            var editor = new GeometryEditor();
            var projectedGeom = editor.Edit(geom, new CoordinateTransformer(transform));
            return projectedGeom.Area;
        }

        private class CoordinateTransformer(MathTransform transform) : GeometryEditor.CoordinateOperation
        {
            public override Coordinate[] Edit(Coordinate[] coordinates, Geometry geometry)
            {
                var transformed = new Coordinate[coordinates.Length];
            
                for (int i = 0; i < coordinates.Length; i++)
                {
                    var coord = coordinates[i];
                    var result = transform.Transform([coord.X, coord.Y]);
                    transformed[i] = new(result[0], result[1]);
                }

                return transformed;
            }
        }
    }
}