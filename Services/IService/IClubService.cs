using BusinessObjects.Entities;

namespace Services.IService;

public interface IClubService
{
    bool CheckPhoneExisted(string phone);
    List<Club> GetAllClubs();
    void AddClub(Club club);
    void DeleteClub(int clubId);
    Club GetClubById(int clubId);
    void UpdateClub(Club c);
}