using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Users.Models;
using System.Collections.Generic;
using System.Linq;

namespace Users.Infrastructure
{
    public class CustomUserValidator : UserValidator<AppUser>
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            IdentityResult result = await base.ValidateAsync(manager, user);

            List<IdentityError> errors = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList();

            return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }
    }
}
