using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;

namespace lab_3_server
{
    // Класс для обработки аутентификации по базовой аутентификации
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

        // Метод для обработки аутентификации
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Получаем заголовок аутентификации
            var authHeader = Request.Headers["Authorization"].ToString();

            if (authHeader != null && authHeader.StartsWith("Basic", StringComparison.OrdinalIgnoreCase))
            {
                // Извлекаем токен из заголовка
                var token = authHeader.Substring("Basic ".Length).Trim();
                var credentialstring = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                var credentials = credentialstring.Split(':');
                var login = credentials[0];
                var password = credentials[1];

                // Получаем права пользователя из репозитория
                var result = _repository.GetUserRights(login, password);

                if (result.Count > 0)
                {
                    var claims = new List<Claim>
                    {
                        // Создаем клейм с именем пользователя
                        new Claim(ClaimsIdentity.DefaultNameClaimType, login)
                    };

                    // Добавляем клеймы с ролями (правами) пользователя
                    foreach (var right in result)
                    {
                        claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, right));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, "Basic");
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
                }

                // Пользователь не аутентифицирован
                Response.StatusCode = 401;
                return Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
            }
            else
            {
                // Заголовок аутентификации не соответствует формату Basic
                Response.StatusCode = 401;
                return Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
            }
        }
    }
}
