using DataAccess.DataAccess;

namespace Repository
{
    public class TourRepository : ITourRepository
    {
        private readonly TourManagementDbContext _context;

        public TourRepository(TourManagementDbContext context)
        {
            _context = context;
        }

        public List<Tour> GetAll() => _context.Tours.ToList();

        public Tour GetById(int id) => _context.Tours.FirstOrDefault(t => t.TourId == id);

        public void Add(Tour entity)
        {
            _context.Tours.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Tour entity)
        {
            _context.Tours.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _context.Tours.Remove(entity);
                _context.SaveChanges();
            }
        }

        public List<Tour> Search(string keyword)
            => _context.Tours
                .Where(t => t.TourName.Contains(keyword) || t.Destination.Contains(keyword))
                .ToList();

        public List<Tour> GetActive()
            => _context.Tours.Where(t => t.IsActive == true).ToList();
    }
}
