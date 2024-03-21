using Newtonsoft.Json;
using System;

public class NTTBMIRecordPostDTO
{
    [JsonProperty("weight")]
    public float Weight { get; set; }
    [JsonProperty("date")]
    public DateTime Date { get; set; }

    public NTTBMIRecordPostDTO() { }

    public NTTBMIRecordPostDTO(float weight, DateTime date)
    {
        Weight = weight;
        Date = date;
    }
}
