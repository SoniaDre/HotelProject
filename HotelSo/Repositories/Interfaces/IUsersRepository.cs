using HotelSo.Data;

namespace HotelSo.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<IEnumerable<ApplicationUser>> GetApplicationUsersAsync();
        Task<ApplicationUser> FindUserByIdAsync(string id);
        Task EditAsync(ApplicationUser user);
        Task<IEnumerable<ApplicationUser>> SearchUsersAsync(string searchTerm);
    }
}
