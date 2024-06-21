using BusinessObjects.Entities;
using DataAccessObjects;
using Repositories.IRepo;

namespace Repositories.Repo;

public class SlotRepository : ISlotRepository
{
    public void AddSlot(Slot slot) => SlotDao.Add(slot);

    public void DeleteSlot(int id)
    {
        throw new NotImplementedException();
    }

    public List<Slot> GetAllSlot()
    {
        return SlotDao.GetAll().ToList();
    }

    public Club GetSlotById(int id)
    {
        throw new NotImplementedException();
    }

    public void UpdateSlot(Club club)
    {
        throw new NotImplementedException();
    }
}