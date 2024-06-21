using BusinessObjects.Entities;

namespace Repositories.IRepo;

public interface ISlotRepository
{
    List<Slot> GetAllSlot();
    //Club GetClubByIdNotInclude(int id);
    Club GetSlotById(int id);
    void DeleteSlot(int id);
    void UpdateSlot(Club club);
    void AddSlot(Slot slot);
}