using HotelSo.Data;
using HotelSo.Models;
using HotelSo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelSo.Repositories
{
    public class ReservationsRepository : IReservationsRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ReservationsRepository> _logger;

        public ReservationsRepository(ApplicationDbContext db, ILogger<ReservationsRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<IEnumerable<Reservation>> GetReservationsAsync()
        {
            return await _db.Reservations
                            .Include(r => r.ApplicationUser)
                            .Include(r => r.Room)
                            .OrderBy(r => r.ArrivalDate)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetReservationsFutureAsync()
        {
            return await _db.Reservations
                            .Include(r => r.ApplicationUser)
                            .Include(r => r.Room)
                            .Where(r => r.ArrivalDate >= DateTime.UtcNow)
                            .ToListAsync();
        }

        public async Task<List<Reservation>> GetUserReservationsAsync(string id)
        {
            return await _db.Reservations
                            .Include(r => r.Room)
                            .Where(reservation => reservation.UserId == id)
                            .ToListAsync();
        }

        public bool IsValid(DateTime value)
        {
           
            return value.Date >= DateTime.UtcNow.Date;
        }


        public async Task<bool> CheckReservationAsync(Reservation reservation)
        {
            if (reservation == null)
            {
                throw new ArgumentNullException(nameof(reservation));
            }

            try
            {
                var room = await _db.Rooms
                                    .Include(r => r.Reservations)
                                    .FirstOrDefaultAsync(r => r.Id == reservation.RoomId);

                if (room == null || !room.Reservations.Any())
                {
                    return true; 
                }

                foreach (var r in room.Reservations)
                {
                  
                    bool overlaps = reservation.ArrivalDate < r.DepartureDate && reservation.DepartureDate > r.ArrivalDate;
                    if (overlaps)
                    {
                        return false;
                    }
                }

                return true; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking reservation availability");
                return false;
            }
        }

        public async Task<bool> AddReservationAsync(Reservation reservation)
        {
            if (reservation == null)
            {
                throw new ArgumentNullException(nameof(reservation));
            }
            try
            {

                await _db.Reservations.AddAsync(reservation);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding reservation");
                return false;
            }
        }

        public async Task<Reservation> FindByIdAsync(int id)
        {
            return await _db.Reservations
                            .Include(r => r.ApplicationUser)
                            .Include(r => r.Room).Where(i => i.Id == id).FirstOrDefaultAsync();
                            
        }

        public async Task<bool> UpdateReservationAsync(Reservation reservation)
        {
            if (reservation == null)
            {
                throw new ArgumentNullException(nameof(reservation));
            }

            try
            {
                _db.Reservations.Update(reservation);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating reservation");
                return false;
            }
        }

        public async Task<bool> DeleteReservationAsync(int id)
        {
            try
            {
                var reservation = await _db.Reservations.FindAsync(id);
                if (reservation != null)
                {
                    _db.Reservations.Remove(reservation);
                    await _db.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting reservation");
                return false;
            }
        }


    } 
}
