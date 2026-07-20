using DataAccess.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly TourManagementDbContext _context;

        public ReviewRepository(TourManagementDbContext context)
        {
            _context = context;
        }

        public List<Review> GetByTourId(int tourId)
        {
            return _context.Reviews
                .Include(r => r.Customer)
                .Where(r => r.TourId == tourId)
                .OrderByDescending(r => r.ReviewDate)
                .ToList();
        }

        public void Add(Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();
        }
    }
}
