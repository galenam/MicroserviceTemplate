using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Const;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project.Models;

namespace Project.Controllers
{
	[Route("api/[controller]")]
	public class ValuesController : Controller
	{
		private static ILogger<ValuesController> _logger;
		private static IDataProtector _dataProtector;
		public ValuesController(ILogger<ValuesController> logger, IDataProtectionProvider dataProtectionProvider)
		{
			_logger = logger;
			_dataProtector = dataProtectionProvider.CreateProtector(Common.Const.DataProtectorsNames.RootDataProtectorName);
		}

		// GET api/values
		[HttpGet]
		public IEnumerable<ReturnedValue> Get()
		{
			_logger.LogInformation("call method get");
			return new[] { new ReturnedValue { SecureProperty = _dataProtector.Protect("test"), NonSecureProperty = 5 } };
		}

		// GET api/values/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			_logger.LogInformation($"call method get with id {id}");
			return "value";
		}

		// POST api/values
		[HttpPost]
		public void Post([FromBody]string value)
		{
			_logger.LogInformation($"call method Post with value={value}");
		}

		// PUT api/values/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody]string value)
		{
			_logger.LogInformation($"call method Put with value={value}, id={id}");
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
			_logger.LogInformation($"call method Delete with id={id}");
		}
	}
}
