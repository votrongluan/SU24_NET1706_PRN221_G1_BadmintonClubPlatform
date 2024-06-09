using BusinessObjects.Entities;

namespace Repositories.IRepo;

public interface IClubRepository
{
    List<Club> GetClubs();
    List<Club> GetMostRatingClubs();
    List<Club> GetMostBookingClubs();
    List<Club> GetMostPopularClubs();
}