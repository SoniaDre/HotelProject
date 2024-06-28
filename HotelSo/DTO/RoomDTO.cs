using HotelSo.Models;

namespace HotelSo.DTO
{
    public class RoomDTO
    {
        public Room Room { get; set; }
        public List<AmenitiesPerRoom> Amenities { get; set; } = new List<AmenitiesPerRoom>();
        public IFormFile? postedFile { get; set; }
    }
}
