using HotelSo.Models;
using System.ComponentModel.DataAnnotations;

namespace HotelSo.DTO
{
    public class SearchDTO
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [Required]
        [Display(Name = "Arrival")]
        [FutureDate]
        public DateTime ArrivalDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d/M/yyyy}", ApplyFormatInEditMode = true)]
        [Required]
        [Display(Name = "Departure")]
        [FutureDate]
        public DateTime DepartureDate { get; set; }

        public IEnumerable<Room>? Rooms { get; set; }
        public IEnumerable<Reservation>? Reservations { get; set; }
        public IEnumerable<Room>? RoomsIndex { get; set; } 

        public Room? Room { get; set; }
    }
}
