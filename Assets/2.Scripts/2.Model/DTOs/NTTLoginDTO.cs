using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTLoginDTO
{
    [JsonProperty("email")]
    public string Email {  get; set; }
    [JsonProperty("password")]
    public string Password { get; set; }

    public NTTLoginDTO(string email, string password)
    {
        Email = email;
        Password = password;
    }
}
