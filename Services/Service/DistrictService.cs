using BusinessObjects.Entities;
using Repositories.IRepo;
using Services.IService;

namespace Services.Service;

public class DistrictService : IDistrictService
{
    private readonly IRepositoryManager _repo;

    public DistrictService(IRepositoryManager repo)
    {
        _repo = repo;
    }
    public List<District> GetAllDistrictsByCityId(int id)
    {
        return _repo.District.GetAllDistrictsByCityId(id);
    }

    public List<District> GetAllDistricts()
    {
        return _repo.District.GetAllDistricts();
    }
}