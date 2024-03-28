using System;

public class NTTExpertRatingsDTO
{
    public int Id { get; set; }
    public int Score { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public int ExpertId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
