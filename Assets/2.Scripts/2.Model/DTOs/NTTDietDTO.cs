using System;

public class NTTDietDTO
{
    public string Id {  get; set; }
    public string Name {  get; set; }
    public string Description {  get; set; }
    public float InitialWeight {  get; set; }
    public float TargetWeight {  get; set; }
    public float ActualWeight {  get; set; }
    public string ExpertId {  get; set; }
    public string UserId {  get; set; }
    public string CategoryId {  get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive {  get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
