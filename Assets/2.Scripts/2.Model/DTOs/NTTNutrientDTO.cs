using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTNutrientDTO : MonoBehaviour
{
    public string Id {  get; set; }
    public string Name {  get; set; }
    public int Amount {  get; set; }
    public string UnitName {  get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
