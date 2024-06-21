using BusinessObjects.Entities;

namespace Services.IService;

public interface ISlotService
{
    List<Slot> GetAllSlot();
    void AddSlot(Slot slot);
    void DeleteSlot(int slotId);
    Club GetSlotById(int slotId);
    void UpdateSlot(Slot c);
}