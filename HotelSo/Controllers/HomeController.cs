using HotelSo.DTO;
using HotelSo.Models;
using HotelSo.Repositories;
using HotelSo.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HotelSo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRoomsRepository _roomsRepository;   
        private readonly IReservationsRepository _reservationsRepository;
        public HomeController(ILogger<HomeController> logger, IRoomsRepository roomsRepository, IReservationsRepository reservationsRepository)
        {
            _logger = logger;
            _roomsRepository = roomsRepository;
            _reservationsRepository = reservationsRepository;
        }

        public async Task<IActionResult> Index()
        {
           
            SearchDTO vm = new SearchDTO();
            vm.ArrivalDate = DateTime.Now;
            vm.DepartureDate = (DateTime.Now).AddDays(1);
            vm.Rooms = await _roomsRepository.GetRoomTypesGeneralAsync();
            vm.RoomsIndex = await _roomsRepository.GetRoomsAsync();
            vm.Reservations = await _reservationsRepository.GetReservationsAsync();
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
		public IActionResult Restaurant()
		{
			return View();
		}
		public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";
        //    //SearchViewModel vm = new SearchViewModel();
        //    //vm.Rooms = _roomsRepository.GetRooms();
        //    //vm.Reservations = _reservationsRepository.GetReservations();

        //    return View();
        //}
    }
}
