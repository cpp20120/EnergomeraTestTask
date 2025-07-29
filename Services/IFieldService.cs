namespace EnergomeraTestTask.Services
{
    public interface IFieldService
    {
        IEnumerable<object> GetAll();
        double GetSize(string id);
        double GetDistance(string id, double lat, double lng);
        object Contains(double lat, double lng);
        void Reload();
    }
}