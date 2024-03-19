using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTMedicalConditionDTO
{
    public string Id {  get; set; }
    public string Name {  get; set; }
    public string Description {  get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<NTTUserDTO> Users { get; set; }
}
