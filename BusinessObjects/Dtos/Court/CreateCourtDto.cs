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
        [Required(ErrorMessage = "Phải nhập số lượng")]
        [Range(1,5,ErrorMessage = "Tối thiểu 1 sân, tối đa 5")]
        public int Quantity { get; set; }
        public int CourtId { get; set; }

        [Required(ErrorMessage = "Hãy chọn loại sân")]
        public int CourtTypeId { get; set; }
        public int ClubId { get; set; }
    }
}
