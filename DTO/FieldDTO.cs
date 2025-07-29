namespace EnergomeraTestTask.DTO;

public record CoordinateDto(double Lat, double Lng);

public record LocationDto(CoordinateDto Center, IEnumerable<CoordinateDto> Polygon);

public record FieldDto(string Id, string Name, double Size, LocationDto Locations);

public record ContainsDto(string Id, string Name);
