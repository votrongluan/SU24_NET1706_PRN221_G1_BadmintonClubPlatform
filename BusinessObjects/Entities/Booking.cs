using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Booking
{
    public int BookingId { get; set; }

    public int UserId { get; set; }

    public int ClubId { get; set; }

    public int BookingTypeId { get; set; }

    public bool? PaymentStatus { get; set; }

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    public virtual BookingType BookingType { get; set; } = null!;

    public virtual Club Club { get; set; } = null!;

    public virtual Match? Match { get; set; }

    public virtual Account User { get; set; } = null!;
}
