﻿using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models
{
	public class Account
	{

		[Key]
		public int Id { get; set; }

		[Required]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }

	}
}
