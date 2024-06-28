using HotelSo.Data;
using HotelSo.DTO;
using HotelSo.Models.Enum;
using HotelSo.Repositories;
using HotelSo.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HotelSo.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RoomsController : Controller
    {
        private readonly IRoomsRepository _roomsRepository;
        private readonly IAmenitiesRepository _amenitiesRepository;
        private readonly IReservationsRepository _reservationsRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RoomsController(IRoomsRepository roomsRepository, IAmenitiesRepository amenitiesRepository, IReservationsRepository reservationsRepository, IWebHostEnvironment webHostEnvironment)
        {
            _roomsRepository = roomsRepository;
            _amenitiesRepository = amenitiesRepository;
            _reservationsRepository = reservationsRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var vm = new SearchDTO
            {
                Rooms = await _roomsRepository.GetRoomsAsync(),
                ArrivalDate = DateTime.Now,
                DepartureDate = DateTime.Now.AddDays(1)
            };
            return View(vm);
        }

        [AllowAnonymous]
        public async Task<IActionResult> IndexRoomTypes(RoomType roomType)
        {
            var vm = new SearchDTO
            {
                Rooms = await _roomsRepository.GetRoomTypesAsync(roomType)
            };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new RoomDTO
            {
                Amenities = await _amenitiesRepository.GetAmenitiesAsync()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoomDTO vm, IFormFile postedFile)
        {
            if (postedFile != null)
            {
                var fileName = Path.GetFileName(postedFile.FileName);
                   //get the file path
           
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await postedFile.CopyToAsync(fileStream);
                }
                vm.Room.ProfilePicture = $"/Content/images/{fileName}";
                           
                //vm.Room.ProfilePicture = "/Content/images/" + fileName;
            }
            var roomAmenities = await _amenitiesRepository.FindByIdAsync(vm.Room.AmenitiesId);
            if (roomAmenities != null)
            {
                vm.Amenities.Add(roomAmenities);
            }


            await _roomsRepository.AddRoomAsync(vm.Room);
            return RedirectToAction("Rooms", "Admin");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vm = new RoomDTO
            {
                Amenities = await _amenitiesRepository.GetAmenitiesAsync(),
                Room = await _roomsRepository.FindByIdAsync(id)

            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoomDTO vm, IFormFile postedFile)
        {
            if (postedFile != null)
            {
                var fileName = Path.GetFileName(postedFile.FileName);
    
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await postedFile.CopyToAsync(fileStream);
                }
                vm.Room.ProfilePicture = $"/Content/images/{fileName}";
               
            }



            await _roomsRepository.UpdateRoomAsync(vm.Room);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var room = await _roomsRepository.FindByIdAsync(id);
            return View(room);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _roomsRepository.DeleteRoomAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var room = await _roomsRepository.FindByIdAsync(id);
            room.Reservations = await _roomsRepository.GetRoomReservationsAsync(id);
            return View(room);
        }

        [AllowAnonymous]
        public async Task<IActionResult> SingleRoom(int id)
        {
            var vm = new SearchDTO
            {
                Room = await _roomsRepository.FindByIdAsync(id)
            };
            return View(vm);
        }

        [AllowAnonymous]
        public async Task<ActionResult> GetFreeRooms(SearchDTO vm)
        {
            if (!_reservationsRepository.IsValid(vm.ArrivalDate) || !_reservationsRepository.IsValid(vm.DepartureDate))
            {
                // Redirect to the Home Index page if the model state is invalid or the dates are not valid.
                TempData["ErrorMessage"] = "Please ensure the model state is valid and the dates are correct.";
                return RedirectToAction("Index", "Home");

            }

            try
            {
                vm.Rooms = await _roomsRepository.GetFreeRoomsAsync(vm.ArrivalDate, vm.DepartureDate, vm.Room.RoomType);
                if (!vm.Rooms.Any())
                {
                    ViewBag.RoomsAvailability = "No Rooms Available in this period";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while processing your request.";
            }

            return View(vm);
        }

        [AllowAnonymous]
        public async Task<ActionResult> AdvancedGetFreeRooms(SearchDTO vm)
        {
            if (/*!ModelState.IsValid || */!_reservationsRepository.IsValid(vm.ArrivalDate) || !_reservationsRepository.IsValid(vm.DepartureDate))
            {
                // Redirect to the Home Index page if the model state is invalid or the dates are not valid.
                return RedirectToAction("Index", "Home");
            }

            try
            {
                vm.Rooms = await _roomsRepository.GetAdvancedFreeRoomsAsync(vm.ArrivalDate, vm.DepartureDate, vm.Room.RoomType, vm.Room.MaxPersons);
                if (!vm.Rooms.Any())
                {
                    ViewBag.RoomsAvailability = "No Rooms Available in this period";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while processing your request.";
            }

            return View(vm);
        }
    }
}
