using HotelSo.Models;

namespace HotelSo.Repositories.Interfaces
{
    public interface IReservationsRepository
    {
        Task<IEnumerable<Reservation>> GetReservationsAsync();
        Task<IEnumerable<Reservation>> GetReservationsFutureAsync();
        Task<List<Reservation>> GetUserReservationsAsync(string id);
        bool IsValid(DateTime value);
        Task<bool> CheckReservationAsync(Reservation reservation);
        Task<bool> AddReservationAsync(Reservation reservation);
        Task<Reservation> FindByIdAsync(int id);
        Task<bool> UpdateReservationAsync(Reservation reservation);
        Task<bool> DeleteReservationAsync(int id);
    }
}
