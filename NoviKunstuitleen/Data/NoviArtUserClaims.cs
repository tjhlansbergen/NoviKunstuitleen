/*
    NoviArtUserClaims.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 24 dec 2019
*/

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NoviKunstuitleen.Data
{

    /// <summary>
    /// Override van de ASP Identity user claims voor het toevoegeven van custom claims voor gebruikerstype en weergavenaam
    /// </summary>
    public class NoviArtUserClaims : UserClaimsPrincipalFactory<NoviArtUser>
    {
        // superklasse aanroepen in constructor voor parameters
        public NoviArtUserClaims(UserManager<NoviArtUser> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
        }


        /// <summary>
        /// Override GenerateClaims voor het toeveogen van customs claims
        /// </summary>
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(NoviArtUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("Type", user.Type.ToString()));
            identity.AddClaim(new Claim("DisplayName", user.DisplayName));
            return identity;
        }
    }
}
