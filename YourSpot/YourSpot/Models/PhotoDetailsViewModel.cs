namespace YourSpot.Models
{
    public class PhotoDetailsViewModel
    {
        public Photographer Photo { get; set; }
        public IEnumerable<Image> images { get; set; }
    }
}
