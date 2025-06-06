﻿global using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrderTask.Models;
using OrderTask.Utilities;

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
                    if (result.Succeeded)  return RedirectToAction("Index", "Home");

                }


            }
            ModelState.AddModelError(string.Empty, "Decline login : Email or Password is wrong ");
            return View(model);


        }




        #endregion

        #region SignOut

        public new IActionResult SignOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        #endregion

        #region ForgetPassword
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgetPassword(ForgetPassword model)
        {
            if (!ModelState.IsValid) return View(model); // Validate the model state
            var user = _userManager.FindByEmailAsync(model.Email).Result; // Find the user by email
            if (user is not null)
            {
                // Generate a password reset token
                var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
                // Create a reset password link (you can customize this as needed)
                var resetLink = Url.Action("ResetPassword", "Account", new { Token = token, email = model.Email }, Request.Scheme);

                var email = new EMail
                {
                    Subject = "Reset Password",
                    body = $"Please reset your password by clicking here: <a href='{resetLink}'>Reset Password</a>",
                    Recipient = model.Email
                };
                // Send the email  
                MailSetting.SendMail(email);

                return RedirectToAction(nameof(CheckYourInbox)); // Redirect to Login page after sending the email

            }
            ModelState.AddModelError(string.Empty, "Email not found");
            return View(model);
        }

        #endregion

        public IActionResult CheckYourInbox()
        {
          
            return View();
        }

        #region ResetPassword
        

        public IActionResult ResetPassword(string email , string token)
        {
            if(email is null || token is null) return BadRequest();
            TempData["Email"] = email;
            TempData["Token"] = token;

            return View();
        }
        [HttpPost]
        public IActionResult ResetPassword(ResetPassword model)
        {
            model.Token = TempData["Token"]?.ToString()?? string.Empty; //tostrig cuz type is object and the key is string 
            model.Email = TempData["Email"]?.ToString()?? string.Empty;

            if (!ModelState.IsValid) return View();

           var user = _userManager.FindByEmailAsync(model.Email).Result; // Find the user by email
            // Check if the user exists
            if (user is not null)
            {
                // Reset the password using the token
                var result = _userManager.ResetPasswordAsync(user, model.Token, model.Password).Result;

                if (result.Succeeded) return RedirectToAction(nameof(Login)); // Redirect to Login page after successful reset

                
                foreach (var item in result.Errors) ModelState.AddModelError(string.Empty, item.Description);
                
            } ModelState.AddModelError(string.Empty, "Email not found");
            

            return View();
        }
       
        
        #endregion
    }
}
