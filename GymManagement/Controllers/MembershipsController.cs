using GymManagement.BLL.Services.Classes;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MembershipViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagement.PL.Controllers
{
    [Authorize]
    public class MembershipsController : Controller
    {
        private readonly IMembershipServices _membershipServices;

        private async Task PopulateDropdownsAsync(CancellationToken ct)
        {
            ViewBag.Plans = new SelectList(await _membershipServices.GetPlansForDropDownAsync(ct), "Id", "Name");
            ViewBag.Members = new SelectList(await _membershipServices.GetMembersForDropDownAsync(ct), "Id", "Name");
        }

        public MembershipsController(IMembershipServices membershipServices)
        {
            _membershipServices = membershipServices;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
            => View(await _membershipServices.GetAllMembershipsAsync(ct: ct));

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropdownsAsync(ct);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMembershipViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(ct);
                return View(model);
            }

            var result = await _membershipServices.CreateMembershipAsync(model, ct);
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Membership created successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["FailedMessage"] = result.errorMessage;
            await PopulateDropdownsAsync(ct);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id, CancellationToken ct)
        {
            var result = await _membershipServices.DeleteActiveMembershipAsync(id, ct);

            if (result.IsSuccess)
                TempData["SuccessMessage"] = "Membership cancelled successfully.";
            else
                TempData["FailedMessage"] = result.errorMessage;

            return RedirectToAction(nameof(Index));
        }
    }
}
