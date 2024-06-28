using HotelSo.Data.Migrations;
using HotelSo.Data;
using HotelSo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HotelSo.Repositories.Interfaces;

namespace HotelSo.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IReservationsRepository _reservationsRepository;

        public UsersController(IUsersRepository usersRepository, IReservationsRepository reservationsRepository)
        {
            _usersRepository = usersRepository;
            _reservationsRepository = reservationsRepository;
        }

        public async Task<IActionResult> UserProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Correct way to get user ID

            var user = await _usersRepository.FindUserByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            user.Reservations = await _reservationsRepository.GetUserReservationsAsync(userId);
            return View(user);
        }

        [Authorize(Roles = "Administrator")] 
        public async Task<IActionResult> ListOfUsers()
        {
            var users = await _usersRepository.GetApplicationUsersAsync();
            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _usersRepository.FindUserByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            await _usersRepository.EditAsync(user);
            return RedirectToAction("ListOfUsers");
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await _usersRepository.FindUserByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            user.Reservations = await _reservationsRepository.GetUserReservationsAsync(id);
            return View(user);
        }

        public async Task<IActionResult> SearchUser(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return View(Enumerable.Empty<ApplicationUser>());
            }

            var users = await _usersRepository.SearchUsersAsync(searchTerm);
            return View(users);
        }
    }
}
