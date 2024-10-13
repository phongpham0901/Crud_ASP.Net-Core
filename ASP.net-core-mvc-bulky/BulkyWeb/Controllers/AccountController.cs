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

        public IActionResult Login()
        {
			return View();
        }

		[HttpPost]
        public IActionResult Login(string email, string password)
        {
			email = Request.Form["email"];
			password = Request.Form["pswd"];
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
            if (ModelState.IsValid) 
            {
                _db.Accounts.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Account created successfully";
                return RedirectToAction("Index"); //return RedirectToAction("Index", "Category");
            }

            return View();
        }
    }
}
