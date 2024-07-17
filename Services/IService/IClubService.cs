using BusinessObjects.Dtos.Club;
using BusinessObjects.Entities;

namespace Services.IService;

public interface IClubService
{
    bool CheckPhoneExisted(string phone);
    List<Club> GetAllClubs();
    List<Club> GetAllDeActiveClubs();
    void AddClub(Club club);
    void DeleteClub(int clubId);
    Club GetClubById(int clubId);
    Club GetDeActiveClubById(int clubId);
    void UpdateClub(Club c);
    List<Club> GetMostRatingClubs();
    List<Club> GetMostBookingClubs();
    List<Club> GetMostPopularClubs();
    double GetAverageRatingStar(int clubId);
    ClubDto ToDto(Club entity);
    Club ToEntity(ClubDto dto);
    Club GetClubByIdReal(int id);
}