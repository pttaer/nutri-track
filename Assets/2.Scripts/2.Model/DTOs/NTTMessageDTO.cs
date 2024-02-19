using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTMessageDTO : MonoBehaviour
{
    public string Id {  get; set; }
    public string ChatId {  get; set; }
    public string SenderId {  get; set; }
    public string Content {  get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}
