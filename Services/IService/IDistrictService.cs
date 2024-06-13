using BusinessObjects.Entities;

namespace Services.IService;

public interface IDistrictService
{
    List<District> GetAllDistrictsByCityId(int id);
    List<District> GetAllDistricts();
}