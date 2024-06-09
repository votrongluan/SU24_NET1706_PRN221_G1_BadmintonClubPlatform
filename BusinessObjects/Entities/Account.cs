using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Account
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Fullname { get; set; }

    public string? Email { get; set; }

    public string? UserPhone { get; set; }

    public string? AvatarLink { get; set; }

    public string? Role { get; set; }

    public int? ClubManageId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Club? ClubManage { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
