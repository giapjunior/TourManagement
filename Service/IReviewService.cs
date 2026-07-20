using DataAccess.DataAccess;
using System.Collections.Generic;

namespace Service
{
    public interface IReviewService
    {
        List<Review> GetByTourId(int tourId);
        void Add(Review review);
    }
}
