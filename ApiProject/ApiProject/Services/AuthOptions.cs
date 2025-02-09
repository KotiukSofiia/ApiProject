using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiProject.Services
{
	public class AuthOptions
	{
        public const string ISSUER = "ApiProject"; 
        public const string AUDIENCE = "ApiProjectClient"; 
        const string KEY = "SofiiaKotiuk_ipz_23_1";   
        public const int LIFETIME = 5; 
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}

