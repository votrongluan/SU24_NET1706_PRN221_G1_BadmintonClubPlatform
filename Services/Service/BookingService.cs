using System.Reflection.Metadata.Ecma335;
using BusinessObjects.Dtos.Booking;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Repositories.IRepo;
using Services.IService;
using static System.Reflection.Metadata.BlobBuilder;

namespace Services.Service;

public class BookingService : IBookingService
{
    private readonly IRepositoryManager _repo;

    public BookingService(IRepositoryManager repositoryManager)
    {
        _repo = repositoryManager;
    }

    public List<Booking> GetAllBookings()
    {
        return _repo.Booking.GetAllBookings();
    }

    public void AddBooking(Booking booking)
    {
        _repo.Booking.AddBooking(booking);
    }

    public void DeleteBooking(int bookingId)
    {
        _repo.Booking.DeleteBooking(bookingId);
    }

    public void DeleteBookingDetail(int bookingId)
    {
        var bookingDetails = _repo.BookingDetail.GetAllBookingDetails().Where(e => e.BookingId == bookingId);

        foreach (var bookingDetail in bookingDetails)
        {
            _repo.BookingDetail.DeleteBookingDetail(bookingDetail.BookingDetailId);
        }
    }

    public Booking GetBookingById(int bookingId)
    {
        return _repo.Booking.GetBookingById(bookingId);
    }

    public void UpdateBooking(Booking booking)
    {
        _repo.Booking.UpdateBooking(booking);
    }

    public (bool status, int bookId) BookLichOffline(DateOnly date, TimeOnly startTime, TimeOnly endTime, int courtId, int clubId, int userId)
    {
        var club = _repo.Club.GetClubById(clubId);
        var courtSchedule = _repo.BookingDetail.GetAllBookingDetails().Where(e =>
            e.BookDate == date && e.Court.CourtId == courtId).OrderBy(e => e.StartTime).ToList();

        bool canBook = false;

        if (courtSchedule.Any())
        {
            // 9-11, 12-14, 14-17 ...
            // 7-9
            if (endTime <= courtSchedule[0].StartTime || startTime >= courtSchedule[courtSchedule.Count - 1].EndTime)
            {
                canBook = true;
            }

            if (courtSchedule.Count == 1)
            {
                canBook = false;
            }
            else
            {
                for (int i = 1; i < courtSchedule.Count - 1; i++)
                {
                    if (courtSchedule[i - 1].EndTime <= startTime && courtSchedule[i].StartTime <= endTime)
                    {
                        canBook = true;
                        break;
                    }
                }
            }
        }
        else
        {
            canBook = true;
        }

        if (canBook)
        {
            var slots = _repo.Slot.GetAllByClubId(clubId);

            Booking booking = new()
            {
                BookingTypeId = (int)BookingTypeEnum.LichNgay,
                ClubId = clubId,
                PaymentStatus = false,
                UserId = userId,
                TotalPrice = CalculatePrice(slots, startTime, endTime, (int)club.DefaultPricePerHour)
            };

            _repo.Booking.AddBooking(booking);

            BookingDetail bookingDetail = new()
            {
                BookingId = booking.BookingId,
                BookDate = date,
                CourtId = courtId,
                StartTime = startTime,
                EndTime = endTime,
            };

            _repo.BookingDetail.AddBookingDetail(bookingDetail);

            return (true, booking.BookingId);
        }

        return (false, -1);
    }

