using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.SessionViewModels;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GymManagement.BLL.Services.Classes.SessionServices;

namespace GymManagement.BLL.Services.Classes
{
    public class SessionServices : ISessionServices
    {

        private readonly IUnitOfWork _unitOfWork;

        public SessionServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SessionViewModel>?> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var sessions = await _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct: ct);

            if (sessions?.Any() != true) return null;

            var MappedSessions = sessions.Select(s => new SessionViewModel()
            {
                Id = s.Id,
                Capacity = s.Capacity,
                CategoryName = s.Category.CategoryName,
                TrainerName = s.Trainer?.Name!,
                Description = s.Description,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
            });

            foreach (var session in MappedSessions)
            {
                session.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
            }
            
            return MappedSessions;
        }

    }
}
