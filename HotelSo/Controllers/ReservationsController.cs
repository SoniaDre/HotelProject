using Azure.Core;
using HotelSo.Data.Migrations;
using HotelSo.Data;
using HotelSo.Models;
using HotelSo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using HotelSo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace HotelSo.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IReservationsRepository _reservationsRepository;
        private readonly IRoomsRepository _roomsRepository;

        public ReservationsController(ApplicationDbContext db, IReservationsRepository reservationsRepository, IRoomsRepository roomsRepository)
        {
            _db = db;
            _reservationsRepository = reservationsRepository;
            _roomsRepository = roomsRepository;
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var reservations = await _reservationsRepository.GetReservationsAsync();
            return View(reservations);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> IndexFuture()
        {
            var reservations = await _reservationsRepository.GetReservationsFutureAsync();
            return View(reservations);
        }

        public async Task<IActionResult> Create(int id)
        {
            try
            {
                var room = await _roomsRepository.FindByIdAsync(id);
                if (room == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var reservation = new Reservation
                {
                    RoomId = room.Id,
                    ArrivalDate = DateTime.Now,
                    DepartureDate = DateTime.Now.AddDays(1)
                };

                return View(reservation);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            reservation.UserId = userId;

            if (!await _reservationsRepository.CheckReservationAsync(reservation))
            {
                ViewBag.MessageAvailability = "Not Available in this period";
                return View("Create", reservation);
            }

            await _reservationsRepository.AddReservationAsync(reservation);
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var reservation = await _reservationsRepository.FindByIdAsync(id);
            if (reservation == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(reservation);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var reservation = await _reservationsRepository.FindByIdAsync(id);
            if (reservation == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Rooms = await _roomsRepository.GetRoomsAsync();
            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(Reservation reservation)
        {
            //if (!ModelState.IsValid)
            //{
            //    ViewBag.Rooms = await _roomsRepository.GetRoomsAsync();
            //    return View(reservation);
            //}

            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //reservation.UserId = userId;
            var reservationExists = await _reservationsRepository.FindByIdAsync(reservation.Id);
            if (reservationExists == null)
            {
                return RedirectToAction("Index", "Home");
            }
            reservationExists.DepartureDate = reservation.DepartureDate;
            reservationExists.ArrivalDate = reservation.ArrivalDate;

            await _reservationsRepository.UpdateReservationAsync(reservationExists);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _reservationsRepository.FindByIdAsync(id);
            if (reservation == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(reservation);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _reservationsRepository.DeleteReservationAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ProceedToCheckout(Reservation reservation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            reservation.UserId = userId;

            var isAvailable = await _reservationsRepository.CheckReservationAsync(reservation);
            if (!isAvailable)
            {
                ViewBag.MessageAvailability = "This room is not available in this period";
                return RedirectToAction("Create", "Reservations", new { id = reservation.RoomId });
            }

            var isValidDates = _reservationsRepository.IsValid(reservation.ArrivalDate) && _reservationsRepository.IsValid(reservation.DepartureDate);
            if (!isValidDates)
            {
                ViewBag.MessageFutureDates = "Future Dates Please";
                return RedirectToAction("Create", "Reservations", new { id = reservation.RoomId });
            }

            reservation.ApplicationUser = await _db.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

            var room = await _roomsRepository.FindByIdAsync(reservation.RoomId);
            if (room == null)
            {
                return RedirectToAction("Index", "Home");
            }
            reservation.Room = room;

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            //// Assuming you're using ASP.NET Core's session handling, which requires setup in Startup.cs
            HttpContext.Session.SetString("ReservationSession", JsonSerializer.Serialize(reservation,options));
            

            return View(reservation);

        }

        // Create a new action method to checkout and create a reservation
        public async Task<IActionResult> Checkout(int reservationId)
        {
            // Retrieve the reservation from the session
            var sessionData = HttpContext.Session.GetString("ReservationSession");
            if (string.IsNullOrEmpty(sessionData))
            {
                // Handle the case where the session data is not found
                return RedirectToAction("Index", "Home");
            }

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            var reservation = JsonSerializer.Deserialize<Reservation>(sessionData, options);

            // Do the payment processing here
            // For example, you can use the PayPal SDK to create a payment
            // https://developer.paypal.com/docs/api/payments/v2/

            // After the payment is successful, save the reservation to the database
            Reservation reservation1 = new Reservation
            {
                ArrivalDate = reservation.ArrivalDate,
                DepartureDate = reservation.DepartureDate,
                RoomId = reservation.RoomId,
                UserId = reservation.UserId,
            };
            var reserved = await _reservationsRepository.AddReservationAsync(reservation1);

            HttpContext.Session.Remove("ReservationSession");
            
            if (reserved)
            {
               TempData.Add("ReservationSuccess", "Reservation created successfully");
            }

            return RedirectToAction("Index", "Home");
        }

        
    }
}
