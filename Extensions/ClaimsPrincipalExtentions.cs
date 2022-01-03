using System.Security.Claims;

namespace TrueStoryMVC.Extensions
{
    public static class ClaimsPrincipalExtentions
    {
        public static bool AtLeastAdmin(this ClaimsPrincipal principal)
        {
            if (principal == null)
                return false;

            return principal.IsInRole("Admin");
        }
    }
}
