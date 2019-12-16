using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NoviKunstuitleen.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NoviKunstuitleen.Data
{
    public class NoviArtUserClaims : UserClaimsPrincipalFactory<NoviArtUser>
    {
        public NoviArtUserClaims(
        UserManager<NoviArtUser> userManager,
        IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(NoviArtUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("Type", user.Type.ToString()));
            identity.AddClaim(new Claim("DisplayName", user.DisplayName));
            return identity;
        }
    }
}
