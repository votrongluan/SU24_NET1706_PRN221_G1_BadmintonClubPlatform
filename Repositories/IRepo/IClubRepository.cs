using BusinessObjects.Entities;

namespace Repositories.IRepo;

public interface IClubRepository
{
    List<Club> GetAllClubs();
    Club GetClubByIdNotInclude(int id);
    Club GetClubById(int id);
    void DeleteClub(int id);
    void UpdateClub(Club club);
    void AddClub(Club club);
    List<Club> GetMostRatingClubs();
    List<Club> GetMostBookingClubs();
    List<Club> GetMostPopularClubs();
}