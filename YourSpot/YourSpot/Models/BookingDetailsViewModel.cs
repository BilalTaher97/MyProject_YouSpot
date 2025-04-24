namespace YourSpot.Models
{
    public class BookingDetailsViewModel
    {


        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal? Price { get; set; }
        public string Message { get; set; }
        public string TypeBook { get; set; }
        public string Status { get; set; }

        
        public string UserName { get; set; }
        public string VenueName { get; set; }
        public string PhotographerName { get; set; }
        public string DressName { get; set; }


    }
}
