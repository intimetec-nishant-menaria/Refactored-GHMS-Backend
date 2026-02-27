namespace guest_house_management_backend.Models
{
    public class RoomTypeAmenity
    {
        public int RoomTypeId { get; set; }
        public RoomType? RoomType { get; set; }

        public int AmenityId { get; set; }
        public RoomAmenity? Amenity { get; set; }

    }
}
