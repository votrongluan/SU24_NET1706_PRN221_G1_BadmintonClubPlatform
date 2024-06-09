using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class BookingDetail
{
    public int BookingDetailId { get; set; }

    public int BookingId { get; set; }

    public int SlotId { get; set; }

    public int CourtId { get; set; }

    public DateOnly? BookDate { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Court Court { get; set; } = null!;

    public virtual Slot Slot { get; set; } = null!;
}
