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

            var slot = firstBookingDetail.Slot;
            debug.AppendLine($"Slot null? {slot == null}");

            string bookingTime = slot == null ? "No slot assigned" : "No time information";

            if (slot != null && slot.StartTime.HasValue && slot.EndTime.HasValue)
            {
                var formatStartTime = slot.StartTime.Value.ToString("HH:mm");
                var formatEndTime = slot.EndTime.Value.ToString("HH:mm");
                bookingTime = $"{formatStartTime} - {formatEndTime}";
                debug.AppendLine($"Formatted BookingTime: {bookingTime}");
            }
            else
            {
                debug.AppendLine("Unable to format BookingTime due to missing or incomplete slot information");
            }

            return new BookingHistoryResponseDto()
            {
                BookingId = booking.BookingId,
                FullName = booking.User?.Fullname,
                ClubName = booking.Club?.ClubName,
                CourtId = firstBookingDetail.CourtId,
                BookingDate = firstBookingDetail.BookDate?.ToString("dd/MM/yyyy") ?? "No date",
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
