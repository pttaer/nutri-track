using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTTokenDTO : MonoBehaviour
{
    public int TokenID { get; set; }
    public string Value { get; set; }
    public string Type { get; set; }
    public int UserID { get; set; }
    public DateTime LastDeposit { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}
