using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.DTOs.Auth
{
    public class RefreshTokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
