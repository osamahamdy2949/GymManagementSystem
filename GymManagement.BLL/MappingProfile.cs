using AutoMapper;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Member, MemberViewModel>();

            CreateMap<Member, MemberDetailsViewModel>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address == null ? "" : $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));

            CreateMap<HealthRecord, HealthRecordViewModel>().ReverseMap();

            CreateMap<Member, UpdateMemberViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City));
                
            CreateMap<UpdateMemberViewModel , Member>()
                .ForMember(dest => dest.Name , opt => opt.Ignore())
                .ForMember(dest => dest.Photo , opt => opt.Ignore())
                .AfterMap((src,dest) =>
                {
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.Address.Street = src.Street;
                    dest.Address.City = src.City;
                });

            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address()
                {
                    BuildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City
                }))
                .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));        
        }
    }
}
