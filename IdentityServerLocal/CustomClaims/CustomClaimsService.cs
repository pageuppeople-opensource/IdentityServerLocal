using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.Extensions.Logging;

namespace IdentityServerLocal.CustomClaims
{
    public class CustomClaimsService : DefaultClaimsService
    {
        public CustomClaimsService(IProfileService profile, ILogger<DefaultClaimsService> logger) : base(profile, logger) { }

        public override async Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(ClaimsPrincipal subject, Client client, Resources resources, ValidatedRequest request)
        {
            var baseClaims = (await base.GetAccessTokenClaimsAsync(subject, client, resources, request)).ToList();
            var requestInstanceIdRaw = request.Raw[CustomClaimTypes.InstanceId];

            baseClaims.Add(new Claim(CustomClaimTypes.InstanceId, requestInstanceIdRaw));

            return baseClaims;
        }
    }
}
