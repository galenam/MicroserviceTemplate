using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Project.Models;

namespace Project.Auth
{
	public class PhoneNumberAuthenticationOptions : AuthenticationSchemeOptions
	{
		public ClaimsIdentity Identity { get; set; }
		public Regex PhoneMask { get; } = new Regex("7\\d{10}");

	}
}