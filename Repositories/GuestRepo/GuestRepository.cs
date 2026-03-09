using guest_house_management_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace guest_house_management_backend.Repositories.GuestRepo
{
    public class GuestRepository : IGuestRepository
    {
        private readonly Data.DBContext _context;

        public GuestRepository(Data.DBContext context)
        {
            _context = context;
        }

        public async Task<List<Guest>> GetAllAsync()
        {
            return await _context.Guest.ToListAsync();
        }

        public async Task<Guest?> GetByIdAsync(int guestId)
        {
            return await _context.Guest.FindAsync(guestId);
        }

        public async Task<List<Guest>> SearchAsync(string search)
        {
            return await _context.Guest
                .Where(g => g.Name.Contains(search)
                         || g.Email.Contains(search)
                         || g.Contact.Contains(search))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> IsDuplicateAsync(string email, string contact)
        {
            return await _context.Guest
                .AnyAsync(g => g.Email == email || g.Contact == contact);
        }

        public async Task AddAsync(Guest guest)
        {
            await _context.Guest.AddAsync(guest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guest guest)
        {
            _context.Guest.Update(guest);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guest guest)
        {
            _context.Guest.Remove(guest);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Booking>> GetGuestBookings(int guestId)
        {
            return await _context.Bookings
                .Where(b => b.GuestId == guestId)
                .Include(b => b.Room)
                .ToListAsync();
        }
    }
}
