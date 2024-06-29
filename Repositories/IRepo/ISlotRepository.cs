using BusinessObjects.Entities;

namespace Repositories.IRepo;

public interface ISlotRepository
{
    List<Slot> GetAllSlot();
    //Club GetClubByIdNotInclude(int id);
    Slot GetSlotById(int id);
    void DeleteSlot(int id);
    void UpdateSlot(Slot slot);
    void AddSlot(Slot slot);
    List<Slot> GetAllByClubId(int id);
}