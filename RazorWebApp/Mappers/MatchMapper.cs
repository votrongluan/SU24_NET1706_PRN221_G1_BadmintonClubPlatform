using BusinessObjects.Dtos.Match;
using BusinessObjects.Entities;

namespace WebAppRazor.Mappers;

public static class MatchMapper
{
    public static MatchResponseDto ToMatchResponseDto(this Match match)
    {
        var bookingDetails = match.Booking?.BookingDetails.ElementAtOrDefault(0);
        var formatDate = bookingDetails?.BookDate ?? DateOnly.MinValue;
        var formatStartTime = match.Booking.BookingDetails.ElementAt(0).Slot.StartTime.Value.Hour.ToString("D2") + ":" + match.Booking.BookingDetails.ElementAt(0).Slot.StartTime.Value.Minute.ToString("D2");
        var formatEndTime = match.Booking.BookingDetails.ElementAt(0).Slot.EndTime.Value.Hour.ToString("D2") + ":" + match.Booking.BookingDetails.ElementAt(0).Slot.EndTime.Value.Minute.ToString("D2");

        return new MatchResponseDto()
        {
            MatchId = match.MatchId,
            CourtId = match.Booking.BookingDetails.ElementAt(0).CourtId,
            Description = match.Description,
            ClubName = match.Booking?.Club?.ClubName,
            Address = $"{match.Booking?.Club?.Address}, {match.Booking?.Club?.District.DistrictName}, {match.Booking?.Club?.District.City.CityName}",
            MatchDate = formatDate,
            MatchTime = $"{formatStartTime} - {formatEndTime}",
            Title = match.Title,
        };
    }
}