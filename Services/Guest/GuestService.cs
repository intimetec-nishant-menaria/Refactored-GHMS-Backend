//using guest_house_management_backend.DTOs;
//using guest_house_management_backend.Repositories.GuestRepo;
//using guest_house_management_backend.DTOs.Paging;
//using AutoMapper;

//namespace guest_house_management_backend.Services.Guest
//{
//    public class GuestService : IGuestService
//    {
//        public readonly IGuestRepository _guestRepository;
//        private readonly IMapper _mapper;

//        public GuestService(IGuestRepository guestRepository,IMapper mapper)
//        {
//            _guestRepository = guestRepository;
//            _mapper = mapper;
//        }

//        public async Task<Paging<GuestResponseDto>> GetAllGuestsAsync(int pageNumber , int pageSize , string searchUser)
//        {
//            return await _guestRepository.GetAllAsync(pageNumber , pageSize , searchUser);
//        }

//        public async Task CreateGuestAsync(CreateGuestDto createdRequest)
//        {
//            var isDuplicate = await _guestRepository
//                .IsDuplicateAsync(createdRequest.Email, createdRequest.Contact);

//            if (isDuplicate)
//                throw new InvalidOperationException("Guest with this Email or Contact already exists.");

//            var guest = new Models.Guest
//            {
//                Name = createdRequest.Name,
//                Email = createdRequest.Email,
//                Contact = createdRequest.Contact,
//                IDProof = createdRequest.IDProof,
//                Address = createdRequest.Address,
//                EmergencyContact = createdRequest.EmergencyContact,
//                CreatedAt = DateTime.UtcNow
//            };

//            await _guestRepository.AddAsync(guest);
//        }

//        public async Task DeleteGuestAsync(int guestId)
//        {
//            var guest = await _guestRepository.GetByIdAsync(guestId);
//            if (guest == null)
//                throw new KeyNotFoundException("Guest not found.");

//            await _guestRepository.DeleteAsync(guest);
//        }

//        public async Task<GuestResponseDto> GetGuestByIdAsync(int guestId)
//        {
//            var guest = await _guestRepository.GetByIdAsync(guestId);
//            if (guest == null)
//                throw new KeyNotFoundException("Guest not found.");

//            return _mapper.Map<GuestResponseDto>(guest);
//        }

//        public async Task UpdateGuestAsync(int guestId, UpdateGuestDto updateRequest)
//        {
//            var guest = await _guestRepository.GetByIdAsync(guestId);
//            if (guest == null)
//                throw new KeyNotFoundException("Guest not found.");

//            var isDuplicate = await _guestRepository
//                .IsDuplicateAsync(updateRequest.Email, updateRequest.Contact);
//            var duplicateGuest = await _guestRepository.GetByEmailAsync(updateRequest.Email);

//            if (isDuplicate &&  duplicateGuest!=null && guest.Id != duplicateGuest.Id )
//                throw new InvalidOperationException("Guest with this Email or Contact already exists.");

//            guest.Name = updateRequest.Name;
//            guest.Email = updateRequest.Email;
//            guest.Contact = updateRequest.Contact;
//            guest.IDProof = updateRequest.IDProof;
//            guest.Address = updateRequest.Address;
//            guest.EmergencyContact = updateRequest.EmergencyContact;
//            guest.UpdatedAt = DateTime.UtcNow;

//            await _guestRepository.UpdateAsync(guest);
//        }

//        public async Task<IEnumerable<GuestResponseDto>> SearchGuestsAsync(string search)
//        {
//            if (string.IsNullOrWhiteSpace(search))
//            {
//                return Enumerable.Empty<GuestResponseDto>();
//            }

//            var guests = await _guestRepository.SearchAsync(search);

//            return _mapper.Map<IEnumerable<GuestResponseDto>>(guests);
//        }
//        public async Task<GuestBookingHistoryResponseDto> GetGuestBookingHistoryAsync(int guestId, GuestBookingHistoryQueryDto guestBookingDto)
//        {
//            var bookings = await _guestRepository.GetGuestBookings(guestId);

//            var totalRecords = bookings.Count;

//            var paginatedBookings = bookings
//                .Skip((guestBookingDto.pageNumber - 1) * guestBookingDto.pageSize)
//                .Take(guestBookingDto.pageSize)
//                .ToList();

//            var bookingDtos = paginatedBookings.Select(b => new BookingHistoryDto
//            {
//                BookingId = b.Id,
//                RoomNumber = b.Room.RoomNumber,
//                CheckIn = b.CheckInDate,
//                CheckOut = b.CheckOutDate,
//                FinalAmount = b.FinalBillAmount ?? 0,
//                Status = b.Status.ToString()
//            }).ToList();

//            var totalNights = bookings.Sum(b => (b.CheckOutDate - b.CheckInDate).Days);

//            var totalSpent = bookings.Sum(b => b.FinalBillAmount ?? 0);

//            var preferredRoomType = bookings
//                .GroupBy(b => b.Room.RoomType)
//                .OrderByDescending(g => g.Count())
//                .Select(g => g.Key.ToString())
//                .FirstOrDefault();

//            var preferredFloor = bookings
//                .GroupBy(b => b.Room.FloorNumber)
//                .OrderByDescending(g => g.Count())
//                .Select(g => g.Key)
//                .FirstOrDefault();

//            return new GuestBookingHistoryResponseDto
//            {
//                GuestId = guestId,
//                NumberOfVisits = bookings.Count,
//                TotalNights = totalNights,
//                TotalSpent = totalSpent,
//                PreferredRoomType = preferredRoomType,
//                PreferredFloor = preferredFloor,
//                TotalRecords = totalRecords,
//                Bookings = bookingDtos
//            };
//        }
//    }
//}
