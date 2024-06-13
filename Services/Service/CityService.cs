using BusinessObjects.Entities;
using Repositories.IRepo;
using Services.IService;

namespace Services.Service;

public class CityService : ICityService
{
    private readonly IRepositoryManager _repo;

    public CityService(IRepositoryManager repo)
    {
        _repo = repo;
    }

    public List<City> GetAllCities()
    {
        return _repo.City.GetAllCities();
    }
}