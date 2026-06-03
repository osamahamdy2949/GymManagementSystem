using Gym.BLL.ViewModels.MemberViewModels;
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

        public MemberServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateMembreAsync(CreateMemberViewModel member, CancellationToken ct = default)
        {
            var existEmail = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == member.Email, ct);
            var existPhone = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.PhoneNumber == member.PhoneNumber, ct);

            if (existEmail || existPhone) return false;

            var createdMember = new Member
            {
                Name = member.Name,
                Email = member.Email,
                PhoneNumber = member.PhoneNumber,
                Gender = member.Gender,
                DateOfBirth = member.DateOfBirth,
                Address = new Address
                {
                    BuildingNumber = member.BuildingNumber,
                    Street = member.Street,
                    City = member.City,
                },
                HealthRecord = new HealthRecord()
                {
                    BloodType = member.HealthRecordViewModel.BloodType,
                    Weight = member.HealthRecordViewModel.Weight,
                    Height = member.HealthRecordViewModel.Height,
                    Note = member.HealthRecordViewModel.Note
                }
            };

            _unitOfWork.GetRepository<Member>().Add(createdMember);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);

            if (!members.Any())
                return Enumerable.Empty<MemberViewModel>();

            return members.Select(m => new MemberViewModel()
            {
                Id = m.Id,
                Photo = m.Photo,
                Name = m.Name,
                Email = m.Email,
                Phone = m.PhoneNumber,
                Gender = m.Gender
            });
        }
        public async Task<MemberDetailsViewModel?> GetMemberByIdAsync(int id, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id, false, ct);

            if (member == null) return null;

            var model =  new MemberDetailsViewModel()
            {
                Photo = member.Photo,
                Name = member.Name,
                Email = member.Email,
                DateOfBirth = member.DateOfBirth,
                Phone = member.PhoneNumber,
                Gender = member.Gender.ToString(),
                Address = member.Address == null ? "" :
                                            $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}"
            };

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

            return new HealthRecordViewModel
            {
                BloodType = healthRecord.BloodType,
                Weight = healthRecord.Weight,
                Height = healthRecord.Height,
                Note = healthRecord.Note
            };
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

            return new UpdateMemberViewModel()
            {
                Id = member.Id,
                Name = member.Name,
                Email = member.Email,
                Phone = member.PhoneNumber,
                Photo = member.Photo,
                BuildingNumber = member.Address.BuildingNumber,
                Street = member.Address.Street,
                City = member.Address.City
            };
        }

        public async Task<bool> UpdateMemberAsync(int id, UpdateMemberViewModel model, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id, true, ct);

            if (member is null)
                return false;

            var emailExists = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Id != id && m.Email == model.Email, ct);
            var phoneExists = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Id != id && m.PhoneNumber == model.Phone, ct);

            if (phoneExists || emailExists)
                return false;

            member.Name = model.Name;
            member.Email = model.Email;
            member.PhoneNumber = model.Phone;
            member.Address = new Address()
            {
                BuildingNumber = model.BuildingNumber,
                Street = model.Street,
                City = model.City
            };

            if (!string.IsNullOrWhiteSpace(model.Photo))
                member.Photo = model.Photo;

            _unitOfWork.GetRepository<Member>().Update(member);

            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? true : false;
        }
    }
}
