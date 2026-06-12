using AutoMapper;
using GymManagement.BLL.Common;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlanServices(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);

            if (!plans.Any())
                return Enumerable.Empty<PlanViewModel>();

            return _mapper.Map<IEnumerable<PlanViewModel>>(plans);
        }

        public async Task<PlanDetailsViewModel?> GetPlanByIdAsync(int id, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, false, ct);
            if (plan == null)
                return null;

            return _mapper.Map<PlanDetailsViewModel?>(plan);
        }

        public async Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int id, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, true, ct);

            if (plan is null || !plan.IsActive)
                return null;

            if (await HasActiveMembershipsAsync(id, ct))
                return null;

            return _mapper.Map<UpdatePlanViewModel>(plan);
        }

        public async Task<Result> UpdatePlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, true, ct);

            if (plan is null)
                return Result.NotFound("Plan not found");

            if(await HasActiveMembershipsAsync(id, ct))
                return Result.Validation("Plan has active memberships");

            _mapper.Map(model, plan);

            plan.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Plan>().Update(plan);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed to update plan");
        }

        public async Task<Result> UpdateStatusAsync(int id, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, true, ct);

            if (plan is null)
                return Result.NotFound("Plan not found");

            if(!plan.IsActive && await HasActiveMembershipsAsync(id, ct))
                return Result.Validation("Plan has active memberships");

            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Plan>().Update(plan);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed to update plan status");
        }

        //Helper method to check if Plan has Active Membership or not
        public async Task<bool> HasActiveMembershipsAsync(int planId, CancellationToken ct = default)
        {
            return await _unitOfWork.GetRepository<Membership>().AnyAsync(m => m.PlanId == planId && m.EndDate > DateOnly.FromDateTime(DateTime.Now), ct: ct);
        }
    }
}
