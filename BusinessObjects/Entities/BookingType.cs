using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class BookingType
{
    public int BookingTypeId { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<AvailableBookingType> AvailableBookingTypes { get; set; } = new List<AvailableBookingType>();

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
