using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.MemberViewModels
{
    public class MemberDetailsViewModel
    {
        public int Id { get; set; }
        public string? Photo { get; set; }
        public string? PlanName { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; }
        public string PhoneNumber { get; set; } = default!;
        public string Gender { get; set; } = default!;
        public DateTime? MembershipStartDate { get; set; }
        public DateOnly? MembershipEndDate { get; set; }
        public string Address { get; set; } = default!;

    }
}
