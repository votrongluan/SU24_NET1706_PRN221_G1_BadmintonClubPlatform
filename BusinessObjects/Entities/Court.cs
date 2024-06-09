using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Court
{
    public int CourtId { get; set; }

    public int CourtTypeId { get; set; }

    public int ClubId { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    public virtual Club Club { get; set; } = null!;

    public virtual CourtType CourtType { get; set; } = null!;
}
