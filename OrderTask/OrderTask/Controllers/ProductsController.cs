using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderTask.Models;
//using OrderTask.Services.IServices;

[Authorize]
public class ProductsController : Controller
{
    private readonly Context _context;

    public ProductsController(Context context)
    {
        _context = context;
    }
    #region Search and Pagination using services

    //// GET: Products with Search and Pagination
    //[AllowAnonymous]
    //public async Task<IActionResult> Index(string searchString, string searchField, int page = 1)
    //{
    //    const int pageSize = 10; // Adjust page size as needed
    //    var products = await _productService.GetProductsAsync(searchString, searchField, page, pageSize);
    //    var totalProducts = await _productService.GetTotalProductsAsync(searchString, searchField);
    //    var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

    //    var searchFields = new List<SelectListItem>
    //        {
    //            new SelectListItem { Value = "", Text = "All Fields", Selected = string.IsNullOrEmpty(searchField) },
    //            new SelectListItem { Value = "Id", Text = "ID", Selected = searchField == "Id" },
    //            new SelectListItem { Value = "Name", Text = "Name", Selected = searchField == "Name" },
    //            new SelectListItem { Value = "Price", Text = "Price", Selected = searchField == "Price" },
    //            new SelectListItem { Value = "Category", Text = "Category", Selected = searchField == "Category" }
    //        };
    //    ViewBag.SearchFields = new SelectList(searchFields, "Value", "Text", searchField);
    //    ViewBag.CurrentPage = page;
    //    ViewBag.TotalPages = totalPages;

    //    return View(products);
    //}
    #endregion

    [AllowAnonymous]
    public async Task<IActionResult> Index(string searchString , int pageNumber )
    {
        var products =  _context.products.AsQueryable();
        if (!string.IsNullOrEmpty(searchString))
        {
            searchString = searchString.ToLower();
            products = products.Where(p => p.Name.ToLower().Contains(searchString) ||
                                           p.Description.ToLower().Contains(searchString));
        }
        if (pageNumber < 1)
        {
            pageNumber = 1; // Ensure page number is at least 1
        }
        int pageSize = 10; // Adjust page size as needed

        return View(await MvcPageList<Product>.CreateAsync(products,pageNumber, pageSize));
    }

    // GET: Products/Create
    //[Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Create(Product product)
    {
        if (ModelState.IsValid)
        {

            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        var errors = ModelState.Values.SelectMany(v => v.Errors);
        foreach (var error in errors)
        {
            Console.WriteLine(error.ErrorMessage);
        }
        return View(product);
    }


    // GET: Products/Edit
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _context.products.FindAsync(id);

        return View(product);
    }

    // POST: Products/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product product)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }
    // GET: products/Delete
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.products.FindAsync(id);
        return View(product);
    }

    //Post: Products/Delete
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, Product product)
    {
        if (ModelState.IsValid)
        {

            _context.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(product);


    }

}