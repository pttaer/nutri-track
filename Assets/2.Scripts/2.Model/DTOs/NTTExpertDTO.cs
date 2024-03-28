using System;
using System.Collections.Generic;

public class NTTExpertDTO
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public DateTime Dob { get; set; }
    public string Avatar { get; set; }
    public string Gender { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public NTTExpertProfileDTO ExpertProfile { get; set; }
    public List<NTTExpertRatingsDTO> ExpertRatings { get; set; }
}