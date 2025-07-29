using NetTopologySuite.Geometries;

namespace EnergomeraTestTask.Models;

public class FieldData
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public Coordinate Center { get; set; } = new();
    public Polygon Polygon { get; set; } = default!;
}
