using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ViolationManagement.Helper
{
    public static class UserSession
    {
        public static ClaimsPrincipal? CurrentUser { get; set; }

        public static void SetUser(int userId, string fullName, string email, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, fullName),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var identity = new ClaimsIdentity(claims, "CustomAuth");
            CurrentUser = new ClaimsPrincipal(identity);
        }


        public static void Logout()
        {
            CurrentUser = null;
        }

        public static bool IsLoggedIn => CurrentUser != null;

        public static string? GetClaim(string type) =>
            CurrentUser?.FindFirst(type)?.Value;
    }
}