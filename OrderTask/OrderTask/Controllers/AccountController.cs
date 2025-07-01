using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderTask.Models;
using OrderTask.Utilities;
using System.Text.Json;

namespace OrderTask.Controllers
{
    public class AccountController : Controller
    {
        private readonly Context _context;
        private readonly TokenManager _tokenManager;

        public AccountController(Context context, TokenManager tokenManager)
        {
            _context = context;
            _tokenManager = tokenManager;

        }

        #region Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Register model)
        {
            if (!ModelState.IsValid) return View(model);

            if (_context.users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email is already taken.");
                return View(model);
            }

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                ResetToken = string.Empty // Default value

            };

            _context.users.Add(user);
            _context.SaveChanges();

            var token = _tokenManager.GenerateToken(user.UserName, user.Email);
            Response.Cookies.Append("JwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                Expires =  DateTime.Now.Add(TokenManager.expiryDuration)
            });
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Login(Login model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = _context.users
                .FirstOrDefault(u => u.Email == model.Email);

            if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                var token = _tokenManager.GenerateToken(user.UserName, user.Email);
                Response.Cookies.Append("JwtToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.Add(TokenManager.expiryDuration)
                });
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }
        #endregion

        #region SignOut
        public IActionResult SignOut()
        {
            Response.Cookies.Delete("JwtToken");
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
            if (!ModelState.IsValid) return View(model);

            var user = _context.users.FirstOrDefault(u => u.Email == model.Email);
            if (user != null)
            {
                var token = Guid.NewGuid().ToString();
                user.ResetToken = token;
                user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);
                _context.SaveChanges();

                // Combine email and token, then encrypt
                var combined = $"{model.Email}|{token}";
                var encryptedCode = EncryptEmail.Encrypt(combined);

                var resetLink = Url.Action("ResetPassword", "Account", new { code = encryptedCode }, Request.Scheme);
                var email = new EMail
                {
                    Subject = "Reset Password",
                    body = $"Please reset your password by clicking here: <a href='{resetLink}'>Reset Password</a>",
                    Recipient = model.Email
                };
                MailSetting.SendMail(email);

                return RedirectToAction(nameof(CheckYourInbox));
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
        public IActionResult ResetPassword(string code)
        {
            if (string.IsNullOrEmpty(code)) return BadRequest();

            string decrypted;
            try
            {
                decrypted = EncryptEmail.Decrypt(code);
            }
            catch
            {
                return BadRequest("Invalid reset link.");
            }

            var parts = decrypted.Split('|');
            if (parts.Length != 2) return BadRequest("Invalid reset link.");

            TempData["Email"] = parts[0];
            TempData["Token"] = parts[1];
            return View();
        }



        [HttpPost]
        public IActionResult ResetPassword(ResetPassword model)
        {
            model.Token = TempData["Token"]?.ToString() ?? string.Empty;
            model.Email = TempData["Email"]?.ToString() ?? string.Empty;

            if (!ModelState.IsValid) return View();

            var user = _context.users.FirstOrDefault(u => u.Email == model.Email);
            if (user != null && user.ResetToken == model.Token && user.ResetTokenExpiry > DateTime.UtcNow)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
                user.ResetToken = null;
                user.ResetTokenExpiry = null;
                _context.SaveChanges();
                return RedirectToAction(nameof(Login));
            }

            ModelState.AddModelError(string.Empty, "Invalid or expired token");
            return View();
        }
        #endregion
    }
}