using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.PlansViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class PlanServices : IPlanServices
    {
        private readonly IGenericRepository<Plan> _planRepository;

        public PlanServices(IGenericRepository<Plan> planRepository) 
        { 
            _planRepository = planRepository; 
        }
        
        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await _planRepository.GetAllAsync(ct:ct);

            if (!plans.Any())
                return Enumerable.Empty<PlanViewModel>();

            return plans.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name,
                DurationDays = p.DurationDays,
                Description = p.Description,
                Price = p.Price,
                IsActive = p.IsActive

            });
        }

        public async Task<PlanDetailsViewModel?> GetPlanByIdAsync(int id, CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(id, false, ct);
            if (plan == null)
                return null;

            return new PlanDetailsViewModel
            {
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                IsActive = plan.IsActive
            };
        }

        public async Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int id, CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(id, true, ct);

            if (plan is null)
                return null;

            return new UpdatePlanViewModel()
            {
                Id = plan.Id,
                Name = plan.Name,
                Price = plan.Price,
                DurationDays = plan.DurationDays,
                Description = plan.Description,
            };
        }

        public async Task<bool> UpdatePlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(id, true, ct);

            if (plan is null)
                return false;

            plan.Id = id;
            plan.Name = model.Name;
            plan.DurationDays = model.DurationDays;
            plan.Description = model.Description;
            plan.Price = model.Price;

            var result = await _planRepository.UpdateAsync(plan, ct);

            return result > 0 ? true : false;
        }

        public async Task<bool> UpdateStatusAsync(int id, CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(id, true, ct);

            if (plan is null)
                return false;

            plan.IsActive = !plan.IsActive;

            var result = await _planRepository.UpdateAsync(plan, ct);

            return result > 0 ? true : false;
        }
    }
}
