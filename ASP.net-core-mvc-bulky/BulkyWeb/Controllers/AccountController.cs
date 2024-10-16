using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace BulkyWeb.Controllers
{
    public class AccountController : Controller
    {

        private readonly ApplicationDbContext _db;

        public AccountController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Login()
        {
			return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            email = Request.Form["email"];
            password = Request.Form["pswd"];
            List<Account> objList = _db.Accounts.ToList();
            foreach (Account obj in objList)
            {
                if (email == obj.Email && password == obj.Password)
                {
                    if (obj.Position == "Nhân viên")
                    {
                        HttpContext.Session.SetString("isVisible", "none");
                    }
                    else
                    {
                        HttpContext.Session.SetString("isVisible", "block");
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }



        public ActionResult Index()
        {
            List<Account> obj = _db.Accounts.ToList();
            obj = BubbleSort(obj, obj.Count);
            return View(obj);
        }

        private List<Account> BubbleSort(List<Account> obj, int n)
        {
            obj = _db.Accounts.ToList();
            List<Account> temp = _db.Accounts.ToList();
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (char.ToLower(obj[j].Email[0]) > char.ToLower(obj[j + 1].Email[0]))
                    {
                        temp[0] = obj[j];
                        obj[j] = obj[j + 1];
                        obj[j + 1] = temp[0];
                    }
                }
            }
            return obj;
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Account obj)
        {
            if (ValidatePassword(obj)) 
            {
                _db.Accounts.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Account created successfully";
                return RedirectToAction("Index"); //return RedirectToAction("Index", "Category");
            }
            else
            {
                TempData["Error"] = "Mật khẩu có độ dài tối đa 20 ký tự, có ít nhất 1 chữ viết hoa, 1 số, 1 ký tự!!!!";
            }

            return View();
        }

        public bool ValidatePassword(Account obj)
        {
            
            if (obj.Password.Length > 20)
            {
                Console.WriteLine("Mật khẩu vượt quá độ dài cho phép.");
                return false;
            }

            // Kiểm tra có ít nhất 1 chữ cái viết hoa
            if (!Regex.IsMatch(obj.Password, @"[A-Z]"))
            {
                Console.WriteLine("Mật khẩu phải có ít nhất 1 chữ cái viết hoa.");
                return false;
            }

            // Kiểm tra có ít nhất 1 chữ số
            if (!Regex.IsMatch(obj.Password, @"[0-9]"))
            {
                Console.WriteLine("Mật khẩu phải có ít nhất 1 chữ số.");

                return false;
            }

            // Kiểm tra có ít nhất 1 ký tự đặc biệt
            if (!Regex.IsMatch(obj.Password, @"[\W_]"))
            {
                Console.WriteLine("Mật khẩu phải có ít nhất 1 ký tự đặc biệt.");
                return false;
            }

            Console.WriteLine("Mật khẩu hợp lệ.");
            return true;
        }
    }
}
