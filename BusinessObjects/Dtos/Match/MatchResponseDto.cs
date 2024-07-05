namespace BusinessObjects.Dtos.Match;

public class MatchResponseDto
{
    public int MatchId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? MatchDate { get; set; }
    public DateOnly? MatchDateOnly { get; set; }
    public string? MatchTime { get; set; }
    public int? CourtId { get; set; }
    public string? ClubName { get; set; }
    public string? Address { get; set; }
}