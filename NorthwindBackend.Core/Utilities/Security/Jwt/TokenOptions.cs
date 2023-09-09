﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NorthwindBackend.Core.Utilities.Security.Jwt
{
    public class TokenOptions
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int AccesTokenExpiration { get; set; }
        public string SecurityKey { get; set; }
    }
}