using BusinessObjects.Entities;
using Repositories.IRepo;
using Services.IService;

namespace Services.Service;

public class SlotService : ISlotService
{
    private readonly IRepositoryManager _repo;

    public SlotService(IRepositoryManager repo)
    {
        _repo = repo;
    }

    public void AddSlot(Slot slot) => _repo.Slot.AddSlot(slot);

    public void DeleteSlot(int slotId)
    {
        throw new NotImplementedException();
    }

    public List<Slot> GetAllSlot()
    {
        return _repo.Slot.GetAllSlot().ToList();
    }

    public Club GetSlotById(int slotId)
    {
        throw new NotImplementedException();
    }

    public void UpdateSlot(Slot c)
    {
        throw new NotImplementedException();
    }
}