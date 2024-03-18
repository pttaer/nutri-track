using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTPageResultDTO<T>
{
    public List<T> Results { get; set; }
    public int TotalPages { get; set; }
    public int Page { get; set; }
    public int Limit { get; set; }
    public int TotalResults { get; set; }
}
