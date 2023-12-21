using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace lab_2_server
{
    /// <summary>
    /// Обработчик базовой аутентификации
    /// </summary>
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserService _repository;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserService repository
        ) : base(options, logger, encoder, clock)
        {
            _repository = repository;
        }

        /// <summary>
        /// Обработка аутентификации
        /// </summary>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (authHeader != null && authHeader.StartsWith("Basic", StringComparison.OrdinalIgnoreCase))
            {
                var token = authHeader.Substring("Basic ".Length).Trim();
                var credentialstring = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                var credentials = credentialstring.Split(':');
                var login = credentials[0];
                var password = credentials[1];
                var result = _repository.GetUserRights(login, password);

                if (result.Count > 0)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, login)
                    };
                    foreach (var right in result)
                    {
                        claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, right));
                    }
                    var claimsIdentity = new ClaimsIdentity(claims, "Basic");
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
                }

                Response.StatusCode = 401;
                return Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
            }
            else
            {
                Response.StatusCode = 401;
                return Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
            }
        }
    }
}
