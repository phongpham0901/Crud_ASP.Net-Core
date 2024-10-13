using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class AccountController : Controller
    {

        private readonly ApplicationDbContext _db;

        public AccountController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
			return View();
        }

		//[HttpPost] // Đảm bảo phương thức này chỉ xử lý yêu cầu POST
		//public IActionResult Login(string email, string pswd) // Nhận email và mật khẩu từ form
		//{
		//	var account = _db.Accounts.FirstOrDefault(a => a.Email == email && a.Password == pswd);

		//	if (account != null)
		//	{
		//		// Đăng nhập thành công, chuyển hướng tới Category/Index
		//		return RedirectToAction("Index", "Category");
		//	}
		//	else
		//	{
		//		// Đăng nhập thất bại, hiển thị thông báo lỗi
		//		ViewBag.ErrorMessage = "Email hoặc mật khẩu không chính xác.";
		//		return View("Index"); // Trả lại view đăng nhập với thông báo lỗi
		//	}
		//}

		[HttpPost]
        public IActionResult Login()
        {
			string email = Request.Form["email"];
			string password = Request.Form["pswd"];
			List<Account> objCategoryList = _db.Accounts.ToList();
			foreach (Account objCategory in objCategoryList)
			{
				if (email == objCategory.Email && password == objCategory.Password)
				{
					return RedirectToAction("Index", "Home");
				}
			}
            return View();
		}
    }
}
