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

        public TrainerServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(false, ct);

            return trainers.Select(t => new TrainerViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.PhoneNumber,
                DateOfBirth = t.DateOfBirth,
                Speciality = t.Speciality,
                Address = $"{t.Address.BuildingNumber} - {t.Address.Street} - {t.Address.City}"
            });
        }
        public async Task<TrainerViewModel?> GetTrainerByIdAsync(int id, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, false, ct);

            if (trainer is null)
                return null;

            return new TrainerViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.PhoneNumber,
                DateOfBirth = trainer.DateOfBirth,
                Speciality = trainer.Speciality,
                Address = $"{trainer.Address.BuildingNumber} - {trainer.Address.Street} - {trainer.Address.City}"
            };
        }
        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel model,CancellationToken ct = default)
        {
            var emailExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.Email == model.Email,ct);
            if (emailExists)
                return false;
            
            var phoneExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.PhoneNumber == model.Phone, ct);
            if (phoneExists)
                return false;

            var createdTrainer = new Trainer
            {
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.Phone,
                DateOfBirth = model.DateOfBirth,
                Speciality = model.Speciality,
                Gender = model.Gender,
                Address = new Address
                {
                    BuildingNumber = model.BuildingNumber,
                    Street = model.Street,
                    City = model.City,
                }
            };

            _unitOfWork.GetRepository<Trainer>().Add(createdTrainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? true : false;
        }
        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int id,CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id,true, ct: ct);
            if (trainer is null)
                return null;

            return new TrainerToUpdateViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.PhoneNumber,
                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.City,
                Speciality = trainer.Speciality
            };
        }
        public async Task<bool> UpdateTrainerAsync(int id,TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, true, ct);

            if (trainer is null)
                return false;

            var emailExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.Id != id && t.Email == model.Email, ct);

            if (emailExists)
                return false;

            var phoneExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.Id != id && t.PhoneNumber == model.Phone, ct);

            if (phoneExists)
                return false;

            trainer.Name = model.Name;
            trainer.Email = model.Email;
            trainer.PhoneNumber = model.Phone;
            trainer.Speciality  = model.Speciality;
            trainer.UpdatedAt = DateTime.Now;
            trainer.Address = new Address()
            {
                BuildingNumber = model.BuildingNumber,
                Street = model.Street,
                City = model.City
            };

            _unitOfWork.GetRepository<Trainer>().Update(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? true : false;
        }
        public async Task<bool> DeleteTrainerAsync(int id,CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct:ct);

            if (trainer is null)
                return false;

            _unitOfWork.GetRepository<Trainer>().Delete(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? true : false;
        }
    }
}
