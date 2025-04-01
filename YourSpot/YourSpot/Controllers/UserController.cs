
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using YourSpot.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.AspNetCore.Http;


namespace YourSpot.Controllers
{
    public class UserController : Controller
    {
        private readonly MyDbContext _context;

        public UserController(MyDbContext context)
        {
            _context = context;
        }
        public IActionResult Log_Reg()
        {
            return View();
        }


        [HttpPost]
        public IActionResult HandelReg(User user)
        {

            if (ModelState.IsValid)
            {
                user.UserType = "User";  
                user.IsActive = true;
                _context.Users.Add(user);
                _context.SaveChanges();
            }


            return View("Log_Reg");
        }


        [HttpPost]
        public IActionResult HandelLogin(string email, string password, string role)
        {
 
            if (email == "Admin@gmail.com")
            {
                var admine = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password && u.IsActive == true && u.UserType == "Admin");
                if(admine != null)
                {
                    HttpContext.Session.SetInt32("id", admine.Id);
                    HttpContext.Session.SetString("Role", "SuperAdmin");
                    return RedirectToAction("SuperAdmin", "Home");
                }
                
            }


            if (role == "user")
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password && u.IsActive == true && u.UserType == "User");
                if (user != null)
                {
                    HttpContext.Session.SetInt32("id", user.Id);
                    HttpContext.Session.SetString("Role", "User");
                    return RedirectToAction("Index", "Home");
                }
            }
            else if (role == "admin")
            {
                var venueOwner = _context.Venues.FirstOrDefault(v => v.Email == email && v.Password == password && v.Status == "Accepted" && v.IsActive == true);
                if (venueOwner != null)
                {
                    HttpContext.Session.SetInt32("id", venueOwner.Id);
                    HttpContext.Session.SetString("Role", "Venue");
                    return RedirectToAction("AdminVenues", "Home");
                }
                var photographer = _context.Photographers.FirstOrDefault(p => p.Email == email && p.Password == password && p.Status == "Accepted" && p.IsActive == true);
                if (photographer != null)
                {
                    HttpContext.Session.SetInt32("id", photographer.Id);
                    HttpContext.Session.SetString("Role", "Photogeapher");
                    return RedirectToAction("AdminPhotographers", "Home");
                }
                var dressRental = _context.Dresses.FirstOrDefault(d => d.Email == email && d.Password == password && d.Status == "Accepted" && d.IsActive == true);
                if (dressRental != null)
                {
                    HttpContext.Session.SetInt32("id", dressRental.Id);
                    HttpContext.Session.SetString("Role", "Dress");
                    return RedirectToAction("AdminDresses", "Home");
                }
            }

       
            ViewData["Message"] = "Invalid email or password!";
            return View("Log_Reg");
        }


        public IActionResult Forgetpassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult HandelReset(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {

                ViewData["Message_1"] = "Email not found!";
                return View(nameof(Forgetpassword));


            }


            Random rand = new Random();

            int verificationCode = rand.Next(100000, 999999);


            HttpContext.Session.SetString("ResetCode", verificationCode.ToString());
            HttpContext.Session.SetString("ResetEmail", email);


            _ = SendEmailAsync(email, "Password Reset Code", $"Your reset code is: {verificationCode}");
            ViewData["Message_1"] = "A verification code has been sent to your email!";
            Console.WriteLine(email);

            return View(nameof(VerifyCode));

        }

        static async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Bilal", "belaltahercr7@gmail.com"));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = subject;

                message.Body = new TextPart("plain") { Text = body };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect, cancellationToken: new CancellationTokenSource(5000).Token);
                    await client.AuthenticateAsync("belaltahercr7@gmail.com", "slhw nhec faxg ixkz");
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        public IActionResult VerifyCode()
        {
            return View();
        }



        [HttpPost]
        public IActionResult HandelCode(string code)
        {
            var MyCode = HttpContext.Session.GetString("ResetCode");


            if (MyCode == null || MyCode != code)
            {
                ViewData["Message_2"] = "Invalid verification code!";
                return View("VerifyCode");
            }


            return View(nameof(NewPassword));
        }


        public IActionResult NewPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult HandelNewPassword(string newPassword, string confirmPassword)
        {
        
            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                ViewData["MSG"] = "New Password/Confirm Password Invalid!";
                return View(nameof(NewPassword));
            }

       
            if (newPassword != confirmPassword)
            {
                ViewData["MSG"] = "The new password does not match the new password confirmation.";
                return View(nameof(NewPassword));
            }

         
            var email = HttpContext.Session.GetString("ResetEmail");
            if (string.IsNullOrEmpty(email))
            {
                ViewData["MSG"] = "Session expired. Please try again.";
                return View(nameof(NewPassword));
            }

     
            var user = _context.Users.FirstOrDefault(e => e.Email == email);
            if (user == null)
            {
                ViewData["MSG"] = "The password has not been changed. There are some problems. Please try again later.";
                return View(nameof(NewPassword));
            }

      
            user.Password = newPassword;


            _context.SaveChanges();

   
            ViewData["MSG"] = "The password has been changed successfully.";
            return View(nameof(NewPassword));
        }

        public IActionResult Profile()
        {

            var Id = HttpContext.Session.GetInt32("id");

            if (Id == null)
            {
                return RedirectToAction("Login", "Account");
            }

    
            var userFavorites = _context.Favorites
                .Where(f => f.UserId == Id)
                .ToList();

           
            var venueIds = userFavorites
                .Where(f => f.ItemType == "Venues")
                .Select(f => f.ItemId)
                .ToList();

            var favoriteVenues = _context.Venues
                .Where(v => venueIds.Contains(v.Id))
                .ToList();

  
            var photoIds = userFavorites
                .Where(f => f.ItemType == "Photography")
                .Select(f => f.ItemId)
                .ToList();

            var favoritePhotography = _context.Photographers
                .Where(p => photoIds.Contains(p.Id))
                .ToList();

       
            var dressIds = userFavorites
                .Where(f => f.ItemType == "Dresses")
                .Select(f => f.ItemId)
                .ToList();

            var favoriteDresses = _context.Dresses
                .Where(d => dressIds.Contains(d.Id))
                .ToList();

            var AllInfo = new UsersViewModel
            {
                InfoUser = _context.Users.FirstOrDefault(u => u.Id == Id),
                booking = _context.Bookings.Where(d => d.UserId == Id).ToList(),
                FavoriteVenues = favoriteVenues,
                FavoritePhotography = favoritePhotography,
                FavoriteDresses = favoriteDresses
            };

            return View(AllInfo);
        }


        [HttpPost]
        public IActionResult EditProfile(string name, string phone)
        {
            int? Id = HttpContext.Session.GetInt32("id");
            if (Id == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == Id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Name = name;
            user.Phone = phone;

            _context.Users.Update(user);
            _context.SaveChanges();

            return RedirectToAction("Profile"); 
        }



        [HttpPost]
        public IActionResult EditPassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return Content("Please ensure all fields are valid.");
            }

            var user = _context.Users.Find(model.Id);
            if (user == null || user.Password != model.CurrentPassword)
            {
                return Content("Current password is incorrect.");
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                return Content("New password and confirm password do not match.");
            }

            user.Password = model.NewPassword;
            _context.SaveChanges();

            return Content("Password successfully changed!");
        }




        public IActionResult Establishment_Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult HandelEstablishment(Venue AddNewVenue)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage_0"] = "There are some problems, please try again!";
                return RedirectToAction("Establishment_Registration");
            }
            AddNewVenue.Status = "Pending";
            AddNewVenue.IsActive = true;
            _context.Add(AddNewVenue);
            _context.SaveChanges();
            TempData["SuccessMessage_0"] = "Thank you for your request. Your request has been sent successfully!";
            return RedirectToAction("Establishment_Registration");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");

        }










        public IActionResult DeleteVenuesFav(int id)
        {
            
            var VenueFav = _context.Favorites.FirstOrDefault(f => f.ItemId == id && f.ItemType == "Venues");

            if (VenueFav != null)
            {
                _context.Favorites.Remove(VenueFav);
                _context.SaveChanges();
            }


            return RedirectToAction("Profile","User");

        }

        public IActionResult DeletePhotoFav(int id)
        {

            var VenueFav = _context.Favorites.FirstOrDefault(f => f.ItemId == id && f.ItemType == "Photography");

            if (VenueFav != null)
            {
                _context.Favorites.Remove(VenueFav);
                _context.SaveChanges();
            }


            return RedirectToAction("Profile", "User");

        }


        public IActionResult DeleteDressesFav(int id)
        {

            var VenueFav = _context.Favorites.FirstOrDefault(f => f.ItemId == id && f.ItemType == "Dresses");

            if (VenueFav != null)
            {
                _context.Favorites.Remove(VenueFav);
                _context.SaveChanges();
            }


            return RedirectToAction("Profile", "User");

        }


        public IActionResult DeleteBooking(int id)
        {

            var delBooking = _context.Bookings.FirstOrDefault(f => f.Id == id);

            if (delBooking != null)
            {
                delBooking.Status = "Cancelled";
                _context.SaveChanges();
            }


            return RedirectToAction("Profile", "User");

        }


        public IActionResult LogoutAdmin()
        {

            HttpContext.Session.Clear();

            return RedirectToAction(nameof(Log_Reg));

        }

        public class ChangePasswordModel
        {
            public int Id { get; set; }
            public string CurrentPassword { get; set; }
            public string NewPassword { get; set; }
            public string ConfirmPassword { get; set; }
        }
    }
}
