using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagement.PL.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ISessionServices _sessionService;

        private async Task PopulateDropDownsAsync(CancellationToken ct)
        {
            ViewBag.Trainers = new SelectList(await _sessionService.GetTrainerforDropDownAsync(ct), "Id", "Name");
            ViewBag.Categories = new SelectList(await _sessionService.GetCategoryforDropDownAsync(ct), "Id", "CategoryName");
        }

        public SessionsController(ISessionServices sessionService)
        {
            _sessionService = sessionService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
            => View(await _sessionService.GetAllSessionsAsync(ct));

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropDownsAsync(ct);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownsAsync(ct);
                return View(model);
            }

            var result = await _sessionService.CreateSessionAsync(model, ct);

            if (result.IsSuccess)
                TempData["SuccessMessage"] = "Session Created Successfully";
            else
                TempData["FailedMessage"] = result.errorMessage;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct = default)
        {
            var result = await _sessionService.GetSessionByIdAsync(id, ct);
            
            if(result.IsSuccess)
                return View(result.value);

            else
            {
                TempData["FailedMessage"] = result.errorMessage;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var result = await _sessionService.GetSessionToUpdate(id, ct);

            if(result.IsSuccess)
            {
                ViewBag.Trainers = new SelectList(await _sessionService.GetTrainerforDropDownAsync(), "Id" ,"Name");
                return View(result.value);
            }
            else
            {
                TempData["FailedMessage"] = result.errorMessage;
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Trainers = new SelectList(await _sessionService.GetTrainerforDropDownAsync(), "Id", "Name");
                return View(model);
            }

            var result = await _sessionService.UpdateSessionAsync(id, model, ct);
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Session Updated Successfuly";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["FailedMessage"] = result.errorMessage;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result =await _sessionService.GetSessionByIdAsync(id, ct);
            
            if (result.IsSuccess)
            {
                return View(result.value);
            }
            else
            {
                TempData["FailedMessage"] = result.errorMessage;
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _sessionService.RemoveSessionAsync(id, ct);
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Session Deleted Successfuly";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["FailedMessage"] = result.errorMessage;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
