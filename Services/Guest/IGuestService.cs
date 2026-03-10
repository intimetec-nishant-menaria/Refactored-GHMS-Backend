using guest_house_management_backend.DTOs;

namespace guest_house_management_backend.Services.Guest
{
    public interface IGuestService
    {
        public Task<IEnumerable<GuestResponseDto>> GetAllGuestsAsync();
        public Task<GuestResponseDto> GetGuestByIdAsync(int guestId);

        public Task<IEnumerable<GuestResponseDto>> SearchGuestsAsync(string search);

        public Task CreateGuestAsync(CreateGuestDto createRequest);

        public Task UpdateGuestAsync(int guestId, UpdateGuestDto updateRequest);

        public Task DeleteGuestAsync(int guestId);
    }
}
