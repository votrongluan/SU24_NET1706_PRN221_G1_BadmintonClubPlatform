using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class AvailableBookingType
{
    public int AvailableBookingTypeId { get; set; }

    public int ClubId { get; set; }

    public int BookingTypeId { get; set; }

    public virtual BookingType BookingType { get; set; } = null!;

    public virtual Club Club { get; set; } = null!;
}
