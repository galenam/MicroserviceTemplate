using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Project.Models;

namespace Project.Auth
{
	public class PhoneNumberAuthorizationHandler : AuthorizationHandler<PhoneNumberRequirement>
	{
		public PhoneNumberAuthorizationHandler()
		{
		}

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PhoneNumberRequirement requirement)
		{
			var filterContext = context.Resource as AuthorizationFilterContext;
			var response = filterContext.HttpContext.Response;
			try
			{
				if (filterContext == null)
				{
					return Task.CompletedTask;
				}

				var headers = filterContext.HttpContext.Request.Headers;
				if (headers == null)
				{
					return Task.CompletedTask;
				}

				var headerName = "X-PhoneNumber";
				var phones = headers[headerName];
				var phone = phones.ToArray()?.FirstOrDefault();

				if (string.IsNullOrEmpty(phone) || !requirement.PhoneMask.IsMatch(phone))
				{
					return Task.CompletedTask;
				}
				context.Succeed(requirement);
				return Task.CompletedTask;
			}
			catch (Exception)
			{
				return Task.CompletedTask;
			}
		}
	}
}