    public (bool status, int bookId) BookLichThiDau(BookingRequestDto dto)
    {
        var availableCourt = _repo.Court.GetCourtsByClubId(dto.ClubId).ToList();
        var bookingDetailInSlotAndDate = _repo.BookingDetail.GetAllBookingDetails().Where(e =>
            e.BookDate == dto.BookDate && e.Court.CourtTypeId == dto.CourtTypeId).OrderBy(e => e.StartTime);
        Court selectedCourt = null;

        foreach (var court in availableCourt)
        {
            var courtSchedule = bookingDetailInSlotAndDate.Where(e => e.CourtId == court.CourtId).ToList();

            if (courtSchedule.Any())
            {
                // 9-11, 12-14, 14-17 ...
                // 7-9
                if (dto.EndTime <= courtSchedule[0].StartTime || dto.StartTime >= courtSchedule[courtSchedule.Count - 1].EndTime)
                {
                    selectedCourt = court;
                    break;
                }

                if (courtSchedule.Count == 1) continue;

                bool canBook = false;

                for (int i = 1; i < courtSchedule.Count - 1; i++)
                {
                    if (courtSchedule[i - 1].EndTime <= dto.StartTime && courtSchedule[i].StartTime <= dto.EndTime)
                    {
                        canBook = true;
                        break;
                    }
                }

                if (canBook)
                {
                    selectedCourt = court;
                    break;
                }
            }
            else
            {
                selectedCourt = court;
                break;
            }
        }

        if (selectedCourt != null)
        {
            var slots = _repo.Slot.GetAllByClubId(dto.ClubId);

            Booking booking = new()
            {
                BookingTypeId = dto.BookingTypeId,
                ClubId = dto.ClubId,
                PaymentStatus = false,
                UserId = dto.UserId,
                TotalPrice = 0,
            };

            _repo.Booking.AddBooking(booking);

            BookingDetail bookingDetail = new()
            {
                BookingId = booking.BookingId,
                BookDate = dto.BookDate,
                CourtId = selectedCourt.CourtId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
            };

            _repo.BookingDetail.AddBookingDetail(bookingDetail);

            return (true, booking.BookingId);
        }

        return (false, -1);
    }

    private int CalculatePrice(List<Slot> slots, TimeOnly startTime, TimeOnly endTime, int defaultPrice)
    {
        var total = 0;

        foreach (var slot in slots)
        {
            var slotStartTime = (TimeOnly)slot.StartTime!;
            var slotEndTime = (TimeOnly)slot.EndTime!;

            if (startTime >= slot.StartTime && startTime <= slot.EndTime)
            {
                if (endTime > slot.EndTime)
                {
                    int passMinute = (slotEndTime.Hour * 60 + slotEndTime.Minute) -
                                     (startTime.Hour * 60 + startTime.Minute);
                    total += passMinute * ((int)slot.Price! / 60);
                    startTime = new TimeOnly(slotEndTime.Hour, slotEndTime.Minute);
                }
                else
                {
                    int passMinute = (endTime.Hour * 60 + endTime.Minute) -
                                     (startTime.Hour * 60 + startTime.Minute);
                    total += passMinute * ((int)slot.Price! / 60);
                    startTime = endTime;
                    break;
                }
            }
            else if (endTime >= slot.StartTime && endTime <= slot.EndTime)
            {
                int passMinute = (endTime.Hour * 60 + endTime.Minute) -
                                 (slotStartTime.Hour * 60 + slotStartTime.Minute);
                total += passMinute * ((int)slot.Price! / 60);
                endTime = new TimeOnly(slotStartTime.Hour, slotStartTime.Minute);
            }
        }

        int lastMinute = (endTime.Hour * 60 + endTime.Minute) -
                         (startTime.Hour * 60 + startTime.Minute);
        total += lastMinute * (defaultPrice / 60);

        var tienVND = total / 1000;
        var sodu = total % (tienVND * 1000);
        total = sodu > 0 ? tienVND * 1000 + 1000 : tienVND * 1000;

        return total;
    }

