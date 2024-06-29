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

    public List<Slot> GetAllByClubId (int id)
    {
        return _repo.Slot.GetAllByClubId(id).ToList();
    }

    public List<Slot> GetAllSlot()
    {
        return _repo.Slot.GetAllSlot().ToList();
    }

    public Slot GetSlotById(int slotId)
    {
        return _repo.Slot.GetSlotById(slotId);
    }

    public void UpdateSlot(Slot c)
    {
        _repo.Slot.UpdateSlot(c);
    }
}