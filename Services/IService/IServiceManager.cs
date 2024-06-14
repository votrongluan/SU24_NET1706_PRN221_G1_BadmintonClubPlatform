namespace Services.IService;

public interface IServiceManager
{
    IAuthenticationService AuthenticationService { get; }
    IClubService ClubService { get; }
    ICityService CityService { get; }
    IDistrictService DistrictService { get; }
    IAccountService AccountService { get; }
    IBookingService BookingService { get; }
    ICourtService CourtService { get; }
    IMatchService MatchService { get; }
    IReviewService ReviewService { get; }
    ISlotService SlotService { get; }
}   