using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTMedicalConditionDTO : MonoBehaviour
{
    public string Id {  get; set; }
    public string Name {  get; set; }
    public string Description {  get; set; }
    public string High {  get; set; }
    public string Low {  get; set; }
    public string Avoid {  get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}
