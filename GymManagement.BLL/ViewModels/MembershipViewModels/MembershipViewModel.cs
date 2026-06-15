using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.MembershipViewModels
{
    public class MembershipViewModel
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string MemberName { get; set; } = default!;
        public string PlanName { get; set; } = default!;
    }
}
