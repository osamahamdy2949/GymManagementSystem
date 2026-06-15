using AutoMapper;
using GymManagement.BLL.ViewModels.BookingViewModels;
using GymManagement.BLL.ViewModels.MembershipViewModels;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.PlansViewModels;
using GymManagement.BLL.ViewModels.SessionViewModels;
using GymManagement.BLL.ViewModels.TrainerViewModels;
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
        private void MapppingMember()
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

            CreateMap<UpdateMemberViewModel, Member>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .AfterMap((src, dest) =>
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
        private void MappingTrainer()
        {
            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address == null ? "" : $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));

            CreateMap<Trainer, UpdateTrainerViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City));

            CreateMap<UpdateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.Address.Street = src.Street;
                    dest.Address.City = src.City;
                });

            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address()
                {
                    BuildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City
                }));
        }
        private void MappingPlan()
        {
            CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan, PlanDetailsViewModel>();
            CreateMap<Plan, UpdatePlanViewModel>().ReverseMap();

        }
        private void MappingSession()
        {
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.CategoryName , opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.TrainerName , opt => opt.MapFrom(src => src.Trainer.Name));
            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<UpdateSessionViewModel, Session>().ReverseMap();
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category , CategorySelectViewModel>();
        }

        private void MappingMembership()
        {
            CreateMap<Membership, MembershipViewModel>()
                     .ForMember(dist => dist.MemberName, Option => Option.MapFrom(Src => Src.Member.Name))
                     .ForMember(dist => dist.PlanName, Option => Option.MapFrom(Src => Src.Plan.Name))
                     .ForMember(dist => dist.StartDate, Option => Option.MapFrom(src => DateOnly.FromDateTime(src.CreatedAt)));

            CreateMap<CreateMembershipViewModel, Membership>();
            CreateMap<Member, MemberSelectListViewModel>();
            CreateMap<Plan, PlanSelectListViewModel>();
        }
        private void MappingBook()
        {
            CreateMap<CreateBookingViewModel, Booking>();

            CreateMap<Member , MemberSelectListViewModel>();

            CreateMap<Booking, MemberForSessionViewModel>()
                .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name))
                .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.CreatedAt)));
        }
        public MappingProfile()
        {
            MapppingMember();
            MappingPlan();
            MappingTrainer();
            MappingSession();
            MappingMembership();
            MappingBook();
        }
    }
}
