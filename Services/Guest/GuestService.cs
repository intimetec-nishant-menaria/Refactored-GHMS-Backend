using guest_house_management_backend.DTOs;
using guest_house_management_backend.Repositories.GuestRepo;
using guest_house_management_backend.Models;

namespace guest_house_management_backend.Services.Guest
{
    public class GuestService : IGuestService
    {
        public readonly IGuestRepository _guestRepository;
        public GuestService(IGuestRepository guestRepository)
        {
            _guestRepository = guestRepository;
        }

        public async Task<IEnumerable<GuestResponseDto>> GetAllGuestsAsync()
        {
            var guests = await _guestRepository.GetAllAsync();

            return guests.Select(g => new GuestResponseDto
            {
                Id = g.Id,
                Name = g.Name,
                Contact = g.Contact,
                Email = g.Email,
                IDProof = g.IDProof,
                Address = g.Address,
                EmergencyContact = g.EmergencyContact,
                CreatedAt = g.CreatedAt
            });
        }

        public async Task CreateGuestAsync(CreateGuestDto createdRequest)
        {
            var isDuplicate = await _guestRepository
                .IsDuplicateAsync(createdRequest.Email, createdRequest.Contact);

            if (isDuplicate)
                throw new InvalidOperationException("Guest with this Email or Contact already exists.");

            var guest = new Models.Guest
            {
                Name = createdRequest.Name,
                Email = createdRequest.Email,
                Contact = createdRequest.Contact,
                IDProof = createdRequest.IDProof,
                Address = createdRequest.Address,
                EmergencyContact = createdRequest.EmergencyContact,
                CreatedAt = DateTime.UtcNow
            };

            await _guestRepository.AddAsync(guest);
        }

        public async Task DeleteGuestAsync(int guestId)
        {
            var guest = await _guestRepository.GetByIdAsync(guestId);
            if (guest == null)
                throw new KeyNotFoundException("Guest not found.");

            await _guestRepository.DeleteAsync(guest);
        }

        public async Task<GuestResponseDto> GetGuestByIdAsync(int guestId)
        {
            var guest = await _guestRepository.GetByIdAsync(guestId);
            if (guest == null)
                throw new KeyNotFoundException("Guest not found.");

            return new GuestResponseDto
            {
                Id = guest.Id,
                Name = guest.Name,
                Email = guest.Email,
                Contact = guest.Contact,
                IDProof = guest.IDProof,
                Address = guest.Address,
                EmergencyContact = guest.EmergencyContact,
                CreatedAt = guest.CreatedAt
            };
        }

        public async Task UpdateGuestAsync(int guestId, UpdateGuestDto updateRequest)
        {
            var guest = await _guestRepository.GetByIdAsync(guestId);
            if (guest == null)
                throw new KeyNotFoundException("Guest not found.");

            var isDuplicate = await _guestRepository
                .IsDuplicateAsync(updateRequest.Email, updateRequest.Contact);

            if (isDuplicate && (guest.Email != updateRequest.Email || guest.Contact != updateRequest.Contact))
                throw new InvalidOperationException("Guest with this Email or Contact already exists.");

                guest.Name = updateRequest.Name;
                guest.Email = updateRequest.Email;
                guest.Contact = updateRequest.Contact;
                guest.IDProof = updateRequest.IDProof;
                guest.Address = updateRequest.Address;
                guest.EmergencyContact = updateRequest.EmergencyContact;
                guest.UpdatedAt = DateTime.UtcNow;

                await _guestRepository.UpdateAsync(guest);
        }

        public async Task<IEnumerable<GuestResponseDto>> SearchGuestsAsync(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return Enumerable.Empty<GuestResponseDto>();
            }

            var guests = await _guestRepository.SearchAsync(search);

            return guests.Select(g => new GuestResponseDto
            {
                Id = g.Id,
                Name = g.Name,
                Email = g.Email,
                Contact = g.Contact,
                IDProof = g.IDProof,
                Address = g.Address,
                EmergencyContact = g.EmergencyContact,
                CreatedAt = g.CreatedAt
            });
        }
    }
}
