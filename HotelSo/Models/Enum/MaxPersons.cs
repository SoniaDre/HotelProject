using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelSo.Models.Enum
{

    public enum MaxPersons
    {
        
        [Display(Name = "1")]
        One =1,
        [Display(Name = "2")]
        Two =2,
        [Display(Name = "3")]
        Three =3,
        [Display(Name = "4")]
        Four =4,
        [Display(Name = "5")]
        Five =5,
        [Display(Name = "6")]
        Six =6,
        [Display(Name = "7")]
        Seven= 7
    }
}