using Repositories.IRepo;

namespace Repositories.Repo;

public class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<IAccountRepository> _accountRepo = new(() => new AccountRepository());
    private readonly Lazy<IClubRepository> _clubRepo = new(() => new ClubRepository());
    private readonly Lazy<IAvailableBookingTypeRepository> _availableBookingTypeRepo = new(() => new AvailableBookingTypeRepository());
    private readonly Lazy<IBookingRepository> _bookingRepo = new(() => new BookingRepository());
    private readonly Lazy<IBookingDetailRepository> _bookingDetailRepo = new(() => new BookingDetailRepository());
    private readonly Lazy<IBookingTypeRepository> _bookingTypeRepo = new(() => new BookingTypeRepository());
    private readonly Lazy<ICityRepository> _cityRepo = new(() => new CityRepository());
    private readonly Lazy<ICourtRepository> _courtRepo = new(() => new CourtRepository());
    private readonly Lazy<ICourtTypeRepository> _courtTypeRepo = new(() => new CourtTypeRepository());
    private readonly Lazy<IDistrictRepository> _districtRepo = new(() => new DistrictRepository());
    private readonly Lazy<IMatchRepository> _matchRepo = new(() => new MatchRepository());
    private readonly Lazy<IReviewRepository> _reviewRepo = new(() => new ReviewRepository());
    private readonly Lazy<ISlotRepository> _slotRepo = new(() => new SlotRepository());

    public IAccountRepository Account => _accountRepo.Value;

    public IClubRepository Club => _clubRepo.Value;

    public IAvailableBookingTypeRepository AvailableBookingType => _availableBookingTypeRepo.Value;

    public IBookingRepository Booking => _bookingRepo.Value;

    public IBookingDetailRepository BookingDetail => _bookingDetailRepo.Value;

    public IBookingTypeRepository BookingType => _bookingTypeRepo.Value;

    public ICityRepository City => _cityRepo.Value;

    public ICourtRepository Court => _courtRepo.Value;

    public ICourtTypeRepository CourtType => _courtTypeRepo.Value;

    public IDistrictRepository District => _districtRepo.Value;

    public IMatchRepository Match => _matchRepo.Value;

    public IReviewRepository Review => _reviewRepo.Value;

    public ISlotRepository Slot => _slotRepo.Value;
}