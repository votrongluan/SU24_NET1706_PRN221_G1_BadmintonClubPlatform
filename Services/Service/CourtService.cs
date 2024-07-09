using BusinessObjects.Entities;
using DataAccessObjects;
using Repositories.IRepo;
using Services.IService;

namespace Services.Service;

public class CourtService : ICourtService
{
    private readonly IRepositoryManager _repo;

    public CourtService(IRepositoryManager repo)
    {
        _repo = repo;
    }
    public List<Court> GetAllCourts()
    {
        return _repo.Court.GetAllCourts();
    }
    public List<Court> GetCourtsByClubId(int id)
    {
        return _repo.Court.GetCourtsByClubId(id);
    }

    public void AddCourt(Court court)
    {
        _repo.Court.AddCourt(court);
    }
    
    public void DeleteCourt(int courtId)
    {
        _repo.Court.DeleteCourt(courtId);
    }

    public Court GetCourtById(int courtId)
    {
        return _repo.Court.GetCourtById(courtId);
    }
    public bool CheckClubIdExisted(int clubId)
    {
        return _repo.Court.GetAllCourts().Any(e => e.ClubId.Equals(clubId));
    }
}