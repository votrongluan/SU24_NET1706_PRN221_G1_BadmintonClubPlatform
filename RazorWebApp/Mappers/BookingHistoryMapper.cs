using BusinessObjects.Dtos.Booking;
using BusinessObjects.Entities;

namespace WebAppRazor.Mappers
{
    public static class BookingHistoryMapper
    {
        public static BookingHistoryResponseDto ToBookingHistory(this Booking booking)
        {
            var debug = new System.Text.StringBuilder();
            debug.AppendLine("BookingTime Debugging:");

            var firstBookingDetail = booking.BookingDetails.FirstOrDefault();
            if (firstBookingDetail == null)
            {
                return new BookingHistoryResponseDto
                {
                    BookingId = booking.BookingId,
                    BookingTime = "No booking details",
                    BookingTimeDebug = debug.ToString()
                };
            }

            var formatStartTime = firstBookingDetail.StartTime.Value.ToString("HH:mm");
            var formatEndTime = firstBookingDetail.EndTime.Value.ToString("HH:mm");
            var bookingTime = $"{formatStartTime} - {formatEndTime}";
            debug.AppendLine($"Formatted BookingTime: {bookingTime}");

            var bookDateStringArray = booking.BookingDetails.Select(e => e.BookDate?.ToString("dd/MM/yyyy") ?? "").ToArray();
            var bookDate = string.Join(" , ", bookDateStringArray);

            return new BookingHistoryResponseDto()
            {
                ClubId = booking.ClubId,
                BookingId = booking.BookingId,
                FullName = booking.User?.Fullname,
                ClubName = booking.Club?.ClubName,
                CourtId = firstBookingDetail.CourtId,
                BookingDate = bookDate,
                BookingTime = bookingTime,
                BookingTimeDebug = debug.ToString(),
                Address = booking.Club?.Address,
                CityName = booking.Club?.District?.City?.CityName,
                DistrictName = booking.Club?.District?.DistrictName,
                BookingTypeDescription = booking.BookingType?.Description,
            };
        }
    }
}
