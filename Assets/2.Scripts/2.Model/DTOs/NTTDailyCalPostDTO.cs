using Newtonsoft.Json;
using System;

public class NTTDailyCalPostDTO
{
    [JsonProperty("description")]
    public string Description { get; set; }
    [JsonProperty("date")]
    public DateTime Date { get; set; }

    public NTTDailyCalPostDTO(string description, DateTime date)
    {
        Description = description;
        Date = date;
    }
}
