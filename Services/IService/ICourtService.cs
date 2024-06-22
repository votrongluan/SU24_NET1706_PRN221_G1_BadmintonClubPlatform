using BusinessObjects.Entities;

namespace Services.IService;

public interface ICourtService
{
    List<Court> GetAllCourts();
    public bool CheckClubIdExisted(int clubId);
    List<Court> GetCourtsByClubId(int clubId);
    void AddCourt(Court court);
    void DeleteCourt(int courtId);
    Court GetCourtById(int courtId);
}