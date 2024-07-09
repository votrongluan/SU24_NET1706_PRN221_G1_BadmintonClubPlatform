using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Dtos.Review
{
    public class CreateReviewDto
    {
        [Required(ErrorMessage = "Vui lòng chọn mức độ hài lòng")]
        public int Star { get; set; }
        public string? Comment { get; set; }
        public int ClubId { get; set; }
        public int UserId { get; set; }
        public DateOnly ReviewDate { get; set; }
    }
}
