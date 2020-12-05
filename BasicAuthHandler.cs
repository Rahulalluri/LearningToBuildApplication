using FoodBlog.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace FoodBlog
{
    public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        readonly IUserService _userService;

        public BasicAuthHandler(IUserService userService, IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory loggerFactory,
            UrlEncoder urlEncoder, ISystemClock systemClock) : base(options, loggerFactory, urlEncoder, systemClock)
        {
            _userService = userService;
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers["WWW-Authenticate"] = "Basic";
            return base.HandleChallengeAsync(properties);
        }
        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string userName = null;
            string password = null;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');
                userName = credentials.FirstOrDefault();
                password = credentials.LastOrDefault();

                if (!_userService.CheckUser(userName,password))
                {
                    throw new ArgumentException("Invalid credentials {0}",
                        string.Format(CultureInfo.InvariantCulture, string.Join(',', nameof(userName), nameof(password))));
                }

            }
            catch (Exception ex)
            {

                return AuthenticateResult.Fail(ex.Message);
            }

            var claims = new[] {
                new Claim(ClaimTypes.Name, userName)
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principle = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principle, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
