using Microsoft.AspNetCore.Mvc;
using OrderTask.Models;

namespace OrderTask.Controllers
{
    

    public class AccountController : Controller
    {
        private readonly Context _context;

        public AccountController(Context context)
        {
            _context = context;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.users
                    .FirstOrDefault(u => u.UserName == model.Username && u.Password == model.Password);

                if (user != null)
                {
                    
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    HttpContext.Session.SetString("Username", user.UserName);
                    return RedirectToAction("Index", "Orders");
                }
                ModelState.AddModelError("", "Invalid username or password.");
            }
            return View(model);
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
