using BusinessObjects.Entities;
using DataAccessObjects;
using Repositories.IRepo;

namespace Repositories.Repo;

public class CourtTypeRepository : ICourtTypeRepository
{
    public List<CourtType> GetAllCourtTypes()
    {
        return CourtTypeDao.GetAll().OrderByDescending(e => e.CourtTypeId).ToList();
    }
}