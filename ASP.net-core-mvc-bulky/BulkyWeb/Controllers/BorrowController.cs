using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;


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
            foreach (Borrow borrow in borrows)
            {
                if (DateTime.Now.Date >= borrow.TimeReturn.Date)
                {
                    borrow.IsReturned = true;
                }
            }
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
                if (obj.NameBook == category.Name && obj.NumerBorrow <= category.RemainingOfBook)
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

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Borrow FormDb = _db.Borrows.Find(id);
            if (FormDb == null)
            {
                return NotFound();
            }
            return View(FormDb);
        }

        [HttpPost]
        public IActionResult Edit(Borrow obj)
        {
            List<Category> categories = _db.Categories.ToList();
            if (ModelState.IsValid)
            {
                foreach (Category category in categories)
                {
                    if (obj.NumerBorrow <= category.RemainingOfBook)
                    {
                        _db.Borrows.Update(obj);
                        _db.SaveChanges();
                        TempData["success"] = "Updated successfully";
                        return RedirectToAction("Index");
                    }
                    TempData["error"] = "Cannot Edit";
                }
            }
            return View();

        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Borrow? FormDb = _db.Borrows.Find(id);
            if (FormDb == null)
            {
                return NotFound();
            }
            return View(FormDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Borrow? obj = _db.Borrows.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Borrows.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Deleted successfully";
            return RedirectToAction("Index");

        }
    }
}
