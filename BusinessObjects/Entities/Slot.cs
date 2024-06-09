using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Slot
{
    public int SlotId { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public bool? Status { get; set; }

    public int ClubId { get; set; }

    public int? Price { get; set; }

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    public virtual Club Club { get; set; } = null!;
}
