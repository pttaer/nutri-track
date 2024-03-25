using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTDailyCalDTO : MonoBehaviour
{
    public string Id {  get; set; }
    public DateTime Date {  get; set; }
    public string Description {  get; set; }
    public string UserId {  get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static NTTDailyCalDTO FromJObject(JObject obj)
    {
        return JsonConvert.DeserializeObject<NTTDailyCalDTO>(obj.ToString());
    }
}
