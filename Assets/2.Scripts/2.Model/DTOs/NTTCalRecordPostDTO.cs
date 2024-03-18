using Newtonsoft.Json;

public class NTTCalRecordPostDTO
{
    [JsonProperty("nutrient")]
    public string Nutrient { get; set; }
    [JsonProperty("amount")]
    public int Amount { get; set; }
    [JsonProperty("unit")]
    public string Unit { get; set; }
    [JsonProperty("description")]
    public string Description { get; set; }

    public NTTCalRecordPostDTO(string nutrient, int amount, string unit, string description)
    {
        Nutrient = nutrient;
        Amount = amount;
        Unit = unit;
        Description = description;
    }
}
