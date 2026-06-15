using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.BookingViewModels;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.SessionViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class BookingServices : IBookingServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingServices(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> CancelBookingAsync(int memberId, int sessionId, CancellationToken ct)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(sessionId);
            if (session == null) return Result.NotFound("Session Not Found");

            if (session.StartDate <= DateTime.Now)
                return Result.Fail("Cannot cancel a booking for a session that has already started.");

            var booking = await _unitOfWork.BookingRepository
                .FirstOrDefaultAsync(b => b.SessionId == sessionId && b.MemberId == memberId, tracking: true, ct: ct);

            if (booking is null) return Result.NotFound("Booking not found.");

            _unitOfWork.BookingRepository.Delete(booking);

            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Booking Cancel Failed");
        }

        public async Task<Result> CreateBookAsync(CreateBookingViewModel model, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(model.SessionId, ct: ct);
            if (session == null)
                return Result.NotFound("Session Not Found.");

            if (session.StartDate <= DateTime.Now)
                return Result.Fail("Cannot book a session that has already started.");

            var hasActiveMembership = await _unitOfWork.MembershipRepository
                .AnyAsync(m => m.MemberId == model.MemberId && m.EndDate > DateOnly.FromDateTime(DateTime.Now), ct);

            if (!hasActiveMembership)
                return Result.Fail("Member does not have an active membership.");

            var alreadyBooked = await _unitOfWork.BookingRepository
                .AnyAsync(b => b.SessionId == model.SessionId && b.MemberId == model.MemberId);

            if (alreadyBooked)
                return Result.Fail("Member is already booked for this session.");
            
            var bookedSlotsCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(model.SessionId, ct);
            if (bookedSlotsCount >= session.Capacity)
                return Result.Fail("No available slots for this session.");

            var creatingbooking = _mapper.Map<Booking>(model);
            creatingbooking.CreatedAt = DateTime.Now;

            _unitOfWork.BookingRepository.Add(creatingbooking);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok():Result.Fail("Failed To Create This Booking");
        }

        public async Task<IEnumerable<SessionViewModel>?> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var sessions = await _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct: ct);

            if (sessions?.Any() != true) return null;

            var MappedSessions = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);

            foreach (var session in MappedSessions)
            {
                session.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
            }

            return MappedSessions;
        }

        public async Task<IEnumerable<MemberViewModel>> GetMembersforDropDownAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
          
            return _mapper.Map<IEnumerable<MemberViewModel>>(members);
        }

        public async Task<IEnumerable<MemberForSessionViewModel>> GetMembersForOngoingBySessionIdAsync(int sessionId, CancellationToken ct = default)
        {
            var bookings = await _unitOfWork.BookingRepository.GetMembersBySessionIdAsync(sessionId, ct); 

            return _mapper.Map<IEnumerable<MemberForSessionViewModel>>(bookings);
        }

        public async Task<IEnumerable<MemberForSessionViewModel>> GetMembersForUpcomingBySessionIdAsync(int sessionId, CancellationToken ct = default)
        {
            var bookings = await _unitOfWork.BookingRepository.GetMembersBySessionIdAsync(sessionId, ct);

            return _mapper.Map<IEnumerable<MemberForSessionViewModel>>(bookings);
        }

        public async Task<Result> MarkAttendedAsync(int memberId, int sessionId, CancellationToken ct = default)
        {
            var booking = await _unitOfWork.BookingRepository.FirstOrDefaultAsync(b => b.MemberId == memberId && b.SessionId == sessionId, tracking: true, ct: ct);
            if (booking is null) return Result.NotFound("Booking not found.");

            booking.IsAttended = true;
            booking.UpdatedAt = DateTime.Now;

            _unitOfWork.BookingRepository.Update(booking);

            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed to Mark As Attended");
        }
    }
}
