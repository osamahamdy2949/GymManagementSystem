using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MembershipViewModels;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.SessionViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class MembershipServices : IMembershipServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MembershipServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result> CreateMembershipAsync(CreateMembershipViewModel model, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(model.MemberId, ct: ct);
            if (member == null) return Result.NotFound("Member Not Found");

            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(model.PlanId, ct: ct);
            if (plan == null) return Result.NotFound("Plan Not Found");
            if (!plan.IsActive) return Result.Fail("Plan is not active.");

            var today = DateOnly.FromDateTime(DateTime.Now);
            var startDate = model.StartDate.HasValue ? DateOnly.FromDateTime(model.StartDate.Value) : today;
            if (startDate < today) return Result.Validation("Start date cannot be in the past.");

            var hasActive = await _unitOfWork.MembershipRepository
                           .AnyAsync(m => m.MemberId == model.MemberId && m.EndDate > DateOnly.FromDateTime(DateTime.Now), ct);
            if (hasActive) return Result.Fail("Member already has an active membership.");

            var createdMembership = _mapper.Map<Membership>(model);
            createdMembership.CreatedAt = startDate.ToDateTime(TimeOnly.MinValue);
            createdMembership.EndDate = startDate.AddDays(plan.DurationDays);

            _unitOfWork.GetRepository<Membership>().Add(createdMembership);

            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed To Create Membership");
        }

        public async Task<IEnumerable<MembershipViewModel>> GetAllMembershipsAsync(CancellationToken ct = default)
        {
            var memberships = await _unitOfWork.MembershipRepository
                .GetAllMembershipsWithMembersAndPlans(ct: ct);
            return _mapper.Map<IEnumerable<MembershipViewModel>>(memberships);
        }
        public async Task<IEnumerable<PlanSelectListViewModel>> GetPlansForDropDownAsync(CancellationToken ct = default)
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(p => p.IsActive, ct: ct);
            return _mapper.Map<IEnumerable<PlanSelectListViewModel>>(plans);
        }

        public async Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropDownAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<MemberSelectListViewModel>>(members);
        }

        public async Task<Result> DeleteActiveMembershipAsync(int membershipId, CancellationToken ct = default)
        {
            var membership = await _unitOfWork.GetRepository<Membership>().GetByIdAsync(membershipId, true, ct);
            if (membership is null) return Result.NotFound("Membership Not Found");

            if (!membership.IsActive) return Result.Fail("Membership is already expired.");

            _unitOfWork.MembershipRepository.Delete(membership);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed To Cancel Membership");
        }
    }
}
