using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Dtos.Court
{
    public class CreateCourtDto
    {
        public int CourtId { get; set; }

        [Required(ErrorMessage = "Hãy chọn loại sân")]
        public int CourtTypeId { get; set; }
        public int ClubId { get; set; }
    }
}
