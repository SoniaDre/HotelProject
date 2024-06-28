using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HotelSo.Models;

namespace HotelSo.Data
{
    public class ApplicationUser : IdentityUser

    {
        [Required]
        [MaxLength(50)]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(50)]
        public string Lastname { get; set; }
        [Phone]
        [Required]
        public string Telephone { get; set; }
        [NotMapped]
        public string FullName
        {
            get { return Firstname + " " + Lastname; }
        }

        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
