using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTDietDTO : MonoBehaviour
{
    public string Id {  get; set; }
    public string Name {  get; set; }
    public string Description {  get; set; }
    public string ExpertId {  get; set; }
    public string UserId {  get; set; }
    public bool IsPrivate {  get; set; }
    public string CategoryId {  get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}
