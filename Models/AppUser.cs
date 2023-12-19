using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Task4Attempt2.Models
{
    public class AppUser : IdentityUser
    {
		public string FirstLogin { get; set; }
		public string LastLogin { get; set; }
		public string Status { get; set; }
	}
}
