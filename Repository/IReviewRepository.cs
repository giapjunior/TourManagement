using DataAccess.DataAccess;

namespace Repository
{
    public interface IReviewRepository
    {
        List<Review> GetByTourId(int tourId);
        void Add(Review review);
    }
}
