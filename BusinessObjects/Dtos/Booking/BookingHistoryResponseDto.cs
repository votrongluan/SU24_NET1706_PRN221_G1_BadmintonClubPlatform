using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Dtos.Booking
{
    public class BookingHistoryResponseDto
    {
        public int? ClubId { get; set; }
        public int? BookingId { get; set; }
        public string? FullName { get; set; }
        public string? ClubName { get; set; }
        public int? CourtId { get; set; }
        public string? BookingDate { get; set; }
        public string? BookingTime { get; set; }
        public string BookingTimeDebug { get; set; }
        public string? Address { get; set; }
        public string? CityName { get; set; }
        public string? DistrictName { get; set; }
        public string? BookingTypeDescription { get; set; }
    }
}
