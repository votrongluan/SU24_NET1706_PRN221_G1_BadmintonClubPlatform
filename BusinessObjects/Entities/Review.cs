using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Review
{
    public int ReviewId { get; set; }

    public int Star { get; set; }

    public string? Comment { get; set; }

    public int ClubId { get; set; }

    public int UserId { get; set; }

    public DateOnly? ReviewDate { get; set; }

    public virtual Club Club { get; set; } = null!;

    public virtual Account User { get; set; } = null!;
}
