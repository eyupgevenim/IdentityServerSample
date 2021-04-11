namespace Todo.MVC.Services
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using Todo.MVC.Models;

    public class IdentityParser : IIdentityParser<User>
    {
        public User Parse(IPrincipal principal)
        {
            // Pattern matching 'is' expression
            // assigns "claims" if "principal" is a "ClaimsPrincipal"
            if (principal is ClaimsPrincipal claims)
            {
                return new User
                {
                    Email = claims.Claims.FirstOrDefault(x => x.Type == "email")?.Value ?? "",
                    Id = claims.Claims.FirstOrDefault(x => x.Type == "sub")?.Value ?? "",
                    Name = claims.Claims.FirstOrDefault(x => x.Type == "name")?.Value ?? "",
                    PhoneNumber = claims.Claims.FirstOrDefault(x => x.Type == "phone_number")?.Value ?? "",
                };
            }
            throw new ArgumentException(message: "The principal must be a ClaimsPrincipal", paramName: nameof(principal));
        }
    }
}
