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
    double GetAverageRatingStar(int clubId);
}