using HotelSo.Data;
using HotelSo.Models.Enum;
using HotelSo.Models;
using Microsoft.EntityFrameworkCore;
using HotelSo.Repositories.Interfaces;

namespace HotelSo.Repositories
{
    public class RoomsRepository: IRoomsRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<RoomsRepository> _logger;

        public RoomsRepository(ApplicationDbContext db, ILogger<RoomsRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<IEnumerable<Room>> GetRoomsAsync()
        {
            return await _db.Rooms
                            .Include(r => r.Reservations)
                            .Include(r => r.Amenities)
                            .OrderBy(r => r.Price)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetRoomTypesGeneralAsync()
        {
            var roomTypes = Enum.GetValues(typeof(RoomType)).Cast<RoomType>();
            var rooms = new List<Room>();

            foreach (var type in roomTypes)
            {
                var room = await _db.Rooms.FirstOrDefaultAsync(r => r.RoomType == type);
                if (room != null)
                {
                    rooms.Add(room);
                }
            }

            return rooms;
        }

        public async Task<IEnumerable<Room>> GetRoomTypesAsync(RoomType roomType)
        {
            return await _db.Rooms
                            .Where(r => r.RoomType == roomType)
                            .OrderBy(r => r.Price)
                            .ToListAsync();
        }

        public async Task AddRoomAsync(Room room)
        {
            if (room == null)
            {
                throw new ArgumentNullException(nameof(room));
            }

            try
            {
                await _db.Rooms.AddAsync(room);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding room");
                throw;
            }
        }

        public async Task UpdateRoomAsync(Room room)
        {
            if (room == null)
            {
                throw new ArgumentNullException(nameof(room));
            }

            try
            {
                _db.Rooms.Update(room);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating room");
                throw;
            }
        }

        public async Task DeleteRoomAsync(int id)
        {
            try
            {
                var room = await _db.Rooms.FindAsync(id);
                if (room != null)
                {
                    _db.Rooms.Remove(room);
                    await _db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting room");
                throw;
            }
        }

        public async Task<Room> FindByIdAsync(int id)
        {
            return await _db.Rooms
                            .Include(r => r.Reservations)
                            .Include(r => r.Amenities)
                            .SingleOrDefaultAsync(r => r.Id == id);
        }
        public async Task<IEnumerable<Room>> GetFreeRoomsAsync(DateTime start, DateTime end, RoomType roomType)
        {
            return await _db.Rooms
                            .Include(r => r.Amenities)
                            .Include(r => r.Reservations)
                            .Where(r => r.RoomType == roomType && r.Reservations.All(res => res.DepartureDate <= start || res.ArrivalDate >= end))
                            .OrderBy(r => r.Price)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetAllFreeRoomsAsync(DateTime start, DateTime end)
        {
            return await _db.Rooms
                            .Include(r => r.Amenities)
                            .Include(r => r.Reservations)
                            .Where(r => r.Reservations.All(res => res.DepartureDate <= start || res.ArrivalDate >= end))
                            .OrderBy(r => r.Price)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetAdvancedFreeRoomsAsync(DateTime start, DateTime end, RoomType roomType, MaxPersons maxPersons)
        {
            return await _db.Rooms
                            .Include(r => r.Amenities)
                            .Include(r => r.Reservations)
                            .Where(r => r.RoomType == roomType && r.MaxPersons == maxPersons && r.Reservations.All(res => res.DepartureDate <= start || res.ArrivalDate >= end))
                            .OrderBy(r => r.Price)
                            .ToListAsync();
        }
       
        public async Task<List<Reservation>> GetRoomReservationsAsync(int id)
        {
            return await _db.Reservations
                            .Include(r => r.Room)
                            .Include(r => r.ApplicationUser)
                            .Where(reservation => reservation.RoomId == id)
                            .ToListAsync();
        }
    }
}
