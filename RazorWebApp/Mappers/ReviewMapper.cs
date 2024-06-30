using BusinessObjects.Dtos.Review;
using BusinessObjects.Entities;
using System.Runtime.CompilerServices;

namespace WebAppRazor.Mappers
{
    public static class ReviewMapper
    {
        public static Review ToReview (this CreateReviewDto e)
        {
            return new Review()
            {
                Star = e.Star,
                Comment = e.Comment,
                ClubId = e.ClubId,
                UserId = e.UserId
            };
        }
    }
}
