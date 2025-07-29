using EnergomeraTestTask.DTO;

namespace EnergomeraTestTask.Services
{
    public interface IFieldService
    {
        IEnumerable<FieldDto> GetAll();       
        double GetSize(string id);
        double GetDistance(string id, double lat, double lng);
        ContainsDto? Contains(double lat, double lng);
        void Reload();
    }
}