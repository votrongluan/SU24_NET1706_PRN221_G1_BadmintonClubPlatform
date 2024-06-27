using BusinessObjects.Entities;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepo;

namespace Repositories.Repo;

public class AvailableBookingTypeRepository : IAvailableBookingTypeRepository
{
    public List<AvailableBookingType> GetAllAvailableBookingTypes()
    {
        return AvailableBookingTypeDao.GetAll().Include(e => e.BookingType).OrderByDescending(e => e.AvailableBookingTypeId).ToList();
    }

    public AvailableBookingType GetAvailableBookingTypeById(int availableBookingTypeId)
    {
        return GetAllAvailableBookingTypes().FirstOrDefault(e => e.AvailableBookingTypeId == availableBookingTypeId);
    }

    public void DeleteAvailableBookingType(int availableBookingTypeId)
    {
        var availableBookingType = GetAvailableBookingTypeById(availableBookingTypeId);
        AvailableBookingTypeDao.Delete(availableBookingType);
    }

    public void UpdateAvailableBookingType(AvailableBookingType availableBookingType)
    {
        AvailableBookingTypeDao.Update(availableBookingType);
    }

    public void AddAvailableBookingType(AvailableBookingType availableBookingType)
    {
        AvailableBookingTypeDao.Add(availableBookingType);
    }
}