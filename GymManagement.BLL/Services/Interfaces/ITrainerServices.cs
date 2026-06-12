using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface ITrainerServices
    {
        Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default);
        Task<TrainerViewModel?> GetTrainerByIdAsync(int id, CancellationToken ct = default);
        Task<UpdateTrainerViewModel?> GetTrainerToUpdateAsync(int id, CancellationToken ct = default);
        Task<Result> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default);
        Task<Result> UpdateTrainerAsync(int id, UpdateTrainerViewModel model,CancellationToken ct = default);
        Task<Result> DeleteTrainerAsync(int id, CancellationToken ct = default);
    }
}
