using BusinessObjects.Entities;

namespace Repositories.IRepo;

public interface IReviewRepository
{
    List<Review> GetAllReviews ();
    Review GetReviewById (int reviewId);
    void DeleteReview (int reviewId);
    void UpdateReview (Review review);
    void AddReview (Review review);
    List<Review> GetAllByClubId (int clubId);
}