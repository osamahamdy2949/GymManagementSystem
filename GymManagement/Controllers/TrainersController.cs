using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class TrainersController : Controller
    {
        private readonly ITrainerServices _trainerServices;

        public TrainersController(ITrainerServices trainerServices)
        {
            _trainerServices = trainerServices;
        }

        // GET: /Trainers
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var trainers = await _trainerServices.GetAllTrainersAsync(ct);
            return View(trainers);
        }

        // GET: /Trainers/Create
        public IActionResult Create() => View();

        // POST: /Trainers/Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(nameof(Create), model);

            var result = await _trainerServices.CreateTrainerAsync(model, ct);

            if (result.IsSuccess)
                TempData["SuccessMessage"] = "Trainer Created Successfully";
            else
                TempData["FailedMessage"] = result.errorMessage;

            return RedirectToAction(nameof(Index));
        }

        // GET: /Trainers/Details/5
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var trainer = await _trainerServices.GetTrainerByIdAsync(id, ct);

            if (trainer is null)
            {
                TempData["FailedMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }

        // GET: /Trainers/Edit/5
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var trainerToUpdate = await _trainerServices.GetTrainerToUpdateAsync(id, ct);

            if (trainerToUpdate is null)
            {
                TempData["FailedMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(trainerToUpdate);
        }

        // POST: /Trainers/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateTrainerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(nameof(Edit), model);

            var result = await _trainerServices.UpdateTrainerAsync(model.Id, model, ct);

            if (result.IsSuccess)
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
            else
                TempData["FailedMessage"] = result.errorMessage;

            return RedirectToAction(nameof(Index));
        }

        // GET: /Trainers/Delete/5
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var trainer = await _trainerServices.GetTrainerByIdAsync(id, ct);

            if (trainer is null)
            {
                TempData["FailedMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }

        // POST: /Trainers/DeleteConfirmed/5
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _trainerServices.DeleteTrainerAsync(id, ct);

            if (result.IsSuccess)
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            else
                TempData["FailedMessage"] = result.errorMessage;

            return RedirectToAction(nameof(Index));
        }
    }
}
