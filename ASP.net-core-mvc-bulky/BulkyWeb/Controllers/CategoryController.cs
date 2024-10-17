using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            
            List<Category> objCategoryList = _db.Categories.ToList();
            List<Borrow> borrows = _db.Borrows.ToList();
            foreach(Category category in objCategoryList)
            {
                foreach (Borrow borrow in borrows)
                {
                    if(category.Name == borrow.NameBook)
                    {
                        category.NumerOfBorrow = category.NumerOfBorrow - borrow.NumerBorrow;
                        _db.Categories.Update(category);
                        _db.SaveChanges();
                    }
                    else 
                    {
                        category.NumerOfBorrow = category.DisplayOrder;
                    }
                }
            }
            objCategoryList = BubbleSort(objCategoryList, objCategoryList.Count);
         
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {

            

            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The DisplayOrder cannot exactly match the Name.");
            }

            if (ModelState.IsValid)
            {
                // Nếu không có giá trị trong ImageUrl, có thể xử lý nó ở đây
                if (string.IsNullOrEmpty(obj.ImageUrl))
                {
                    ModelState.AddModelError("ImageUrl", "Image URL cannot be null.");
                }
                else
                {
                    _db.Categories.Add(obj);
                    _db.SaveChanges();
                    TempData["success"] = "Category created successfully";
                    // Xóa giá trị trong Session sau khi đã sử dụng
                    HttpContext.Session.Remove("UploadedFileName");
                    return RedirectToAction(nameof(Index));
                }
            }

            // Trả về model để giữ lại dữ liệu đã nhập
            return View(obj);
        }


        [HttpPost]
        public async Task<IActionResult> SingleFileUpload(IFormFile SingleFile)
        {
            if (SingleFile != null && SingleFile.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", SingleFile.FileName);

                // Save file to the uploads directory
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await SingleFile.CopyToAsync(stream);
                }
                string img = $"\\uploads\\{SingleFile.FileName}";
                // Store the file name in Session to access it later in the Create method
                HttpContext.Session.SetString("UploadedFileName", img);

                // Redirect back to the Create action
                return RedirectToAction("Create", "Category");
            }

            HttpContext.Session.SetString("error", "File upload failed."); // Store error message in Session
            return RedirectToAction("Index"); // Redirect if file upload fails
        }




        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            Category categoryFormDb = _db.Categories.Find(id);
            if (categoryFormDb == null) 
            {
                return NotFound();
            }
            return View(categoryFormDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index"); //return RedirectToAction("Index", "Category");
            }
            return View();

        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? categoryFormDb = _db.Categories.Find(id);
            if (categoryFormDb == null)
            {
                return NotFound();
            }
            return View(categoryFormDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _db.Categories.Find(id);
            if (obj == null) 
            {
                return NotFound();
            }
            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index"); 

        }


        //bubble sort
        private List<Category> BubbleSort(List<Category> obj, int n)
        {
            obj = _db.Categories.ToList();
            List<Category> temp = _db.Categories.ToList();
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (obj[j].DisplayOrder < obj[j + 1].DisplayOrder)
                    {
                        temp[0] = obj[j];
                        obj[j] = obj[j + 1];
                        obj[j + 1] = temp[0];
                    }
                }
            }
            return obj;
        }

    }
}
