namespace YourSpot.Models
{
    public class BookingViewModel
    {
        public int BookingId { get; set; }  // إضافة معرف الحجز
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateOnly BookingDate { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public int Price { get; set; }
        public int NumberOfAttendees { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public int? VenueId { get; set; }
        public int UserId { get; set; }
        public int? PhotographersId { get; set; }
        public int? DressesId { get; set; }
        public string TypeBook { get; set; }
    }



}
