

using HotelSo.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HotelSo.Models
{
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public Bed Bed { get; set; }
        [Required]
        //[DisplayFormat(DataFormatString = "m\u00b2")]
        public int Size { get; set; }
        public View? View { get; set; }

        [Required]
        [Display(Name="Max Persons")]
        public MaxPersons MaxPersons { get; set; }
        [Display(Name = "Type")]
        public RoomType RoomType { get; set; }
        public Floor Floor { get; set; }
        [NotMapped]
        public string EnumStringMaxPesrons
        {
            get
            {
                string value = "";
                switch (MaxPersons)
                {
                    case MaxPersons.One:
                        value = "1";
                        break;
                    case MaxPersons.Two:
                        value = "2";
                        break;
                    case MaxPersons.Three:
                        value = "3";
                        break;
                    case MaxPersons.Four:
                        value = "4";
                        break;
                    case MaxPersons.Five:
                        value = "5";
                        break;
                }
                return value;
            }
        }

        [NotMapped]
        public string EnumStringBed
        {
            get
            {
                string value = "";
                switch (Bed)
                {
                    case Bed.OneS:
                        value = "1 Single";
                        break;
                    case Bed.OneD:
                        value = "1 Double";
                        break;
                    case Bed.TwoS:
                        value = "2 Single";
                        break;
                    case Bed.ThreeDS:
                        value = "1 Double, 1 Single";
                        break;
                    case Bed.ThreeS:
                        value = "3 Singles";
                        break;
                    case Bed.TwoD:
                        value = "2 Doubles";
                        break;
                    case Bed.FourDS:
                        value = "1 Double, 2 Single";
                        break;
                    case Bed.FiveDDS:
                        value = "1 Double, 3 Single";
                        break;
                    case Bed.FiveDSS:
                        value = "2 Double, 3 Single";
                        break;
                    
                }
                return value;
            }
        }
        [NotMapped]
        public string EnumStringRoomType
        {
            get
            {
                string value = "";
                switch (RoomType)
                {
                    case RoomType.ClassicRoom:
                        value = "Classic Room";
                        break;
                    case RoomType.PremiumRoom:
                        value = "Premium Room";
                        break;
                    case RoomType.ClassicSuite:
                        value = "Classic Suite";
                        break;
                    case RoomType.PremiumSuite:
                        value = "Premium Suite";
                        break;
                    }
                return value;
            }
        }
        [NotMapped]
        public string EnumStringFloor
        {
            get
            {
                string value = "";
                switch (Floor)
                {
                    case Floor.One:
                        value = "1";
                        break;
                    case Floor.Two:
                        value = "2";
                        break;
                    case Floor.Three:
                        value = "3";
                        break;
                    case Floor.Four:
                        value = "4";
                        break;
                    case Floor.Five:
                        value = "5";
                        break;
                    case Floor.Six:
                        value = "6";
                        break;
                    case Floor.Seven:
                        value = "7";
                        break;


                }

                return value;
            }
        }
        public int AmenitiesId { get; set; }

        public virtual AmenitiesPerRoom Amenities { get; set; }

        public string ProfilePicture { get; set; }
        
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}