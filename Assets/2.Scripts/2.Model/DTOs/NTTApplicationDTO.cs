using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTApplicationDTO : MonoBehaviour
{
    public int Id { get; set; }
    public string Status { get; set; }
    public string Type { get; set; }
    public string Image { get; set; }
    public string UserId { get; set; }
    public string ApprovedById { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}
