using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.PlansViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Controllers
{
    [Authorize]
    public class PlansController : Controller
    {
        //private readonly GymDbContext context;
        private readonly IPlanServices _planServices;
        public PlansController(IPlanServices planServices)
        {
            _planServices = planServices;
        }
        public async Task<IActionResult> Index()
        {
            var Plans = await _planServices.GetAllPlansAsync();

            if (!Plans.Any())
                return View();

            return View(Plans);
        }

        public async Task<IActionResult> Details(int id)
        {
            var plan = await _planServices.GetPlanByIdAsync(id);

            return View(plan);
        }

        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var plan = await _planServices.GetPlanToUpdateAsync(id, ct);

            if (plan is null)
            {
                TempData["FailedMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }
        [HttpPost]
        public async Task<IActionResult> EditPlan(UpdatePlanViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Edit), model);
            }

            var result = await _planServices.UpdatePlanAsync(model.Id, model, ct);

            if (result.IsSuccess)
                TempData["SuccessMessage"] = "Plan Updated Successfully";
            else
                TempData["FailedMessage"] = result.errorMessage;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, CancellationToken ct)
        {
            var result = await _planServices.UpdateStatusAsync(id, ct);
            if (result.IsSuccess)
                TempData["SuccessMessage"] = "Plan Status Updated Successfully";
            else
                TempData["FailedMessage"] = result.errorMessage;

            return RedirectToAction(nameof(Index));
        }
    }
}
