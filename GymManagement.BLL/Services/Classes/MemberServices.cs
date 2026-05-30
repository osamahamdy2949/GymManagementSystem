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
        public MemberServices(IGenericRepository<Member> memberRepository)
        {
             _memberRepository = memberRepository;
        }
        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var existingEmail = await _memberRepository.AnyAsync(m => m.Email == model.Email, ct);
            var existingPhone = await _memberRepository.AnyAsync(m => m.PhoneNumber == model.PhoneNumber, ct);

            if (existingEmail || existingPhone)
                return false;

            var createdMember = new Member
            {
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Address = new Address
                {
                    BuildingNumber = model.BuildingNumber,
                    Street = model.Street,
                    City = model.City,
                },
                HealthRecord = new HealthRecord()
                {
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Weight = model.HealthRecordViewModel.Weight,
                    Height = model.HealthRecordViewModel.Height,
                    Note = model.HealthRecordViewModel.Note
                }
            };

            var result = await _memberRepository.AddAsync(createdMember, ct);

            return result > 0;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _memberRepository.GetAllAsync(ct : ct);
            
            if (!members.Any())
                return Enumerable.Empty<MemberViewModel>();
            

            return members.Select(m => new MemberViewModel
            {
                Id = m.Id,
                Photo = m.Photo,
                Name = m.Name,
                Email = m.Email,
                Phone = m.PhoneNumber,
                Gender = m.Gender

            });            
        }
    }
}
