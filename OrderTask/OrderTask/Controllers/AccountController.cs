global using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrderTask.Models;

namespace OrderTask.Controllers
{
    public class AccountController : Controller
    {
        //private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //public AccountController(Context context)
        //{
        //    _context = context;
        //}
        #region Old

        //// GET: /Account/Login
        //public IActionResult Login()
        //{
        //    ViewBag.Username = HttpContext.Session.GetString("Username");
        //    return View();
        //}

        //// POST: /Account/Login
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Login(Login model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = _context.User
        //            .FirstOrDefault(u => u.UserName == model.Username && u.Password == model.Password);

        //        if (user != null)
        //        {
        //            HttpContext.Session.SetString("UserId", user.Id.ToString());
        //            HttpContext.Session.SetString("Username", user.UserName);
        //            return RedirectToAction("Index", "Orders");
        //        }
        //        ModelState.AddModelError("", "Invalid username or password.");
        //    }
        //    ViewBag.Username = HttpContext.Session.GetString("Username");
        //    return View(model);
        //}

        //// GET: /Account/SignUp
        //public IActionResult SignUp()
        //{
        //    ViewBag.Username = HttpContext.Session.GetString("Username");
        //    return View();
        //}

        //// POST: /Account/SignUp
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult SignUp(SignUp model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Check if username already exists
        //        if (_context.User.Any(u => u.UserName == model.Username))
        //        {
        //            ModelState.AddModelError("Username", "Username is already taken.");
        //            return View(model);
        //        }

        //        // Check if passwords match
        //        if (model.Password != model.ConfirmPassword)
        //        {
        //            ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
        //            return View(model);
        //        }

        //        // Create new user
        //        var user = new User
        //        {
        //            UserName = model.Username,
        //            Password = model.Password
        //        };

        //        _context.User.Add(user);
        //        _context.SaveChanges();

        //        // Log in the new user automatically
        //        HttpContext.Session.SetString("UserId", user.Id.ToString());
        //        HttpContext.Session.SetString("Username", user.UserName);
        //        return RedirectToAction("Index", "Orders");
        //    }
        //    ViewBag.Username = HttpContext.Session.GetString("Username");
        //    return View(model);
        //}

        //// GET: /Account/Logout
        //public IActionResult Logout()
        //{
        //    HttpContext.Session.Clear();
        //    return RedirectToAction("Login");
        //}
        //// GET: /Account/Login
        //public IActionResult Login()
        //{
        //    ViewBag.Username = HttpContext.Session.GetString("Username");
        //    return View();
        //}

        //// POST: /Account/Login
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Login(Login model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = _context.User
        //            .FirstOrDefault(u => u.UserName == model.Username && u.Password == model.Password);

        //        if (user != null)
        //        {
        //            HttpContext.Session.SetString("UserId", user.Id.ToString());
        //            HttpContext.Session.SetString("Username", user.UserName);
        //            return RedirectToAction("Index", "Orders");
        //        }
        //        ModelState.AddModelError("", "Invalid username or password.");
        //    }
        //    ViewBag.Username = HttpContext.Session.GetString("Username");
        //    return View(model);
        //}

        //// GET: /Account/SignUp
        //public IActionResult SignUp()
        //{
        //    ViewBag.Username = HttpContext.Session.GetString("Username");
        //    return View();
        //}

        //// POST: /Account/SignUp
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult SignUp(SignUp model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Check if username already exists
        //        if (_context.User.Any(u => u.UserName == model.Username))
        //        {
        //            ModelState.AddModelError("Username", "Username is already taken.");
        //            return View(model);
        //        }

        //        // Check if passwords match
        //        if (model.Password != model.ConfirmPassword)
        //        {
        //            ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
        //            return View(model);
        //        }

        //        // Create new user
        //        var user = new User
        //        {
        //            UserName = model.Username,
        //            Password = model.Password
        //        };

        //        _context.User.Add(user);
        //        _context.SaveChanges();

        //        // Log in the new user automatically
        //        HttpContext.Session.SetString("UserId", user.Id.ToString());
        //        HttpContext.Session.SetString("Username", user.UserName);
        //        return RedirectToAction("Index", "Orders");
        //    }
        //    ViewBag.Username = HttpContext.Session.GetString("Username");
        //    return View(model);
        //}

        //// GET: /Account/Logout
        //public IActionResult Logout()
        //{
        //    HttpContext.Session.Clear();
        //    return RedirectToAction("Login");
        //}

        #endregion

        #region Register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(Register model)
        {
            if (!ModelState.IsValid) return View(model); // Validate the model state

            var user = new User // Create a new User object
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
            };
            var res =_userManager.CreateAsync(user, model.Password).Result;
            if (res.Succeeded) 
                return RedirectToAction(nameof(Login));

            foreach (var item in res.Errors)
            {
                ModelState.AddModelError(string.Empty, item.Description);
                
            }
            return View();
        }

        #endregion

        #region Login


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Login model)
        {
            if (!ModelState.IsValid) return View(model); // server side Validate 
            var user = _userManager.FindByEmailAsync(model.Email).Result; // Find the user by email
            if (user is not null)
            {
                //_userManager.CheckPasswordAsync(user, model.Password).Result;
                if (_userManager.CheckPasswordAsync(user, model.Password).Result) // Check if the password is correct
                {
                    var result = _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false).Result; // Sign in the user
                    //return RedirectToAction("Index", "Orders"); // Redirect to Orders index page
                    if (result.Succeeded)  return RedirectToAction("Index", "Orders");

                }


            }
            ModelState.AddModelError(string.Empty, "Decline login : Email or Password is wrong ");
            return View(model);


        }




        #endregion



    }
}
