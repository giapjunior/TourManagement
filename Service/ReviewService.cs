using DataAccess.DataAccess;
using Repository;
using System;
using System.Collections.Generic;

namespace Service
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepo;

        public ReviewService(IReviewRepository reviewRepo)
        {
            _reviewRepo = reviewRepo;
        }

        public List<Review> GetByTourId(int tourId)
        {
            return _reviewRepo.GetByTourId(tourId);
        }

        public void Add(Review review)
        {
            if (review.Rating < 1 || review.Rating > 5)
                throw new Exception("Đánh giá phải từ 1 đến 5 sao.");
                
            _reviewRepo.Add(review);
        }
    }
}
