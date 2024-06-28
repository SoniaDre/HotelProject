using HotelSo.Models;
using HotelSo.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelSo.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AmenitiesController : Controller
    {
        private readonly IAmenitiesRepository _amenitiesRepository;

        public AmenitiesController(IAmenitiesRepository amenitiesRepository)
        {
            _amenitiesRepository = amenitiesRepository;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var amenities = await _amenitiesRepository.GetAmenitiesAsync();
            return View(amenities);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AmenitiesPerRoom amenities)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(amenities);
            //}

            await _amenitiesRepository.AddAmenitiesAsync(amenities);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var amenities = await _amenitiesRepository.FindByIdAsync(id);
            if (amenities == null)
            {
                return NotFound();
            }
            return View(amenities);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AmenitiesPerRoom amenities)
        {

            await _amenitiesRepository.UpdateAmenitiesAsync(amenities);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var amenities = await _amenitiesRepository.FindByIdAsync(id);
            if (amenities == null)
            {
                return NotFound();
            }
            return View(amenities);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _amenitiesRepository.DeleteAmenitiesAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var amenities = await _amenitiesRepository.FindByIdAsync(id);
            if (amenities == null)
            {
                return NotFound();
            }
            return View(amenities);
        }
    }
}