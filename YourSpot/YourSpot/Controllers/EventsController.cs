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

            var AllWedding = _context.Venues.Where(x => x.EstablishmentType == "Wedding").ToList();


            return View(AllWedding);
        }


        public IActionResult Event()
        {
            var specialVenues = _context.FeaturedItems
                .Where(f => f.ItemType == "Venue")
                .Select(f => f.ItemId)
                .ToList();

            var viewModel = new EventsViewModel
            {
                Event = _context.Venues.Where(_x => _x.EstablishmentType != "Wedding").ToList(),
                SpecialEvent = _context.Venues 
                .Where(v => specialVenues.Contains(v.Id))
                .Take(3)
                .ToList()
            };

            return View(viewModel);
        }

        public IActionResult Photography()
        {
            var specialVenues = _context.FeaturedItems
                 .Where(f => f.ItemType == "Photographer")
                 .Select(f => f.ItemId)
                 .ToList();

            var viewModel = new PhotographerViewModel
            {
                AllPhoto = _context.Photographers.ToList(),
                SpecialPhoto = _context.Photographers
                .Where(v => specialVenues.Contains(v.Id))
                .Take(3)
                .ToList()
            };

            return View(viewModel);
        }

        public IActionResult Reg_Photography()
        {
           

            return View();
        }

        [HttpPost]
        public IActionResult HandelAddPhotography(Photographer AddPhotographer)
        {
            
            if(ModelState.IsValid)
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
            var AllDresses = _context.Dresses.ToList();

            return View(AllDresses);
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
