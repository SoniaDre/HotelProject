using HotelSo.Data;
using HotelSo.Models;
using HotelSo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelSo.Repositories
{
    public class AmenitiesRepository : IAmenitiesRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<AmenitiesRepository> _logger;

        public AmenitiesRepository(ApplicationDbContext db, ILogger<AmenitiesRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<List<AmenitiesPerRoom>> GetAmenitiesAsync()
        {
            return await _db.Amenities.Include(a => a.Rooms).ToListAsync();
        }

        public async Task AddAmenitiesAsync(AmenitiesPerRoom amenities)
        {
            if (amenities == null)
            {
                throw new ArgumentNullException(nameof(amenities));
            }
            try
            {
                await _db.Amenities.AddAsync(amenities);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding amenities");
                throw;
            }
        }

        public async Task UpdateAmenitiesAsync(AmenitiesPerRoom amenities)
        {
            if (amenities == null)
            {
                throw new ArgumentNullException(nameof(amenities));
            }

            try
            {
                _db.Amenities.Attach(amenities);
                _db.Entry(amenities).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating amenities");
                throw;
            }
        }

        public async Task DeleteAmenitiesAsync(int id)
        {
            try
            {
                var amenities = await _db.Amenities.FindAsync(id);
                if (amenities != null)
                {
                    _db.Amenities.Remove(amenities);
                    await _db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting amenities");
                throw;
            }
        }

        public async Task<AmenitiesPerRoom> FindByIdAsync(int id)
        {
            return await _db.Amenities.FindAsync(id);
        }
    }
}

