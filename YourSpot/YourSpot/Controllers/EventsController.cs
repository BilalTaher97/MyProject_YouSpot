using Microsoft.AspNetCore.Mvc;
using YourSpot.Models;

namespace YourSpot.Controllers
{
    public class EventsController : Controller
    {

        private readonly MyDbContext _context;

        public EventsController(MyDbContext context)
        {
            _context = context;
        }
        public IActionResult Wedding()
        {

            var allVenue = _context.Venues
                .Select(d => new WeddingViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    EstablishmentName = d.EstablishmentName,
                    EstablishmentType = d.EstablishmentType,
                    Price = Convert.ToDecimal(d.Price),
                    Capacity = Convert.ToInt32(d.Capacity),
                    Address = d.Address,
                    WeddingVenueTypeice = d.WeddingVenueType,
                    Status = d.Status,
                    FirstImagePath = _context.Images
                        .Where(img => img.ServiceId == d.Id && img.ServiceType == "Venue")
                        .OrderBy(img => img.Id)
                        .Select(img => img.ImageUrl)
                        .FirstOrDefault()
                })
                .ToList();

            return View(allVenue);
        }


        public IActionResult Event()
        {
            var allEvent = _context.Venues
               .Select(d => new EventViewModel
               {
                   Id = d.Id,
                   Name = d.Name,
                   EstablishmentName = d.EstablishmentName,
                   EstablishmentType = d.EstablishmentType,
                   Price = Convert.ToDecimal(d.Price),
                   Capacity = Convert.ToInt32(d.Capacity),
                   Address = d.Address,
                   Status = d.Status,
                   FirstImagePath = _context.Images
                        .Where(img => img.ServiceId == d.Id && img.ServiceType == "Venue")
                        .OrderBy(img => img.Id)
                        .Select(img => img.ImageUrl)
                        .FirstOrDefault()
               })
               .ToList();

            return View(allEvent);
        }


        public IActionResult Photography()
        {
            var allPhoto = _context.Photographers
                .Select(d => new PhotographerViewModel
                {
                    Id = d.Id,                
                    Price = Convert.ToDecimal(d.Price),
                    Status = d.Status,
                    FirstImagePath = _context.Images
                        .Where(img => img.ServiceId == d.Id && img.ServiceType == "Photogeapher")
                        .OrderBy(img => img.Id)
                        .Select(img => img.ImageUrl)
                        .FirstOrDefault()
                })
                .ToList();

            return View(allPhoto);
        }



        public IActionResult Reg_Photography()
        {


            return View();
        }

        [HttpPost]
        public IActionResult HandelAddPhotography(Photographer AddPhotographer)
        {

            if (ModelState.IsValid)
            {
                AddPhotographer.Status = "Pending";
                AddPhotographer.IsActive = true;
                _context.Add(AddPhotographer);
                _context.SaveChanges();
                TempData["SuccessMessage_1"] = "Thank you for your request. Your request has been sent successfully!";
                return RedirectToAction("Reg_Photography");
            }

            TempData["ErrorMessage_1"] = "There are some problems, please try again!";
            return RedirectToAction("Reg_Photography");
        }

        public IActionResult Dresses_Gallery()
        {
            var allDresses = _context.Dresses
                .Select(d => new DressWithImageViewModel
                {
                    Id = d.Id,
                    Brand = d.Brand,
                    Price = Convert.ToDecimal(d.Price),
                    Size = d.Size,
                    Status = d.Status,
                    FirstImagePath = _context.Images
                        .Where(img => img.ServiceId == d.Id && img.ServiceType == "Dress")
                        .OrderBy(img => img.Id)
                        .Select(img => img.ImageUrl)
                        .FirstOrDefault()
                })
                .ToList();

            return View(allDresses);
        }

        public IActionResult Reg_Dresses()
        {
            return View();
        }

        [HttpPost]
        public IActionResult HandelAddDresses(Dress dresses)
        {

            if (ModelState.IsValid)
            {
                dresses.Status = "Pending";
                dresses.IsActive = true;
                _context.Add(dresses);
                _context.SaveChanges();
                TempData["SuccessMessage_2"] = "Thank you for your request. Your request has been sent successfully!";
                return RedirectToAction("Reg_Dresses");
            }

            TempData["ErrorMessage_2"] = "There are some problems, please try again!";
            return RedirectToAction("Reg_Dresses");
        }
    }
}
