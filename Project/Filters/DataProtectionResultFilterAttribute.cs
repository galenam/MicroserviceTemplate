using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Project.Attributes;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Collections.Generic;
using Common.Const;

namespace Project.Filters {
	public class DataProtectionResultFilterAttribute : ResultFilterAttribute {
		private static IDataProtector _dataProtector;
		public DataProtectionResultFilterAttribute () {
			
		}
		public override void OnResultExecuted (ResultExecutedContext context) {
		}

		public override void OnResultExecuting (ResultExecutingContext context) {
			var dp = context.HttpContext.RequestServices.GetDataProtectionProvider();
			_dataProtector = dp.CreateProtector(DataProtectorsNames.RootDataProtectorName);
			
			var result = (context.Result as ObjectResult)?.Value;
			var customType = result.GetType();
			foreach (var property in customType.GetProperties())
			{
				var customAttributes = property.GetCustomAttributes(false);
				foreach (var attr in customAttributes)
				{
					if (typeof(ProtectAttribute) == attr.GetType())
					{
						var obj = property.GetValue(result);
						var bytes =(string)(obj);
						if (bytes != null)
						{
							var protectedBytes = _dataProtector.Protect(bytes);
							property.SetValue(result, protectedBytes);
						}
					}
				}
			}
		}
	}
}