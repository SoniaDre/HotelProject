using HotelSo.Data;
using HotelSo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelSo.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ReservationsRepository> _logger;

        public UsersRepository(ApplicationDbContext db, ILogger<ReservationsRepository> logger)
        {
            _db = db;         
            _logger = logger;
        }

        public async Task<IEnumerable<ApplicationUser>> GetApplicationUsersAsync()
        {
            return await _db.Users
                            .Include(u => u.Reservations)
                            .ToListAsync();
        }

        public async Task<ApplicationUser> FindUserByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Id cannot be null or whitespace.", nameof(id));
            }

            return await _db.Users
                            .Include(u => u.Reservations)
                            .SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task EditAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            try
            {
                _db.Users.Update(user);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in edit user");
                throw; 
            }
        }

        public async Task<IEnumerable<ApplicationUser>> SearchUsersAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Enumerable.Empty<ApplicationUser>();
            }

            return await _db.Users
                            .Where(user => user.Firstname.Contains(searchTerm) ||
                                           user.Lastname.Contains(searchTerm) ||
                                           user.Email.Contains(searchTerm) ||
                                           user.UserName.Contains(searchTerm))
                            .Include(u => u.Reservations)
                            .ToListAsync();
        }
    }
}
