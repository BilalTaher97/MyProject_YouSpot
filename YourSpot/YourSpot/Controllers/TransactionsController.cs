using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourSpot.Models;

namespace YourSpot.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly MyDbContext _context;

        public TransactionsController(MyDbContext context)
        {
            _context = context;
        }
        public IActionResult Details(int id,string type)
        {

            var DetailsVenue = new EventsDetailsViewModel()
            {
                venue = _context.Venues.Find(id),

                images = _context.Images.Where(i => i.ServiceId == id && i.ServiceType == type),

            };

            return View(DetailsVenue);
        }


        public IActionResult HandelFav(int id,string type)
        {

            if (HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Log_Reg", "User");
            }


            int userId = HttpContext.Session.GetInt32("id").Value;


            var addFav = new Favorite
            {
                UserId = userId,
                ItemId = id,
                ItemType = "Venues"
            };


            _context.Add(addFav);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "The item has been successfully added to favorites!";


            return RedirectToAction("Details", new { id, type });
        }

        public IActionResult HandelFav_Photo(int id,string type)
        {

            if (HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Log_Reg", "User");
            }


            int userId = HttpContext.Session.GetInt32("id").Value;


            var addFav = new Favorite
            {
                UserId = userId,
                ItemId = id,
                ItemType = "Photography"
            };


            _context.Add(addFav);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "The item has been successfully added to favorites!";


            return RedirectToAction("Details_Phtotography", new { id, type });
        }

        public IActionResult HandelFav_Dresses(int id , string type)
        {

            if (HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Log_Reg", "User");
            }


            int userId = HttpContext.Session.GetInt32("id").Value;


            var addFav = new Favorite
            {
                UserId = userId,
                ItemId = id,
                ItemType = "Dresses"
            };


            _context.Add(addFav);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "The item has been successfully added to favorites!";


            return RedirectToAction("Details_Dresses", new { id, type });

        }

        public IActionResult Payment(int id)
        {
            TempData["BookingId"] = id;
            return View();
        }


        public IActionResult ProcessPayment(string CardNumber, string Name)
        {

            int bookingId = (int)TempData["BookingId"];




            int? userId = HttpContext.Session.GetInt32("id");
            if (userId == null)
            {
                TempData["ErrorMessage"] = "❌ You must be logged in to make a payment!";
                return RedirectToAction("Login");
            }

          
            var Pay = new Payment
            {
                CardNumber = CardNumber,
                Name = Name,
                Date = DateOnly.FromDateTime(DateTime.Now),
                BookingId = bookingId,
                UserId = userId.Value
            };

            _context.Payments.Add(Pay);


            if (_context.SaveChanges() > 0)
            {
                var UpBooking = _context.Bookings.FirstOrDefault(b => b.Id == bookingId);
                if (UpBooking != null)
                {
                  
                    UpBooking.Status = "Completed"; 
                    _context.Bookings.Update(UpBooking); 
                    if (_context.SaveChanges() > 0)
                    {
                        TempData["SuccessMessage_P"] = "🎉 Thank you! Your transaction was successful. We are happy to serve you and wish you a wonderful day! 😊";
                        return RedirectToAction("SuccessPage");
                    }
                    else
                    {
                        TempData["ErrorMessage_P"] = "❌ Failed to update booking status. Please try again!";
                        return RedirectToAction("Payment", new { id = bookingId });
                    }
                }
                else
                {
                    TempData["ErrorMessage_P"] = "❌ Booking not found.";
                    return RedirectToAction("Payment", new { id = bookingId });
                }
            }
            else
            {
                TempData["ErrorMessage_P"] = "❌ Payment failed. Please try again!";
                return RedirectToAction("Payment", new { id = bookingId });
            }

        }

        public IActionResult SuccessPage()
        {
           
            return View();
        }

        public IActionResult Payform(int id)
        {
            TempData["VenId"] = id;

            return View();
        }

        [HttpPost]
        public IActionResult HandelPayform(TimeOnly startTime, TimeOnly endTime, DateOnly eventDate, string message)
        {
            if (HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Log_Reg", "User");
            }

            var booking = new Booking();
            if (ModelState.IsValid)
            {
                booking.BookingDate = eventDate;
                booking.StartTime = startTime;
                booking.EndTime = endTime;
                booking.Message = message;
                booking.VenueId = Convert.ToInt32(TempData["VenId"]);
                booking.UserId = HttpContext.Session.GetInt32("id").Value;
                booking.Status = "Pending";
                booking.TypeBook = "Venues";

                _context.Add(booking);
                _context.SaveChanges();

                TempData["SuccessMessage_3"] = "Thank you for your booking. Your request has been sent successfully!";
                return RedirectToAction("Payform");
            }

            TempData["ErrorMessage_3"] = "There are some problems, please try again!";
            return RedirectToAction("Payform");
        }



        public IActionResult Payform_Photo(int id)
        {
            TempData["photoId"] = id;

            return View();
        }

        [HttpPost]
        public IActionResult HandelPayform_photo(DateOnly bookingDate , string message)
        {
            if (HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Log_Reg", "User");
            }

            var booking = new Booking();
            if (ModelState.IsValid)
            {
                booking.BookingDate = bookingDate;
                booking.Message = message;
                booking.PhotographersId = Convert.ToInt32(TempData["photoId"]);
                booking.UserId = HttpContext.Session.GetInt32("id").Value;
                booking.Status = "Pending";
                booking.TypeBook = "Photographers";
                _context.Add(booking);
                _context.SaveChanges();
                TempData["SuccessMessage_4"] = "Thank you for your booking. Your request has been sent successfully!";
                return RedirectToAction("Payform_Photo");
            }


            TempData["ErrorMessage_4"] = "There are some problems, please try again!";
            return RedirectToAction("Payform_Photo");
        }

        public IActionResult Payform_dresses(int id)
        {
            TempData["dressId"] = id;

            return View();
        }

        [HttpPost]
        public IActionResult HandelPayform_dress(DateOnly bookingDate, string message)
        {
            if (HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Log_Reg", "User");
            }

            var booking = new Booking();
            if (ModelState.IsValid)
            {
                booking.BookingDate = bookingDate;
                booking.Message = message;
                booking.DressesId = Convert.ToInt32(TempData["dressId"]);
                booking.UserId = HttpContext.Session.GetInt32("id").Value;
                booking.Status = "Pending";
                booking.TypeBook = "Dresses";
                _context.Add(booking);
                _context.SaveChanges();
                TempData["SuccessMessage_5"] = "Thank you for your booking. Your request has been sent successfully!";
                return RedirectToAction("Payform_dresses");
            }


            TempData["ErrorMessage_5"] = "There are some problems, please try again!";
            return RedirectToAction("Payform_dresses");
        }


        public IActionResult HandelRequest()
        {




            return View();
        }

        public IActionResult Details_Phtotography(int id,string type)
        {
            var DetailsPhoto = new PhotoDetailsViewModel()
            {
                Photo = _context.Photographers.Find(id),

                images = _context.Images.Where(i => i.ServiceId == id && i.ServiceType == type),

            };



            return View(DetailsPhoto);
        }

        public IActionResult Details_Dresses(int id,string type)
        {
            var Detailsdresse = new DressDetailsViewModel()
            {
                dress = _context.Dresses.Find(id),

                images = _context.Images.Where(i => i.ServiceId == id && i.ServiceType == type),
                
            };

            


            return View(Detailsdresse);
        }

    }
}
