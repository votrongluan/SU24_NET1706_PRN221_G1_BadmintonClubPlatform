using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class City
{
    public int CityId { get; set; }

    public string CityName { get; set; } = null!;

    public virtual ICollection<District> Districts { get; set; } = new List<District>();
}
