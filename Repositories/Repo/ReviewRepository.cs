using BusinessObjects.Entities;
using DataAccessObjects;
using Repositories.IRepo;

namespace Repositories.Repo;

public class ReviewRepository : IReviewRepository
{
    public List<Review> GetAllReviews()
    {
        return ReviewDao.GetAll().OrderByDescending(e => e.ReviewId).ToList();
    }

    public Review GetReviewById(int reviewId)
    {
        return GetAllReviews().FirstOrDefault(e => e.ReviewId == reviewId);
    }

    public void DeleteReview(int reviewId)
    {
        var review = GetReviewById(reviewId);
        ReviewDao.Delete(review);
    }

    public void UpdateReview(Review review)
    {
        ReviewDao.Update(review);
    }

    public void AddReview(Review review)
    {
        ReviewDao.Add(review);
    }
}