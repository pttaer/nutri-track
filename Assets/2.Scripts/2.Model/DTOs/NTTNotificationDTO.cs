using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTNotificationDTO : MonoBehaviour
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime ReadAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}
