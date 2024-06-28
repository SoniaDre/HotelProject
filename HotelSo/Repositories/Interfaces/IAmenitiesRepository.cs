
using HotelSo.Models;

namespace HotelSo.Repositories.Interfaces
{
    public interface IAmenitiesRepository
    {
        Task<List<AmenitiesPerRoom>> GetAmenitiesAsync();
        Task AddAmenitiesAsync(AmenitiesPerRoom amenities);
        Task UpdateAmenitiesAsync(AmenitiesPerRoom amenities);
        Task DeleteAmenitiesAsync(int id);
        Task<AmenitiesPerRoom> FindByIdAsync(int id);
    }
}
