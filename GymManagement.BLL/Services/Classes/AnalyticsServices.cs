using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.AnalyticsViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class AnalyticsServices : IAnalyticsServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AnalyticsViewModel> GetAnalyticsAsync(CancellationToken ct)
        {
            var totalMembers = await _unitOfWork.GetRepository<Member>().CountAsync();
            var totalActiveMember = await _unitOfWork.GetRepository<Member>().CountAsync
                (m => m.Memberships.Any());
            var totalTrainers = await _unitOfWork.GetRepository<Trainer>().CountAsync();
            var totalUpcomingSession = await _unitOfWork.GetRepository<Session>().CountAsync
                (s => s.StartDate > DateTime.Now);
            var totalOngoingSession = await _unitOfWork.GetRepository<Session>().CountAsync
                (s => s.StartDate <= DateTime.Now && s.EndDate >= DateTime.Now);
            var totalCompleteSession = await _unitOfWork.GetRepository<Session>().CountAsync
                (s => s.EndDate < DateTime.Now);

            var model = new AnalyticsViewModel()
            {
                TotalMembers = totalMembers,
                TotalActiveMembers = totalActiveMember,
                TotalTrainers = totalTrainers,
                TotalUpcomingSessions = totalUpcomingSession,
                TotalOngoingSessions = totalOngoingSession,
                TotalCompletedSessions = totalCompleteSession
            };

            return model;
        }
    }
}
