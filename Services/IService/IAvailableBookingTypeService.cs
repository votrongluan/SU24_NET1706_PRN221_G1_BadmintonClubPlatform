using BusinessObjects.Entities;

namespace Services.IService;

public interface IAvailableBookingTypeService
{
    List<AvailableBookingType> GetAvailableBookingTypes();
    List<AvailableBookingType> GetAvailableBookingTypesByClubId(int clubId);
    void AddAvailableBookingType(AvailableBookingType availableBookingType);
    void DeleteAvailableBookingType(int availableBookingTypeId);
    AvailableBookingType GetAvailableBookingTypeById(int availableBookingTypeId);
    void UpdateAvailableBookingType(AvailableBookingType booking);
}