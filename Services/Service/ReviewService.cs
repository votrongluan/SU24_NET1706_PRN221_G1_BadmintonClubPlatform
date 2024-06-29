using BusinessObjects.Entities;
using Repositories.IRepo;
using Services.IService;

namespace Services.Service;

public class ReviewService : IReviewService
{
    private readonly IRepositoryManager _repo;

    public ReviewService (IRepositoryManager repositoryManager)
    {
        _repo = repositoryManager;
    }

    public List<Review> GetAllReviews ()
    {
        return _repo.Review.GetAllReviews();
    }

    public void AddReview (Review review)
    {
        _repo.Review.AddReview(review);
    }

    public void DeleteReview (int reviewId)
    {
        _repo.Review.DeleteReview(reviewId);
    }

    public Review GetReviewById (int reviewId)
    {
        return _repo.Review.GetReviewById(reviewId);
    }

    public void UpdateReview (Review review)
    {
        _repo.Review.UpdateReview(review);
    }

    public List<Review> GetAllByClubId (int clubId)
    {
        return _repo.Review.GetAllByClubId(clubId);
    }
}