using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTUserDTO
{
    public UserDTO User { get; set; }
    public NTTTokenDTO Tokens { get; set; }
    
    public class UserDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public DateTime Dob { get; set; }
        public float Height { get; set; }
        public string Gender { get; set; }
        public string Role { get; set; }
        public string AccountType { get; set; }
        public string ExpertProfileId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public NTTUserDTO() { }
}
