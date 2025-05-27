using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderTask.Models;

public class ProductsController : Controller
{
    private readonly Context _context;

    public ProductsController(Context context)
    {
        _context = context;
    }


    public async Task<IActionResult> Index()
    {
        var products = await _context.products.ToListAsync();
        return View(products);
    }


    


   
}