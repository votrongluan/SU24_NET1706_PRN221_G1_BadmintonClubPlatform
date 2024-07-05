using BusinessObjects.Entities;

namespace Repositories.IRepo;

public interface IAvailableBookingTypeRepository
{
    List<AvailableBookingType> GetAllAvailableBookingTypes();
    List<AvailableBookingType> GetAllAvailableBookingTypesByClubId(int clubId);
    AvailableBookingType GetAvailableBookingTypeById(int availableBookingTypeId);
    void DeleteAvailableBookingType(int availableBookingTypeId);
    void UpdateAvailableBookingType(AvailableBookingType availableBookingType);
    void AddAvailableBookingType(AvailableBookingType availableBookingType);
}