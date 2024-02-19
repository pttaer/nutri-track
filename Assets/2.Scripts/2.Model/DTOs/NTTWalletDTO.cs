using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTWalletDTO : MonoBehaviour
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public float Balance { get; set; }
    public float PreviousBalance { get; set; }
    public DateTime LastDeposit { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}
