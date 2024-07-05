using BusinessObjects.Dtos.Booking;
using BusinessObjects.Entities;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace WebAppRazor.Mappers
{
    public static class BookingMapper
    {
        public static BookingResponseDto ToBooking(this Booking booking)
        {
            var formatDate = booking.BookingDetails.ElementAt(0).BookDate.Value.ToString("dd/MM/yyyy");
            var formatStartTime = booking.BookingDetails.ElementAt(0).Slot.StartTime.Value.Hour.ToString("D2") + ":" + booking.BookingDetails.ElementAt(0).Slot.StartTime.Value.Minute.ToString("D2");
            var formatEndTime = booking.BookingDetails.ElementAt(0).Slot.EndTime.Value.Hour.ToString("D2") + ":" + booking.BookingDetails.ElementAt(0).Slot.EndTime.Value.Minute.ToString("D2");

            return new BookingResponseDto()
            {
                BookingId = booking.BookingId,
                UserName = booking.User?.Username,
                ClubName = booking.Club?.ClubName,
                CourtId = booking.BookingDetails.FirstOrDefault()?.CourtId,
                BookingDate = formatDate,
                BookingTime = $"{formatStartTime} - {formatEndTime}",
                Address = booking.Club?.Address,
                CityName = booking.Club?.District?.City?.CityName,
                DistrictName = booking.Club?.District?.DistrictName,
                BookingTypeDescription = booking.BookingType?.Description,
            };
        }
    }
}
