using BusinessObjects.Entities;

namespace Repositories.IRepo;

public interface ICourtRepository
{
    List<Court> GetAllCourts();
    List<Court> GetCourtsByClubId(int id);
    Court GetCourtById(int id);
    void DeleteCourt(int id);
    void UpdateCourt(Court court);
    void AddCourt(Court court);
}