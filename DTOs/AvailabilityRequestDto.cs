using guest_house_management_backend.Enums;

namespace guest_house_management_backend.DTOs
{
    public class AvailabilityRequestDto
    {
        public GenderEnum Gender { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
    }
}
