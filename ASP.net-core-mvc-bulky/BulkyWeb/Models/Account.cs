using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models
{
	public class Account
	{

		[Key]
		public int Id { get; set; }

		[Required]
		public string Email { get; set; }

		[Required]
		[MaxLength(20, ErrorMessage ="Mật khẩu có độ dài tối đa 20 ký tự, có ít nhất 1 chữ viết hoa, 1 số, 1 ký tự!!!!")]
		public string Password { get; set; }

	}
}
