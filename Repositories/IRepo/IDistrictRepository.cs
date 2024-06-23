using BusinessObjects.Entities;

namespace Repositories.IRepo;

public interface IDistrictRepository
{
    List<District> GetAllDistrictsByCityId(int id);
    List<District> GetAllDistricts();
}