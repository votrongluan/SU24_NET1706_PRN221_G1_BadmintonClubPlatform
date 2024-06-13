namespace BusinessObjects.Dtos.Club;

public class ResponseClubDto
{
    public int ClubId { get; set; }
    public string ClubName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string? ClubPhone { get; set; }
}