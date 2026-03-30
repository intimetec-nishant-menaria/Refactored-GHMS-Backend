//using guest_house_management_backend.DTOs;
//using guest_house_management_backend.DTOs.Paging;

//namespace guest_house_management_backend.Services.Guest
//{
//    public interface IGuestService
//    {
//        public Task<Paging<GuestResponseDto>> GetAllGuestsAsync(int pageNumber ,int pageSize ,string searchUser);
//        public Task<GuestResponseDto> GetGuestByIdAsync(int guestId);

//        public Task<IEnumerable<GuestResponseDto>> SearchGuestsAsync(string search);

//        public Task CreateGuestAsync(CreateGuestDto createRequest);

//        public Task UpdateGuestAsync(int guestId, UpdateGuestDto updateRequest);

//        public Task DeleteGuestAsync(int guestId);
//        public Task<GuestBookingHistoryResponseDto> GetGuestBookingHistoryAsync(int guestId, GuestBookingHistoryQueryDto queryDto);
//    }
//}
