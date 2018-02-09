using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Project.Models;

namespace Project
{
	public class PhoneNumberRequirement : IAuthorizationRequirement
	{
		public Regex PhoneMask { get; }
		public PhoneNumberRequirement(string regex)
		{
			if (string.IsNullOrEmpty(regex))
			{
				PhoneMask = new Regex(regex);
			}
		}
	}
}