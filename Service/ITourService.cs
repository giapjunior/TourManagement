using DataAccess.DataAccess;

namespace Service
{
    public interface ITourService
    {
        List<Tour> GetAll();
        List<Tour> GetActive();
        Tour GetById(int id);
        List<Tour> Search(string keyword);
        List<Tour> AdvancedSearch(string keyword, decimal? maxPrice, System.DateTime? departureDate);
        void Add(Tour tour);
        void Update(Tour tour);
        void SoftDelete(int id);
    }
}
