using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.SessionViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Data.Models.Enums;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GymManagement.BLL.Services.Classes.SessionServices;

namespace GymManagement.BLL.Services.Classes
{
    public class SessionServices : ISessionServices
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionServices(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate) return Result.Validation("EndDate Must Be After StartDate");
            if (model.StartDate <= DateTime.Now) return Result.Validation("StartDate Must Be After Current Date");
            if (model.Capacity < 1 || model.Capacity > 25) return Result.Validation("Capacity Must Be Between 1 And 25");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);
            if (trainer == null) return Result.NotFound("Trainer Not Found");

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId);
            if (category == null) return Result.NotFound("Category Not Found");

            var isValid = Enum.TryParse<Speciality>(category.CategoryName,true, out var speciality);
            if (!isValid || trainer.Speciality != speciality) return Result.Validation("Trainer Speciality Does Not Match Category");

            var createdSession = _mapper.Map<Session>(model);

            _unitOfWork.GetRepository<Session>().Add(createdSession);

            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed To Create Session");
        }

        public async Task<Result> RemoveSessionAsync(int id, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(id, ct: ct);

            if (session == null) return Result.NotFound("Session Not Found");

            if (session.EndDate >= DateTime.Now) return Result.Fail("Can't Delete Session That Has Not Ended Yet");

            var bookingCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(id, ct);

            if (bookingCount > 0) return Result.Fail("Can't Remove Session That Has Bookings");

            _unitOfWork.SessionRepository.Delete(session);

            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed To Delete Session");
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

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoryforDropDownAsync(CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetRepository<Category>().GetAllAsync(false, ct);
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(result);
        }

        public async Task<Result<SessionViewModel>> GetSessionByIdAsync(int id , CancellationToken ct)
        {
            var session = await _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategoryByIdAsync(id, ct);

            if (session == null) return Result<SessionViewModel>.NotFound("Session Not Found");

            else
            {
                var mappedSession = _mapper.Map<SessionViewModel>(session);
                var bookedSlotsCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
                mappedSession.AvailableSlots = mappedSession.Capacity - bookedSlotsCount;
                return Result<SessionViewModel>.Ok(mappedSession);
            }

        }
        
        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainerforDropDownAsync(CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(false, ct);
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(result);
        }
        
        public async Task<Result<UpdateSessionViewModel>> GetSessionToUpdate(int id, CancellationToken ct = default)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(id);

            if(session == null) return Result<UpdateSessionViewModel>.NotFound("Session Not Found");

            if (session.StartDate <= DateTime.Now)
                return Result<UpdateSessionViewModel>.Fail("Can't Update Session That Has Already Started");

            var bookingCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(id, ct);
            if(bookingCount > 0)
                return Result<UpdateSessionViewModel>.Fail("Can't Update Session That Already Has Booking");

            var model = _mapper.Map<UpdateSessionViewModel>(session);

            return Result<UpdateSessionViewModel>.Ok(model);
        }
        
        public async Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(id);

            if (session == null) return Result.NotFound("Session Not Found");

            if (model.EndDate <= model.StartDate) return Result.Validation("EndDate Must Be After StartDate");
            if (model.StartDate <= DateTime.Now) return Result.Validation("Can't Update Session That Has Already Started");


            var bookingCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(id, ct);
            if (bookingCount > 0)
                return Result.Fail("Can't Update Session That Already Has Booking");

            if (model.StartDate <= DateTime.Now) return Result.Validation("StartDate Must Be in The Futuer");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);
            if (trainer == null) return Result.NotFound("Trainer Not Found");

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(session.CategoryId);
            if (category == null) return Result.NotFound("Category Not Found");

            var isValid = Enum.TryParse<Speciality>(category.CategoryName,true, out var speciality);
            if (!isValid || trainer.Speciality != speciality) return Result.Validation("Trainer Speciality Does Not Match Category");

            _mapper.Map(model, session);

            _unitOfWork.GetRepository<Session>().Update(session);

            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed To Update Session");
        }
    }
}
