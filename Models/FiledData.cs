using NetTopologySuite.Geometries;

namespace EnergomeraTestTask.Models;

public class FieldData
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public Coordinate Center { get; set; } = new();
    public Polygon Polygon { get; init; } = default!;
}
