using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.Models.Enums
{
    public enum BloodType
    {
        [Display(Name = "A+")]
        Aplus = 1,

        [Display(Name = "A-")]
        Aminus,

        [Display(Name = "B+")]
        Bplus,

        [Display(Name = "B-")]
        Bminus,

        [Display(Name = "AB+")]
        ABplus,

        [Display(Name = "AB-")]
        ABminus,

        [Display(Name = "O+")]
        Oplus,

        [Display(Name = "O-")]
        Ominus
    }
}
