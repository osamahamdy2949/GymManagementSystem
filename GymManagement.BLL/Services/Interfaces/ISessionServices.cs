using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.SessionViewModels;
using GymManagement.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface ISessionServices
    {
        Task<IEnumerable<SessionViewModel>?> GetAllSessionsAsync(CancellationToken ct = default);
        Task<Result<SessionViewModel>> GetSessionByIdAsync(int id , CancellationToken ct = default);
        Task<Result<UpdateSessionViewModel>> GetSessionToUpdate(int id, CancellationToken ct = default);
        Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default);
        Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default);
        Task<Result> RemoveSessionAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<TrainerSelectViewModel>> GetTrainerforDropDownAsync(CancellationToken ct = default);
        Task<IEnumerable<CategorySelectViewModel>> GetCategoryforDropDownAsync(CancellationToken ct = default);
    }
}
