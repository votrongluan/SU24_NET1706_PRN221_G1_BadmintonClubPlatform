using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Dtos.Court
{
    public class ResponseCourtDto
    {
        public int CourtId { get; set; }
        public int CourtTypeId { get; set; }
        public int ClubId { get; set; }
        public bool? Status { get; set; }
        public string? TypeName { get; set; }
    }
}
