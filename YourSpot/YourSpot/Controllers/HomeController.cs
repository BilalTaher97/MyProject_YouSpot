using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using YourSpot.Models;
using static YourSpot.Controllers.UserController;

namespace YourSpot.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(MyDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var specialVenues = _context.FeaturedItems
                .Where(f => f.ItemType == "Special")
                .Select(f => f.ItemId)
                .ToList();

            var venues = _context.Venues
                .Where(v => specialVenues.Contains(v.Id))
                .Take(3)
                .Select(v => new HomeViewModel
                {
                    Venue = v,
                    ImageUrl = _context.Images
                        .Where(img => img.ServiceType == "Venue" && img.ServiceId == v.Id)
                        .Select(img => img.ImageUrl)
                        .FirstOrDefault() ?? "/uploads/default.jpg" 
                })
                .ToList();

            return View(venues);
        }







        [HttpPost]
        public IActionResult HandelFeedback(string first_name, string last_name, string email, string message)
        {
            if (string.IsNullOrEmpty(first_name) || string.IsNullOrEmpty(last_name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(message))
            {
                return Json(new { success = false, message = "You must fill in all fields!" });
            }

            Feedback feedback = new Feedback
            {
                FirstName = first_name,
                LastName = last_name,
                Email = email,
                Description = message
            };

            _context.Add(feedback);
            _context.SaveChanges();

            return Json(new { success = true, message = "Your feedback has been sent successfully!" });
        }

        public IActionResult About()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult AdminVenues()
        {
            int VId = Convert.ToInt32(HttpContext.Session.GetInt32("id"));
            var Myvenues = _context.Venues.FirstOrDefault(v => v.Id == VId);

            if (Myvenues == null)
            {
                return View();
            }

            return View(Myvenues);
        }

        [HttpPost]
        public IActionResult HandelAdminVenues(Venue EditVenus)
        {
            int VId = Convert.ToInt32(HttpContext.Session.GetInt32("id"));
            var existingVenue = _context.Venues.Find(VId);

            if (existingVenue == null)
            {
                return NotFound();
            }


            existingVenue.Name = EditVenus.Name;
            existingVenue.Email = EditVenus.Email;
            existingVenue.Phone = EditVenus.Phone;
            existingVenue.Address = EditVenus.Address;
            existingVenue.EstablishmentName = EditVenus.EstablishmentName;
            existingVenue.EstablishmentType = EditVenus.EstablishmentType;
            existingVenue.Price = EditVenus.Price;
            existingVenue.Capacity = EditVenus.Capacity;
            existingVenue.Description = EditVenus.Description;

            _context.SaveChanges();

            return RedirectToAction(nameof(AdminVenues));
        }



        public IActionResult AdminPhotographers()
        {
            int PId = Convert.ToInt32(HttpContext.Session.GetInt32("id"));
            var MyPage = _context.Photographers.FirstOrDefault(p => p.Id == PId);

            if (MyPage == null)
            {
                return View();
            }

            return View(MyPage);
        }


        [HttpPost]
        public IActionResult HandelAdminPhotographers(Photographer Editphoto)
        {
            int VId = Convert.ToInt32(HttpContext.Session.GetInt32("id"));
            var existingPhoto = _context.Photographers.Find(VId);

            if (existingPhoto == null)
            {
                return NotFound();
            }


            existingPhoto.Name = Editphoto.Name;
            existingPhoto.Email = Editphoto.Email;
            existingPhoto.Phone = Editphoto.Phone;
            existingPhoto.Price = Editphoto.Price;


            _context.SaveChanges();

            return RedirectToAction(nameof(AdminPhotographers));
        }


        public IActionResult AdminDresses()
        {

            int DId = Convert.ToInt32(HttpContext.Session.GetInt32("id"));
            var MyPage = _context.Dresses.FirstOrDefault(p => p.Id == DId);

            if (MyPage == null)
            {
                return View();
            }

            return View(MyPage);

        }


        [HttpPost]
        public IActionResult HandelAdminDresses(Dress Editdress)
        {
            int DId = Convert.ToInt32(HttpContext.Session.GetInt32("id"));
            var existingDress = _context.Dresses.Find(DId);

            if (existingDress == null)
            {
                return NotFound();
            }


            existingDress.Name = Editdress.Name;
            existingDress.Email = Editdress.Email;
            existingDress.Phone = Editdress.Phone;
            existingDress.Price = Editdress.Price;
            existingDress.Size = Editdress.Size;
            existingDress.Brand = Editdress.Brand;

            _context.SaveChanges();

            return RedirectToAction(nameof(AdminDresses));
        }


        public IActionResult Gallery()
        {
            int serId = Convert.ToInt32(HttpContext.Session.GetInt32("id"));
            string Type_ser = HttpContext.Session.GetString("Role");

            var images = _context.Images
                                 .Where(i => i.ServiceId == serId && i.ServiceType == Type_ser)
                                 .ToList();
            return View(images);
        }



        [HttpPost]
        public IActionResult UploadImage(IFormFile imageFile, string description)
        {
            int serId = Convert.ToInt32(HttpContext.Session.GetInt32("id"));
            string Type_ser = HttpContext.Session.GetString("Role");




            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError("", "Please select an image.");
                return RedirectToAction("Gallery");
            }

            //  wwwroot/uploads
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }


            var image = new Image
            {
                ServiceType = Type_ser,
                ServiceId = serId,
                ImageUrl = "/uploads/" + uniqueFileName,
                Description = description
            };

            _context.Images.Add(image);
            _context.SaveChanges();

            return RedirectToAction("Gallery");
        }




        public IActionResult DeleteImage(int id)
        {
            var image = _context.Images.Find(id);
            if (image == null)
            {
                return NotFound();
            }


            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, image.ImageUrl.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }


            _context.Images.Remove(image);
            _context.SaveChanges();

            return RedirectToAction("Gallery");
        }



        public IActionResult Manage_Requests()
        {
            int id = Convert.ToInt32(HttpContext.Session.GetInt32("id"));
            string typeBook = HttpContext.Session.GetString("Role");


            var query = from b in _context.Bookings
                        join u in _context.Users on b.UserId equals u.Id
                        select new BookingViewModel
                        {
                            BookingId = b.Id,
                            Name = u.Name,
                            Email = u.Email,
                            Phone = u.Phone,
                            BookingDate = b.BookingDate,
                            StartTime = b.StartTime,
                            EndTime = b.EndTime,
                            NumberOfAttendees = b.NumberOfAttendees ?? 0, 
                            Message = b.Message,
                            Status = b.Status,
                            VenueId = b.VenueId,
                            UserId = b.UserId,
                            PhotographersId = b.PhotographerId,
                            DressesId = b.DressId,
                            TypeBook = b.TypeBook
                        };


            switch (typeBook)
            {
                case "Venue":
                    query = query.Where(b => b.VenueId == id);
                    break;
                case "Photogeapher":
                    query = query.Where(b => b.PhotographersId == id);
                    break;
                case "Dress":
                    query = query.Where(b => b.DressesId == id);
                    break;
                default:
                    return BadRequest("Invalid Type_Book value");
            }

            var result = query.ToList();
            return View(result);
        }

        public IActionResult Manage_Requests_2()
        {
            int id = Convert.ToInt32(HttpContext.Session.GetInt32("id"));
            string typeBook = HttpContext.Session.GetString("Role");


            var query = from b in _context.Bookings
                        join u in _context.Users on b.UserId equals u.Id
                        select new BookingViewModel
                        {
                            BookingId = b.Id,
                            Name = u.Name,
                            Email = u.Email,
                            Phone = u.Phone,
                            BookingDate = b.BookingDate,
                            StartTime = b.StartTime,
                            EndTime = b.EndTime,
                            Message = b.Message,
                            Status = b.Status,
                            VenueId = b.VenueId,
                            UserId = b.UserId,
                            PhotographersId = b.PhotographerId,
                            DressesId = b.DressId,
                            TypeBook = b.TypeBook
                        };

            switch (typeBook)
            {
                case "Venue":
                    query = query.Where(b => b.VenueId == id);
                    break;
                case "Photogeapher":
                    query = query.Where(b => b.PhotographersId == id);
                    break;
                case "Dress":
                    query = query.Where(b => b.DressesId == id);
                    break;
                default:
                    return BadRequest("Invalid Type_Book value");
            }

            var result = query.ToList();
            return View(result);
        }

        [HttpPost]
        public IActionResult AcceptRequest(int id,int Price)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking != null)
            {
                booking.Status = "Approved";
                booking.Price = Price;
                _context.SaveChanges();
            }
            return RedirectToAction("Manage_Requests");
        }


        public IActionResult AcceptRequest_2(int id)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking != null)
            {
                booking.Status = "Approved";
                _context.SaveChanges();
            }
            return RedirectToAction("Manage_Requests_2");
        }

        public IActionResult CancelRequest(int id)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking != null)
            {
                booking.Status = "Cancelled";
                _context.SaveChanges();
            }
            return RedirectToAction("Manage_Requests");
        }

        public IActionResult CancelRequest_2(int id)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking != null)
            {
                booking.Status = "Cancelled";
                _context.SaveChanges();
            }
            return RedirectToAction("Manage_Requests_2");
        }

        public IActionResult ChangePasswordAdmin()
        {
            return View();
        }


        [HttpPost]
        public IActionResult HandelChangePasswordAd(string pwd_current, string pwd_new, string pwd_confirm)
        {


            if (pwd_current == null || pwd_new == null || pwd_confirm == null)
            {
                ViewBag.note = "You must enter all files.";
                return View("ChangePasswordAdmin");
            }

            if (pwd_new != pwd_confirm)
            {

                ViewBag.note = "New password and confirm password do not match.";
                return View("ChangePasswordAdmin");
            }

            string Role = HttpContext.Session.GetString("Role");
            var Id = HttpContext.Session.GetInt32("id");


            if (Role == "SuperAdmin")
            {
                var SuperAd = _context.Users.FirstOrDefault(s => s.Id == Id);

                if (SuperAd == null)
                {
                    ViewBag.note = "Invalid Data,please try again!";
                    return View("ChangePasswordAdmin");
                }


                if (SuperAd.Password != pwd_current)
                {
                    ViewBag.note = "The current password is incorrect!";
                    return View("ChangePasswordAdmin");
                }

                SuperAd.Password = pwd_new;
                _context.SaveChanges();
                ViewBag.note = "Password successfully changed!";
                return View("ChangePasswordAdmin");
            }








            if (Role == "Venue")
            {
                var Ven = _context.Venues.FirstOrDefault(v => v.Id == Id);

                if (Ven == null)
                {
                    ViewBag.note = "Invalid Data,please try again!";
                    return View("ChangePasswordAdmin");
                }


                if (Ven.Password != pwd_current)
                {
                    ViewBag.note = "The current password is incorrect!";
                    return View("ChangePasswordAdmin");
                }

                Ven.Password = pwd_new;
                _context.SaveChanges();
                ViewBag.note = "Password successfully changed!";
                return View("ChangePasswordAdmin");
            }

            if (Role == "Photogeapher")
            {
                var Ven = _context.Photographers.FirstOrDefault(v => v.Id == Id);

                if (Ven == null)
                {
                    ViewBag.note = "Invalid Data,please try again!";
                    return View("ChangePasswordAdmin");
                }


                if (Ven.Password != pwd_current)
                {
                    ViewBag.note = "The current password is incorrect!";
                    return View("ChangePasswordAdmin");
                }

                Ven.Password = pwd_new;
                _context.SaveChanges();
                ViewBag.note = "Password successfully changed!";
                return View("ChangePasswordAdmin");
            }


            if (Role == "Dress")
            {
                var Ven = _context.Dresses.FirstOrDefault(v => v.Id == Id);




                if (Ven == null)
                {
                     ViewBag.note = "Invalid Data,please try again!";
                    return View("ChangePasswordAdmin");
                }


                if (Ven.Password != pwd_current)
                {
                    ViewBag.note = "The current password is incorrect!";
                    return View("ChangePasswordAdmin");
                }


                Ven.Password = pwd_new;
                _context.SaveChanges();
                ViewBag.note = "Password successfully changed!";
                return View("ChangePasswordAdmin");
            }



            return View("ChangePasswordAdmin");
        }





        public IActionResult SuperAdmin()
        {
            int superId = Convert.ToInt32(HttpContext.Session.GetInt32("id"));
            string Type_ser = HttpContext.Session.GetString("Role");

            var superAdmin = _context.Users.FirstOrDefault(s => s.Id == superId);

            if (superAdmin == null)
            {
                return View();
            }


            return View(superAdmin);
        }


        [HttpPost]
        public IActionResult HandelSuperAdmin(User Admin)
        {
            if(!ModelState.IsValid)
            {
                return View("SuperAdmin");
            }

            var superAdmin = _context.Users.Find(Admin.Id);

            if (superAdmin == null) {
            
                return BadRequest();
            }

            superAdmin.Name = Admin.Name;
            superAdmin.Phone = Admin.Phone;

            _context.SaveChanges();

            return View("SuperAdmin",superAdmin);
        }




        public IActionResult Venue_Management()
        {
            var Venue = _context.Venues.ToList();

            return View(Venue);
        }

        public IActionResult Photographer_Management()
        {
            var Photo = _context.Photographers.ToList();

            return View(Photo);
        }

        public IActionResult Dress_Management()
        {
            var Dress = _context.Dresses.ToList();

            return View(Dress);
        
        }


        public IActionResult User_Management()
        {

            var User = _context.Users.Where(u => u.UserType == "User");
            return View(User);
        }



       
        public IActionResult HandelAcceptVenue(int id)
        {
            var ven = _context.Venues.Find(id);
            if (ven == null)
            {
                return BadRequest();
            }

            ven.Status = "Accepted";
            _context.SaveChanges();
            return RedirectToAction("Venue_Management", ven);

        }

        public IActionResult HandelAcceptPhoto(int id)
        {

            var ven = _context.Photographers.Find(id);
            if (ven == null)
            {
                return BadRequest();
            }

            ven.Status = "Accepted";
            _context.SaveChanges();
            return RedirectToAction("Photographer_Management", ven);
        }


        public IActionResult HandelAcceptDress(int id)
        {

            var ven = _context.Dresses.Find(id);
            if (ven == null)
            {
                return BadRequest();
            }

            ven.Status = "Accepted";
            _context.SaveChanges();
            return RedirectToAction("Dress_Management", ven);
        }




















        public IActionResult HandelRejectVenue(int id)
        {
            var ven = _context.Venues.Find(id);
            if (ven == null)
            {
                return BadRequest();
            }

            ven.Status = "Rejected";
            _context.SaveChanges();
            return RedirectToAction("Venue_Management", ven);

        }

        public IActionResult HandelRejectPhoto(int id)
        {

            var ven = _context.Photographers.Find(id);
            if (ven == null)
            {
                return BadRequest();
            }

            ven.Status = "Rejected";
            _context.SaveChanges();
            return RedirectToAction("Photographer_Management", ven);
        }


        public IActionResult HandelRejectDress(int id)
        {

            var ven = _context.Dresses.Find(id);
            if (ven == null)
            {
                return BadRequest();
            }

            ven.Status = "Rejected";
            _context.SaveChanges();
            return RedirectToAction("Dress_Management", ven);
        }



        

























     








        public IActionResult HandelBlockVenue(int id)
        {
            var ven = _context.Venues.Find(id);
            if (ven == null)
            {
                return BadRequest();
            }

            ven.IsActive = false;
            _context.SaveChanges();
            return RedirectToAction("Venue_Management", ven);

        }

        public IActionResult HandelBlockPhoto(int id)
        {

            var ven = _context.Photographers.Find(id);
            if (ven == null)
            {
                return BadRequest();
            }

            ven.IsActive = false;
            _context.SaveChanges();
            return RedirectToAction("Photographer_Management", ven);
        }


        public IActionResult HandelBlockDress(int id)
        {

            var ven = _context.Dresses.Find(id);
            if (ven == null)
            {
                return BadRequest();
            }

            ven.IsActive = false;
            _context.SaveChanges();
            return RedirectToAction("Dress_Management", ven);

        }


            
        


        public IActionResult HandelBlockUser(int id)
        {

            var ven = _context.Users.Find(id);
            if (ven == null)
            {
                return BadRequest();
            }

            ven.IsActive = false;
            _context.SaveChanges();
            return RedirectToAction("User_Management", ven);
        }

        public IActionResult SpecialPlaces()
        {
            var specialItems = _context.FeaturedItems.ToList();

            var topVenues = new List<Venue>();

            foreach (var item in specialItems)
            {
                var venue = _context.Venues.FirstOrDefault(v => v.Id == item.ItemId);
                if (venue != null)
                {
                    topVenues.Add(venue);
                }
            }

            return View(topVenues);
        }


        public IActionResult HandelAddToFeature(int id)
        {
            var Feature = _context.FeaturedItems.FirstOrDefault(F => F.ItemId == id);

            if (Feature != null)
            {
                TempData["Msg"] = "The item is already installed, please choose another item.";
                return RedirectToAction("Venue_Management");
            }

            var NewFeature = new FeaturedItem();

            NewFeature.ItemId = id;
            NewFeature.ItemType = "Special";
            _context.FeaturedItems.Add(NewFeature);
            _context.SaveChanges();

            TempData["Msg"] = "The item was Pin successfully.";
            return RedirectToAction("Venue_Management");
        }


        public IActionResult RemoveTopVenue(int id)
        {
            var specialItems = _context.FeaturedItems.FirstOrDefault(s => s.ItemId == id);


            if(specialItems == null)
            {
                return RedirectToAction("SpecialPlaces");
            }
           
            _context.FeaturedItems.Remove(specialItems);
            _context.SaveChanges();
            return RedirectToAction("SpecialPlaces");
        }


        public IActionResult ShowAllFeedBack()
        {
            var FeedBack = _context.Feedbacks.ToList();

            return View(FeedBack);
        }


        public IActionResult ShowAllPayment()
        {
            var bookings = _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Venue)
                .Include(b => b.Photographer)
                .Include(b => b.Dress)
                .Select(b => new BookingDetailsViewModel
                {
                    Id = b.Id,
                    BookingDate = b.BookingDate.ToDateTime(TimeOnly.MinValue),
                    Price = b.Price,
                    Message = b.Message,
                    TypeBook = b.TypeBook,
                    Status = b.Status,
                    UserName = b.User != null ? b.User.Name : "N/A",
                    VenueName = b.Venue != null ? b.Venue.Name : "N/A",
                    PhotographerName = b.Photographer != null ? b.Photographer.Name : "N/A",
                    DressName = b.Dress != null ? b.Dress.Name : "N/A"
                })
                .ToList();

            return View(bookings);
        }



    }
}
