using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Club
{
    public int ClubId { get; set; }

    public string ClubName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public int DistrictId { get; set; }

    public string? FanpageLink { get; set; }

    public string? AvatarLink { get; set; }

    public TimeOnly? OpenTime { get; set; }

    public TimeOnly? CloseTime { get; set; }

    public string? ClubEmail { get; set; }

    public string? ClubPhone { get; set; }

    public string? ClientId { get; set; }

    public string? ApiKey { get; set; }

    public string? ChecksumKey { get; set; }

    public bool? Status { get; set; }

    public int? TotalStar { get; set; }

    public int? TotalReview { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<AvailableBookingType> AvailableBookingTypes { get; set; } = new List<AvailableBookingType>();

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Court> Courts { get; set; } = new List<Court>();

    public virtual District District { get; set; } = null!;

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();
}
