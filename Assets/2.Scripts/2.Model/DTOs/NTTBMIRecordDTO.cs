using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTBMIRecordDTO
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public float Weight { get; set; }
    public DateTime Date { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static NTTBMIRecordDTO FromJObject(JObject obj)
    {
        return JsonConvert.DeserializeObject<NTTBMIRecordDTO>(obj.ToString());
    }
}
