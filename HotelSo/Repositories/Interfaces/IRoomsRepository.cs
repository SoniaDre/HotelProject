using HotelSo.Models.Enum;
using HotelSo.Models;

namespace HotelSo.Repositories.Interfaces
{
    public interface IRoomsRepository
    {
        Task<IEnumerable<Room>> GetRoomsAsync();
        Task<IEnumerable<Room>> GetRoomTypesGeneralAsync();
        Task<IEnumerable<Room>> GetRoomTypesAsync(RoomType roomType);
        Task AddRoomAsync(Room room);
        Task UpdateRoomAsync(Room room);
        Task DeleteRoomAsync(int id);
        Task<Room> FindByIdAsync(int id);
        Task<IEnumerable<Room>> GetFreeRoomsAsync(DateTime start, DateTime end, RoomType roomType);
        Task<IEnumerable<Room>> GetAllFreeRoomsAsync(DateTime start, DateTime end);
        Task<IEnumerable<Room>> GetAdvancedFreeRoomsAsync(DateTime start, DateTime end, RoomType roomType, MaxPersons maxPersons);
        Task<List<Reservation>> GetRoomReservationsAsync(int id);
    }
}
