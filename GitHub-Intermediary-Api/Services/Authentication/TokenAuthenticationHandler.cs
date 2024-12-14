using GitHub_Intermediary_Api.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;


namespace GitHub_Intermediary_Api.Services.Authentication {
    public class TokenAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions> {
        private readonly IAuthService _AuthService;

        public TokenAuthenticationHandler(IAuthService authService, IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder urlEncoder) : base(options, logger, urlEncoder) {
            _AuthService = authService;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync() {
            if (Context.Request.Path.StartsWithSegments("/Auth")) {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            
            if (!Context.Request.Path.StartsWithSegments("/GitHub")) { 
                return Task.FromResult(AuthenticateResult.NoResult()); 
            }

            if (!Request.Headers.TryGetValue("Authorization", out var tokenHeader) || tokenHeader.Count == 0) {
                return Task.FromResult(AuthenticateResult.Fail("Authorization token is missing."));
            }

            string token = tokenHeader[0]["Bearer ".Length..].Trim();
            if (!_AuthService.ValidateToken(token)) {
                return Task.FromResult(AuthenticateResult.Fail("Invalid or expired token."));
            }

            List<Claim> claims = [
                new Claim(ClaimTypes.Name, "User")
            ];
            ClaimsIdentity claimsIdentity = new(claims, "Bearer");
            ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, "Bearer")));
        }
    }
}
