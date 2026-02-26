using System.ComponentModel.DataAnnotations;

namespace guest_house_management_backend.Models
{
    public class RoomAmenity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        public ICollection<RoomTypeAmenity> RoomTypeAmenities { get; set; } = new List<RoomTypeAmenity>();
    }
}
