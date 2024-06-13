using BusinessObjects.Entities;
using DataAccessObjects;
using Repositories.IRepo;

namespace Repositories.Repo;

public class DistrictRepository : IDistrictRepository
{
    public List<District> GetAllDistrcitsByCityId(int id)
    {
        return DistrictDao.FindByCondition(e => e.CityId == id).ToList();
    }

    public List<District> GetAllDistricts()
    {
        return DistrictDao.GetAll().ToList();
    }
}