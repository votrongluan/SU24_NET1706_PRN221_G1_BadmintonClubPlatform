namespace Repositories.IRepo;

public interface IRepositoryManager
{
    IAccountRepository Account { get; }
    IClubRepository Club { get; }
    IAvailableBookingTypeRepository AvailableBookingType { get; }
    IBookingRepository Booking { get; }
    IBookingDetailRepository BookingDetail { get; }
    IBookingTypeRepository BookingType { get; }
    ICityRepository City { get; }
    ICourtRepository Court { get; }
    ICourtTypeRepository CourtType { get; }
    IDistrictRepository District { get; }
    IMatchRepository Match { get; }
    IReviewRepository Review { get; }
    ISlotRepository Slot { get; }
}