using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.BLL.Sevices.Classes
{
    public class TrainerServices : ITrainerServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerServices(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);

            return _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);
        }
        public async Task<TrainerViewModel?> GetTrainerByIdAsync(int id, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, false, ct);

            if (trainer is null)
                return null;

            return _mapper.Map<TrainerViewModel>(trainer);
        }
        public async Task<Result> CreateTrainerAsync(CreateTrainerViewModel model,CancellationToken ct = default)
        {
            var emailExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.Email == model.Email,ct);
            if (emailExists)
                return Result.Validation("Email already exists");
            
            var phoneExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.PhoneNumber == model.PhoneNumber, ct);
            if (phoneExists)
                return Result.Validation("Phone number already exists");
            var createdTrainer = _mapper.Map<Trainer>(model);

            _unitOfWork.GetRepository<Trainer>().Add(createdTrainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed to create trainer");
        }
        public async Task<UpdateTrainerViewModel?> GetTrainerToUpdateAsync(int id,CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id,true, ct: ct);
            if (trainer is null)
                return null;

            return _mapper.Map<UpdateTrainerViewModel>(trainer);
        }
        public async Task<Result> UpdateTrainerAsync(int id,UpdateTrainerViewModel model, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, true, ct);

            if (trainer is null)
                return Result.NotFound("Trainer not found");

            var emailExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.Id != id && t.Email == model.Email, ct);

            if (emailExists)
                return Result.Validation("Email already exists");

            var phoneExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.Id != id && t.PhoneNumber == model.PhoneNumber, ct);

            if (phoneExists)
                return Result.Validation("Phone number already exists");

           _mapper.Map(model, trainer);

            _unitOfWork.GetRepository<Trainer>().Update(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed to update trainer");
        }
        public async Task<Result> DeleteTrainerAsync(int id,CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct:ct);

            if (trainer is null)
                return Result.NotFound("Trainer not found");

            _unitOfWork.GetRepository<Trainer>().Delete(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed to delete trainer");
        }
    }
}
