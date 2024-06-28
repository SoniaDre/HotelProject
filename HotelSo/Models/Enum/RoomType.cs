using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelSo.Models.Enum
{
    public enum RoomType
    {

        [Display(Name = "Classic Room")]
        ClassicRoom =0,
        [Display(Name = "Premium Room")]
        PremiumRoom =1,
        [Display(Name = "Classic Suite")]
        ClassicSuite=2,
        [Display(Name = "Premium Suite")]
        PremiumSuite=3
    }
}