using GymManagement.DAL.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.TrainerViewModels
{
    public class CreateTrainerViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^01[0125][0-9]{8}$",
            ErrorMessage = "Invalid Egyptian phone number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public int BuildingNumber { get; set; }

        [Required]
        public string Street { get; set; } = default!;

        [Required]
        public string City { get; set; } = default!;

        [Required]
        public Speciality Speciality { get; set; }
    }
}
