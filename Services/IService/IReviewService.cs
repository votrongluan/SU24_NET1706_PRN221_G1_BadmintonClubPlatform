using BusinessObjects.Entities;

namespace Services.IService;

public interface IReviewService
{
    List<Review> GetAllReviews();
    void AddReview(Review review);
    void DeleteReview(int reviewId);
    Review GetReviewById(int reviewId);
    void UpdateReview(Review review);
}