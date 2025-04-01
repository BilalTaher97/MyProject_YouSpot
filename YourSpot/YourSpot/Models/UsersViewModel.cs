
namespace YourSpot.Models
{
    public class UsersViewModel
    {
        public User InfoUser { get; set; }
        public IEnumerable<Booking> booking { get; set; }

      
        public IEnumerable<Venue> FavoriteVenues { get; set; }
        public IEnumerable<Photographer> FavoritePhotography { get; set; }
        public IEnumerable<Dress> FavoriteDresses { get; set; }
    }
}