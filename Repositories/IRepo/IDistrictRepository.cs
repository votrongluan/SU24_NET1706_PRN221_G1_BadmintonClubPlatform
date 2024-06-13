using BusinessObjects.Entities;

namespace Repositories.IRepo;

public interface IDistrictRepository
{
    List<District> GetAllDistrcitsByCityId(int id);
    List<District> GetAllDistricts();
}