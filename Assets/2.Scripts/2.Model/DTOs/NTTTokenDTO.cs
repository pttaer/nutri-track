using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTTokenDTO
{
    public AccessToken Access { get; set; }
    public RefreshToken Refresh { get; set; }

    public class AccessToken
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }

    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }

}
