using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class CourtType
{
    public int CourtTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public string? TypeDescription { get; set; }

    public virtual ICollection<Court> Courts { get; set; } = new List<Court>();
}
