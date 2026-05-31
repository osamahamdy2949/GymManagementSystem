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
        private readonly IGenericRepository<Member> _memberRepository;
        private readonly IGenericRepository<Membership> _membershipRepository;
        private readonly IGenericRepository<HealthRecord> _healthRecordRepository;
        public MemberServices(IGenericRepository<Member> memberRepository, IGenericRepository<Membership> membershipRepository
            , IGenericRepository<HealthRecord> healthRecordRepository)
        {
            _memberRepository = memberRepository;
            _membershipRepository = membershipRepository;
            _healthRecordRepository = healthRecordRepository;
        }

        public async Task<bool> CreateMembreAsync(CreateMemberViewModel member, CancellationToken ct = default)
        {
            var existEmail = await _memberRepository.AnyAsync(m => m.Email == member.Email, ct);
            var existPhone = await _memberRepository.AnyAsync(m => m.PhoneNumber == member.PhoneNumber, ct);

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

            var result = await _memberRepository.AddAsync(createdMember, ct);

            return result > 0;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _memberRepository.GetAllAsync(ct: ct);

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
            var member = await _memberRepository.GetByIdAsync(id, false, ct);
            var membership = await _membershipRepository.FirstOrDefaultAsync(m => m.MemberId == id && m.EndDate > DateOnly.FromDateTime(DateTime.Now), false, ct);

            if (member == null) return null;

            return new MemberDetailsViewModel
            {
                Photo = member.Photo,
                PlanName = membership?.Plan.Name,
                Name = member.Name,
                Email = member.Email,
                DateOfBirth = member.DateOfBirth,
                MembershipStartDate = membership?.CreatedAt,
                MembershipEndDate = membership?.EndDate,
                Phone = member.PhoneNumber,
                Gender = member.Gender.ToString(),
                Address = member.Address == null ? "" :
                                            $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}"
            };
        }
        public async Task<HealthRecordViewModel?> GetHealthRecordByMemberIdAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _memberRepository.GetByIdAsync(memberId, false, ct);
            var healthRecord = await _healthRecordRepository.FirstOrDefaultAsync(hr => hr.MemberId == memberId, false, ct);

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
            var member = await _memberRepository.GetByIdAsync(id, false, ct);
            if (member is null)
                return false;

            return await _memberRepository.DeleteAsync(id, ct) > 0;
        }

        public async Task<UpdateMemberViewModel?> GetMemberToUpdateAsync(int id, CancellationToken ct = default)
        {
            var member = await _memberRepository.GetByIdAsync(id, true, ct);

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
            var member = await _memberRepository.GetByIdAsync(id, true, ct);

            if (member is null)
                return false;

            var emailExists = await _memberRepository.AnyAsync(m => m.Id != id && m.Email == model.Email, ct);

            if (emailExists)
                return false;

            var phoneExists = await _memberRepository.AnyAsync(m => m.Id != id && m.PhoneNumber == model.Phone, ct);

            if (phoneExists)
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

            var result = await _memberRepository.UpdateAsync(member, ct);

            return result > 0 ? true : false;
        }
    }
}