    public (bool status, int bookId) BookLichCoDinh(BookingRequestDto dto)
    {
        var availableCourt = _repo.Court.GetCourtsByClubId(dto.ClubId).ToList();
        Court selectedCourt = null;

        foreach (var court in availableCourt)
        {
            var date = dto.BookDate;
            bool isAvailable = true;

            for (int week = 1; week <= dto.WeekCount; week++)
            {
                var bookingDetailInSlotAndDate = _repo.BookingDetail.GetAllBookingDetails().Where(e =>
                    e.BookDate == date && e.Court.CourtTypeId == dto.CourtTypeId).OrderBy(e => e.StartTime);
                var courtSchedule = bookingDetailInSlotAndDate.Where(e => e.CourtId == court.CourtId).ToList();

                if (courtSchedule.Any())
                {
                    // 9-11, 12-14, 14-17 ...
                    // 7-9
                    if (dto.EndTime <= courtSchedule[0].StartTime || dto.StartTime >= courtSchedule[courtSchedule.Count - 1].EndTime)
                    {
                        continue;
                    }

                    if (courtSchedule.Count == 1)
                    {
                        isAvailable = false;
                        break;
                    };

                    bool canBook = false;

                    for (int i = 1; i < courtSchedule.Count - 1; i++)
                    {
                        if (courtSchedule[i - 1].EndTime <= dto.StartTime && courtSchedule[i].StartTime <= dto.EndTime)
                        {
                            canBook = true;
                            break;
                        }
                    }

                    if (!canBook)
                    {
                        isAvailable = false;
                        break;
                    }
                }
            }

            if (isAvailable)
            {
                selectedCourt = court;
                break;
            }
        }

        if (selectedCourt != null)
        {
            var slots = _repo.Slot.GetAllByClubId(dto.ClubId);

            Booking booking = new()
            {
                BookingTypeId = dto.BookingTypeId,
                ClubId = dto.ClubId,
                PaymentStatus = false,
                UserId = dto.UserId,
                TotalPrice = CalculatePrice(slots, dto.StartTime, dto.EndTime, dto.DefaultPrice) * dto.WeekCount,
            };

            _repo.Booking.AddBooking(booking);

            var bookDate = dto.BookDate;

            for (int i = 1; i <= dto.WeekCount; i++)
            {
                BookingDetail bookingDetail = new()
                {
                    BookingId = booking.BookingId,
                    BookDate = bookDate,
                    CourtId = selectedCourt.CourtId,
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime,
                };

                _repo.BookingDetail.AddBookingDetail(bookingDetail);
                bookDate = bookDate.AddDays(7);
            }

            return (true, booking.BookingId);
        }

        return (false, -1);
    }

    public (bool status, int bookId) BookLichNgay(BookingRequestDto dto)
    {
        var availableCourt = _repo.Court.GetCourtsByClubId(dto.ClubId).ToList();
        var bookingDetailInSlotAndDate = _repo.BookingDetail.GetAllBookingDetails().Where(e =>
            e.BookDate == dto.BookDate && e.Court.CourtTypeId == dto.CourtTypeId).OrderBy(e => e.StartTime);
        Court selectedCourt = null;

        foreach (var court in availableCourt)
        {
            var courtSchedule = bookingDetailInSlotAndDate.Where(e => e.CourtId == court.CourtId).ToList();

            if (courtSchedule.Any())
            {
                // 9-11, 12-14, 14-17 ...
                // 7-9
                if (dto.EndTime <= courtSchedule[0].StartTime || dto.StartTime >= courtSchedule[courtSchedule.Count - 1].EndTime)
                {
                    selectedCourt = court;
                    break;
                }

                if (courtSchedule.Count == 1) continue;

                bool canBook = false;

                for (int i = 1; i < courtSchedule.Count - 1; i++)
                {
                    if (courtSchedule[i - 1].EndTime <= dto.StartTime && courtSchedule[i].StartTime <= dto.EndTime)
                    {
                        canBook = true;
                        break;
                    }
                }

                if (canBook)
                {
                    selectedCourt = court;
                    break;
                }
            }
            else
            {
                selectedCourt = court;
                break;
            }
        }

        if (selectedCourt != null)
        {
            var slots = _repo.Slot.GetAllByClubId(dto.ClubId);

            Booking booking = new()
            {
                BookingTypeId = dto.BookingTypeId,
                ClubId = dto.ClubId,
                PaymentStatus = false,
                UserId = dto.UserId,
                TotalPrice = CalculatePrice(slots, dto.StartTime, dto.EndTime, dto.DefaultPrice),
            };

            _repo.Booking.AddBooking(booking);

            BookingDetail bookingDetail = new()
            {
                BookingId = booking.BookingId,
                BookDate = dto.BookDate,
                CourtId = selectedCourt.CourtId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
            };

            _repo.BookingDetail.AddBookingDetail(bookingDetail);

            return (true, booking.BookingId);
        }

        return (false, -1);
    }

    public List<Booking> GetAllBookingsWithBookingDetails()
    {
        return _repo.Booking.GetAllBookingsWithBookingDetails();
    }
}