using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;

namespace HotelSo.Models
{
    public class AmenitiesPerRoom
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]

        [Display(Name = "Amenities Package")]
        public string AmenitiesPackage { get; set; }
        public bool Tv { get; set; }

        [Display(Name = "Air Condition")]
        public bool AirCondition { get; set; }
        public bool Sauna { get; set; }

        [Display(Name = "Alarm Clock")]
        public bool AlarmClock { get; set; }

        [Display(Name = "Bath Amenities")]
        public bool BathAmenities { get; set; }

        [Display(Name = "Central Heating")]
        public bool CentralHeating { get; set; }

        [Display(Name = "Coffee Facilities")]
        public bool CoffeeFacilities{ get; set; }

        [Display(Name = "Voice Mail")]
        public bool VoiceMail { get; set; }


        public bool Hairdryer { get; set; }

        [Display(Name = "Mini-Bar")]
        public bool MiniBar { get; set; }

        [Display(Name = "Movie Channels")]
        public bool MovieChannels { get; set; }
        public bool Slippers { get; set; }

        [Display(Name = "Wake-up Call")]
        public bool WakeUpCall { get; set; }

        [Display(Name = "Free Wi-Fi ")]

        public bool FreeWiFi  { get; set; }
        public bool Closet { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}