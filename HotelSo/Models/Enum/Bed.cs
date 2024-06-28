using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelSo.Models.Enum
{
    public enum Bed
    {
    
        [Display(Name ="1 Single")]
        OneS,
        [Display(Name = "1 Double")]
        OneD,
        [Display(Name = "2 Singles")]
        TwoS,
        [Display(Name = "1 Double, 1 Single")]
        ThreeDS,
        [Display(Name = "3 Singles")]
        ThreeS,
        [Display(Name = "2 Doubles")]
        TwoD,
        [Display(Name = "1 Double, 2 Singles")]
        FourDS,
        [Display(Name = "2 Double, 1 Single")]
        FiveDDS,
        [Display(Name = "1 Double, 3 Singles")]
        FiveDSS


    }
}