using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Project.Auth
{
	public class PhoneNumberAuthenticationHandler : AuthenticationHandler<PhoneNumberAuthenticationOptions>
	{
		public PhoneNumberAuthenticationHandler(IOptionsMonitor<PhoneNumberAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
			: base(options, logger, encoder, clock)
		{
		}

		protected override Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			var headers = Request.Headers;

			var headerName = "X-PhoneNumber";
			var phones = headers[headerName];
			var phone = phones.ToArray()?.FirstOrDefault();

			if (!string.IsNullOrEmpty(phone) && Options.PhoneMask.IsMatch(phone))
			{
				var claims = new[] { new Claim("phone", phone) };
				var identity = new ClaimsIdentity(claims, "PhoneNumberAuthenticationHandler");
				var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), this.Scheme.Name);

				return Task.FromResult(AuthenticateResult.Success(ticket));
			}

			return Task.FromResult(AuthenticateResult.Fail("Unuathorized with phone"));
		}
	}
}