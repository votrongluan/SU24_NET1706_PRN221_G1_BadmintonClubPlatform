using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Dtos.Match;

public class MatchCreateDto
{
    [Required(ErrorMessage = "Cần phải nhập tiêu đề thi đấu")]
    [MinLength(4, ErrorMessage = "Tối thiểu 4 kí tự")]
    [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Cần phải nhập nội dung thi đấu")]
    [MinLength(4, ErrorMessage = "Tối thiểu 4 kí tự")]
    [MaxLength(50, ErrorMessage = "Tối đa 500 kí tự")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "Cần phải chọn loại sân")]
    public int CourtTypeId { get; set; }

    [Required(ErrorMessage = "Cần phải chọn ngày")]
    public DateOnly MatchDate { get; set; }
    [Required(ErrorMessage = "Cần phải chọn giờ bắt đầu")]
    public TimeOnly StartTime { get; set; }
    [Required(ErrorMessage = "Cần phải chọn giờ kết thúc")]
    public TimeOnly EndTime { get; set; }
}