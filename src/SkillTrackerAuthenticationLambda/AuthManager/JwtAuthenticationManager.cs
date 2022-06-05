using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SkillTrackerAuthenticationLambda.Model;

namespace SkillTrackerAuthenticationLambda.AuthManager
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly string _key;
        private List<UserData> users = null;

        public JwtAuthenticationManager(string key)
        {
            _key = key;
            LoadJsonData();
        }

        private void LoadJsonData()
        {
            using StreamReader r = new StreamReader("userdata.json");
            string json = r.ReadToEnd();
            users = JsonConvert.DeserializeObject<List<UserData>>(json);
        }

        public Response Authenticate(string username, string password)
        {
            var user = users.Where(u => u.Username == username && u.Password == password).FirstOrDefault();
            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.Now.AddHours(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new Response
            {
                Token = tokenHandler.WriteToken(token),
                Role = user.Role
            };
        }
    }
}
