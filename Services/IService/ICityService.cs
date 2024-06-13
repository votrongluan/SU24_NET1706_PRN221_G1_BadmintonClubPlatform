using BusinessObjects.Entities;

namespace Services.IService;

public interface ICityService
{
    List<City> GetAllCities();
}