using BusinessObjects.Entities;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepo;

namespace Repositories.Repo;

public class CityRepository : ICityRepository
{
    public List<City> GetAllCities()
    {
        return CityDao.GetAll().Include(d => d.Districts).ToList();
    }
}