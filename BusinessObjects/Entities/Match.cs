using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Match
{
    public int MatchId { get; set; }

    public int BookingId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public virtual Booking Booking { get; set; } = null!;
}
