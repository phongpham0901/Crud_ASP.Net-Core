using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class BorrowController : Controller
    {

        private readonly ApplicationDbContext _db;

        public BorrowController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Borrow> borrows = _db.Borrows.ToList();
            return View(borrows);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Borrow obj)
        {
            List<Category> categories = _db.Categories.ToList();
            foreach (Category category in categories)
            {
                if (obj.NameBook == category.Name && obj.NumerBorrow <= category.NumerOfBorrow)
                {
                    _db.Borrows.Add(obj);
                    _db.SaveChanges();
                    TempData["success"] = "successfully";
                    return RedirectToAction("Index");
                }
            }

            TempData["error"] = "Fail";
            return View();
        }

    }
}
