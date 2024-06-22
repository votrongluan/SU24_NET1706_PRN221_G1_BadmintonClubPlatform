using BusinessObjects.Entities;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepo;

namespace Repositories.Repo;

public class CourtRepository : ICourtRepository
{
    public Court GetCourtByIdNoInclude(int id)
    {
        return CourtDao.FindByCondition(e => e.CourtId == id).FirstOrDefault();
    }

    public List<Court> GetAllCourts()
    {
        return CourtDao.GetAll().Include(e => e.CourtType).Include(e => e.Club).OrderByDescending(e => e.ClubId).Where(e => e.Status != false).ToList();
    }

    public List<Court> GetCourtsByClubId(int id)
    {
        return CourtDao.FindByCondition(x => x.ClubId == id).ToList();
    }

    public Court GetCourtById(int id)
    {
        return GetAllCourts().Where(e => e.CourtId == id).FirstOrDefault();
    }

    public void DeleteCourt(int id)
    {
        var court = GetCourtByIdNoInclude(id);
        court.Status = false;
        CourtDao.Update(court);
    }

    public void UpdateCourt(Court court)
    {
        CourtDao.Update(court);
    }

    public void AddCourt(Court court)
    {
        CourtDao.Add(court);
    }
}