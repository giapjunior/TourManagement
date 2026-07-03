using DataAccess.DataAccess;

namespace Repository
{
    public interface ITourRepository : IGenericRepository<Tour>
    {
        List<Tour> Search(string keyword);
        List<Tour> AdvancedSearch(string keyword, decimal? maxPrice, System.DateTime? departureDate);
        List<Tour> GetActive();
    }
}
