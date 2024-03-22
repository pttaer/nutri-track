using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTCalRecordDTO : MonoBehaviour
{
    public string Id {  get; set; }
    public string Nutrient {  get; set; }
    public int Amount {  get; set; }
    public string Unit {  get; set; }
    public string DayCalId {  get; set; }
    public string Description {  get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static NTTCalRecordDTO FromJObject(JObject obj)
    {
        return JsonConvert.DeserializeObject<NTTCalRecordDTO>(obj.ToString());
    }
}
