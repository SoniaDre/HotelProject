using HotelSo.Data;
using HotelSo.Models;

namespace HotelSo.DTO
{
    public class ReservationDTO
    {
        public Reservation Reservation { get; set; }
        public Room Room { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
