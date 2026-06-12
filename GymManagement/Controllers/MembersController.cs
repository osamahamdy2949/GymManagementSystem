using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.Services.Classes;
using GymManagement.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberServices _memberService;

        public MembersController(IMemberServices memberService)
        {
            _memberService = memberService;
        }

        //Index() - Displays member listing page
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await _memberService.GetAllMembersAsync(ct);

            return View(members);
        }


        //Create() - Shows member registration form 
        public IActionResult Create() => View();

        //CreateMember() - Processes form submission 
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Create), model);
            }

            var result = await _memberService.CreateMemberAsync(model, ct);

            if (result.IsSuccess)
                TempData["SuccessMessage"] = "Member Created Successfully";
            else
                TempData["FailedMessage"] = result.errorMessage;

            return RedirectToAction(nameof(Index));
        }

        //MemberDetails(int id) - Displays member profile page 
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberByIdAsync(id, ct: ct);

            return View(member);
        }

        //HealthRecordDetails(int id) - Shows health record page
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var healthRecord = await _memberService.GetHealthRecordByMemberIdAsync(id, ct);

            return View(healthRecord);
        }

        //MemberEdit(int id) - Displays edit form 
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberToUpdateAsync(id, ct);
            if (member is null)
            {
                TempData["FailedMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        //MemberEdit() - Processes update 
        [HttpPost]
        public async Task<IActionResult> EditMember(UpdateMemberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Edit), model);
            }

            var result = await _memberService.UpdateMemberAsync(model.Id, model, ct);

            if (result.IsSuccess)
                TempData["SuccessMessage"] = "Member Updated Successfully";
            else
                TempData["FailedMessage"] = result.errorMessage;

            return RedirectToAction(nameof(Index));
        }

        //Delete(int id) - Shows deletion confirmation page 
        public async Task<IActionResult> DeleteMember(int id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberByIdAsync(id, ct);
            if (member is null)
            {
                TempData["FailedMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        //DeleteConfirmed(int id) - Processes deletion
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _memberService.DeleteMemberAsync(id, ct);

            if (result.IsSuccess)
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            else
                TempData["FailedMessage"] = result.errorMessage;

            return RedirectToAction(nameof(Index));
        }

    }
}
