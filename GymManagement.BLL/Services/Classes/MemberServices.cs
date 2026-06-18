using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Services.Attachments;
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
        private readonly IAttachmentServices _attachmentServices;

        public MemberServices(IUnitOfWork unitOfWork , IMapper mapper , IAttachmentServices attachmentServices )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachmentServices = attachmentServices;
        }

        public async Task<Result> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var existEmail = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email, ct);
            var existPhone = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.PhoneNumber == model.PhoneNumber, ct);

            if (existEmail || existPhone) return Result.Validation("Email or phone number already exists");

            var storedPhotoName =  await _attachmentServices.UploadAsync(model.PhotoFile.OpenReadStream() , model.PhotoFile.FileName , "MembersPhoto");
            if(string.IsNullOrWhiteSpace(storedPhotoName.value))
                return Result.Fail("Failed to upload photo");

            var createdMember = _mapper.Map<CreateMemberViewModel, Member>(model);
            createdMember.Photo = storedPhotoName.value;

            _unitOfWork.GetRepository<Member>().Add(createdMember);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            //return result > 0 ? Result.Ok() : Result.Fail("Failed to create member");

            if(result > 0)
            {
                return Result.Ok();
            }
            else
            {
                //Delete Uploaded Photo
                _attachmentServices.Delete(storedPhotoName.value , "MembersPhoto");
                return Result.Fail("Failed to create member");
            }
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

        public async Task<Result> DeleteMemberAsync(int id, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id, false, ct);
            if (member is null)
                return Result.NotFound("Member not found");
            _unitOfWork.GetRepository<Member>().Delete(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed to delete member");
        }

        public async Task<UpdateMemberViewModel?> GetMemberToUpdateAsync(int id, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id, true, ct);

            if (member is null)
                return null;

            return _mapper.Map<Member, UpdateMemberViewModel>(member);
        }

        public async Task<Result> UpdateMemberAsync(int id, UpdateMemberViewModel model, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id, true, ct);

            if (member is null)
                return Result.NotFound("Member not found");

            var emailExists = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Id != id && m.Email == model.Email, ct);
            var phoneExists = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Id != id && m.PhoneNumber == model.PhoneNumber, ct);

            if (phoneExists || emailExists)
                return Result.Validation("Email or phone number already exists");

            _mapper.Map(model, member);

            member.UpdatedAt = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(model.Photo))
                member.Photo = model.Photo;

            _unitOfWork.GetRepository<Member>().Update(member);

            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed to update member");
        }
    }
}
