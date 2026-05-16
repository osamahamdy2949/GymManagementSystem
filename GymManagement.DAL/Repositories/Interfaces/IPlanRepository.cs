using GymManagement.DaL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IPlanRepository
    {
            Task<IEnumerable<Plan>> GetAllPlansAsync(bool tracking = false , CancellationToken ct = default);
            Task<Plan?> GetPlanByIdAsync(int id, CancellationToken ct = default);
            Task<int> AddPlanAsync(Plan plan, CancellationToken ct = default);
            Task<int> UpdatePlanAsync(Plan plan, CancellationToken ct = default);
            Task<int> DeletePlanAsync(int id, CancellationToken ct = default);
    }
}
