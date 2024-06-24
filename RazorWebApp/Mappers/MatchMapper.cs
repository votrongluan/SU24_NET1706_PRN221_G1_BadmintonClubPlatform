using BusinessObjects.Dtos.Match;
using BusinessObjects.Entities;

namespace WebAppRazor.Mappers;

public static class MatchMapper
{
    public static MatchResponseDto ToMatch(this Match match)
    {
        var formatDate = match.Booking.BookingDetails.ElementAt(0).BookDate.Value.ToString("dd/MM/yyyy");
        var formatStartTime = match.Booking.BookingDetails.ElementAt(0).Slot.StartTime.Value.Hour.ToString("D2") + ":" + match.Booking.BookingDetails.ElementAt(0).Slot.StartTime.Value.Minute.ToString("D2");
        var formatEndTime = match.Booking.BookingDetails.ElementAt(0).Slot.EndTime.Value.Hour.ToString("D2") + ":" + match.Booking.BookingDetails.ElementAt(0).Slot.EndTime.Value.Minute.ToString("D2");

        return new MatchResponseDto()
        {
            MatchId = match.MatchId,
            CourtId = match.Booking.BookingDetails.ElementAt(0).CourtId,
            Description = match.Description,
            MatchDate = formatDate,
            MatchTime = $"{formatStartTime} - {formatEndTime}",
            Title = match.Title,
        };
    }
}