using Microsoft.AspNetCore.Mvc;
using Project.Attributes;

namespace Project.Models {
	public class ReturnedValue {
		[Protect]
		public string SecureProperty { get; set; }
		public int NonSecureProperty { get; set; }
	}
}