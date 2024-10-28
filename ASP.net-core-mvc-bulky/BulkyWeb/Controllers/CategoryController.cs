using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

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
            
            foreach (Category category in objCategoryList)
            {
                int totalNumerBorrow = 0;
                
                foreach (Borrow borrow in borrows)
                {
                    if (borrow.NameBook == category.Name)
                    {
                        totalNumerBorrow += borrow.NumerBorrow;
                    }
                }
       
                if (totalNumerBorrow > 0)
                {
                    category.NumerOfBorrow = totalNumerBorrow;
                }
                else
                {
                    category.NumerOfBorrow = category.DisplayOrder;
                }
                category.RemainingOfBook = category.DisplayOrder - category.NumerOfBorrow;
                _db.Categories.Update(category);
            }

            _db.SaveChanges();

            objCategoryList = BubbleSort(objCategoryList, objCategoryList.Count);

            return View(objCategoryList);
        }



        public IActionResult Search(string search)
        {
            search = Request.Form["search"];
            List<Category> objCategoryList = _db.Categories.ToList();
            List<Category> listSearch = new List<Category>();

            foreach (Category category in objCategoryList)
            {

                if (category.Name == search)
                {
                    listSearch.Add(category);
                }
            }
            return View("Index", listSearch);


        }

        public IActionResult Create()
        {
            HttpContext.Session.SetString("Mode", "Create");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            List<Category> ctr = _db.Categories.ToList();
            foreach(Category category in ctr)
            {
                if(obj.Name == category.Name)
                {
                    ModelState.AddModelError("Name", "The Name cannot exactly match the Name.");
                }
            }

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
                // kiểm tra xem đang ở edit hay create thì chuyển về view đấy
                string mode = HttpContext.Session.GetString("Mode");

                // Redirect to the appropriate view based on mode
   
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

        [HttpPost]
        public async Task<IActionResult> SingleFileUploadForEdit(IFormFile SingleFileEdit)
        {
            if (SingleFileEdit != null && SingleFileEdit.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", SingleFileEdit.FileName);

                // Save file to the uploads directory
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await SingleFileEdit.CopyToAsync(stream);
                }
                string img = $"\\uploads\\{SingleFileEdit.FileName}";

                // Return JSON with the file path
                return Json($"File uploaded successfully: {img}");
            }

            return Json("File upload failed.");
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
