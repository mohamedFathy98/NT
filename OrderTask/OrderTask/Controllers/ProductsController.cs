using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderTask.Models;

[Authorize]
public class ProductsController : Controller
{
    private readonly Context _context;

    public ProductsController(Context context)
    {
        _context = context;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var products = await _context.products.ToListAsync();
        return View(products);
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
            product.CreatedAt = DateTime.Now;
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