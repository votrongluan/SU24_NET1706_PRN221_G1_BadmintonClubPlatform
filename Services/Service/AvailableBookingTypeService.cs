using BusinessObjects.Entities;
using Repositories.IRepo;
using Services.IService;

namespace Services.Service;

public class AvailableBookingTypeService : IAvailableBookingTypeService
{
    private readonly IRepositoryManager _repo;

    public AvailableBookingTypeService(IRepositoryManager repo)
    {
        _repo = repo;
    }

    public List<AvailableBookingType> GetAvailableBookingTypes()
    {
        return _repo.AvailableBookingType.GetAllAvailableBookingTypes();
    }

    public void AddAvailableBookingType(AvailableBookingType availableBookingType)
    {
        _repo.AvailableBookingType.AddAvailableBookingType(availableBookingType);
    }

    public void DeleteAvailableBookingType(int availableBookingTypeId)
    {
        _repo.AvailableBookingType.DeleteAvailableBookingType(availableBookingTypeId);
    }

    public AvailableBookingType GetAvailableBookingTypeById(int availableBookingTypeId)
    {
        return _repo.AvailableBookingType.GetAvailableBookingTypeById(availableBookingTypeId);
    }

    public void UpdateAvailableBookingType(AvailableBookingType availableBookingType)
    {
        _repo.AvailableBookingType.UpdateAvailableBookingType(availableBookingType);
    }
}