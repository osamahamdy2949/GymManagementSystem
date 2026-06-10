using AutoMapper;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
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
    public class MemberServices : IMemberServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberServices(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateMembreAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var existEmail = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email, ct);
            var existPhone = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.PhoneNumber == model.PhoneNumber, ct);

            if (existEmail || existPhone) return false;

            var createdMember = _mapper.Map<CreateMemberViewModel, Member>(model);

            _unitOfWork.GetRepository<Member>().Add(createdMember);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);

            if (!members.Any())
                return Enumerable.Empty<MemberViewModel>();

            var mappedMembers = _mapper.Map<IEnumerable<Member>,IEnumerable<MemberViewModel>>(members);

            return mappedMembers;
        }
        public async Task<MemberDetailsViewModel?> GetMemberByIdAsync(int id, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id, false, ct);

            if (member == null) return null;

            var model = _mapper.Map<Member, MemberDetailsViewModel>(member);

            var activeMembership = await _unitOfWork.GetRepository<Membership>().FirstOrDefaultAsync(m => m.MemberId == id && m.EndDate > DateOnly.FromDateTime(DateTime.Now), false, ct);

            if (activeMembership is not null)
            {
                var activePlan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(activeMembership.PlanId, false, ct);
                model.PlanName = activePlan?.Name;
                model.MembershipStartDate = activeMembership.CreatedAt;
                model.MembershipEndDate = activeMembership.EndDate;

            }

            return model;
        }
        public async Task<HealthRecordViewModel?> GetHealthRecordByMemberIdAsync(int memberId, CancellationToken ct = default)
        {
            var healthRecord = await _unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(hr => hr.MemberId == memberId, false, ct);

            if (healthRecord == null) return null;

            return _mapper.Map<HealthRecord, HealthRecordViewModel>(healthRecord);
        }

        public async Task<bool> DeleteMemberAsync(int id, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id, false, ct);
            if (member is null)
                return false;

            _unitOfWork.GetRepository<Member>().Delete(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? true : false;
        }

        public async Task<UpdateMemberViewModel?> GetMemberToUpdateAsync(int id, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id, true, ct);

            if (member is null)
                return null;

            return _mapper.Map<Member, UpdateMemberViewModel>(member);
        }

        public async Task<bool> UpdateMemberAsync(int id, UpdateMemberViewModel model, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id, true, ct);

            if (member is null)
                return false;

            var emailExists = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Id != id && m.Email == model.Email, ct);
            var phoneExists = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Id != id && m.PhoneNumber == model.PhoneNumber, ct);

            if (phoneExists || emailExists)
                return false;

            _mapper.Map(model, member);

            member.UpdatedAt = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(model.Photo))
                member.Photo = model.Photo;

            _unitOfWork.GetRepository<Member>().Update(member);

            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? true : false;
        }
    }
}
