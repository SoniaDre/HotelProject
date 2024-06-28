using HotelSo.DTO;
using HotelSo.Repositories;
using HotelSo.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelSo.Controllers
{

    public class AdminController : Controller
    {
        private readonly IRoomsRepository _roomsRepository;
        private readonly IAmenitiesRepository _amenitiesRepository;
        private readonly IReservationsRepository _reservationsRepository;

        public AdminController(IRoomsRepository roomsRepository, IAmenitiesRepository amenitiesRepository, IReservationsRepository reservationsRepository)
        {
            _roomsRepository = roomsRepository;
            _amenitiesRepository = amenitiesRepository;
            _reservationsRepository = reservationsRepository;
        }

        public IActionResult Reservations()
        {
            return RedirectToAction("IndexFuture", "Reservations");
        }

        public async Task<IActionResult> Rooms()
        {
            var vm = new SearchDTO
            {
                Rooms = await _roomsRepository.GetRoomsAsync()
            };
            return View(vm);
        }

        public async Task<IActionResult> AvailableRooms(SearchDTO vm)
        {
            if (!ModelState.IsValid || vm.ArrivalDate == default || vm.DepartureDate == default)
            {
                return RedirectToAction("Index", "Home");
            }

            vm.Rooms = await _roomsRepository.GetAllFreeRoomsAsync(vm.ArrivalDate, vm.DepartureDate);

            return View(vm);
        }

        public IActionResult Users()
        {
            return RedirectToAction("ListOfUsers", "Users");
        }


        public IActionResult Amenities()
        {
            return RedirectToAction("Index", "Amenities");
        }
    }
}
