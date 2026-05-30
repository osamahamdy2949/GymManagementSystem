using GymManagement.BLL.Services.Classes;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
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

            if (result)
                TempData["SuccessMessage"] = "Member Created Succefully";
            else
                TempData["FailedMessage"] = "Failed To Create Member";

            return RedirectToAction(nameof(Index));
        }

        //MemberDetails(int id) - Displays member profile page 
        public IActionResult MemberDetails(int id)
        {
            return View();
        }

        //HealthRecordDetails(int id) - Shows health record page
        public IActionResult HealthRecordDetails(int id)
        {
            return View();
        }

        //MemberEdit(int id) - Displays edit form 
        public IActionResult Edit(int id)
        {
            return View();
        }

        //MemberEdit() - Processes update 
        public IActionResult EditMember()
        {
            return View();
        }

        //Delete(int id) - Shows deletion confirmation page 
        public IActionResult Delete(int id)
        {
            return View();
        }

        //DeleteConfirmed(int id) - Processes deletion
        public IActionResult DeleteConfirmed(int id)
        {
            return View();
        }

    }
}
