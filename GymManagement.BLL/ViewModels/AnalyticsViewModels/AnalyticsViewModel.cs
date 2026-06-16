using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.AnalyticsViewModels
{
    public class AnalyticsViewModel
    {
        public int TotalMembers { get; set; }
        public int TotalActiveMembers { get; set; }
        public int TotalTrainers { get; set; }
        public int TotalUpcomingSessions { get; set; }
        public int TotalOngoingSessions { get; set; }
        public int TotalCompletedSessions { get; set; }
    }
}
