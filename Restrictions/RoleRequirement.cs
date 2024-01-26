using Microsoft.AspNetCore.Authorization;
using Shop_Express.Models;

namespace Shop_Express.Restrictions
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public Role Role { get; }

        public RoleRequirement(Role requiredRole)
        {
            Role = requiredRole;
        }
    }
}
