using GymManagement.BLL.Services.Classes;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.BookingViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagement.PL.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly IBookingServices _bookingServices;

        private async Task PopulateDropDownsAsync(CancellationToken ct)
        {
            ViewBag.Members = new SelectList(await _bookingServices.GetMembersforDropDownAsync(ct), "Id", "Name");
        }

        public BookingsController(IBookingServices bookingServices)
        {
            _bookingServices = bookingServices;
        }

        public async Task<IActionResult> Index(CancellationToken ct = default)
            => View(await _bookingServices.GetAllSessionsAsync(ct));

        [HttpGet]
        public async Task<IActionResult> GetMembersForUpcomingSession(int id, CancellationToken ct)
            => View(await _bookingServices.GetMembersForUpcomingBySessionIdAsync(id, ct));

        [HttpGet]
        public async Task<IActionResult> GetMembersForOngoingSessions(int id, CancellationToken ct)
            => View(await _bookingServices.GetMembersForOngoingBySessionIdAsync(id, ct));

        [HttpGet]
        public async Task<IActionResult> Create(int id, CancellationToken ct)
        {
            var members = await _bookingServices.GetMembersforDropDownAsync(ct);
            ViewBag.Members = new SelectList(members, "Id", "Name");
            ViewBag.SessionId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int id, CreateBookingViewModel model, CancellationToken ct)
        {
            model.SessionId = id;

            if (!ModelState.IsValid)
            {
                await PopulateDropDownsAsync(ct);
                return View(model);
            }

            var result = await _bookingServices.CreateBookAsync(model, ct);

            if (result.IsSuccess)
                TempData["SuccessMessage"] = "Booking Created Successfully";
            else
                TempData["FailedMessage"] = result.errorMessage;

            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = model.SessionId });
        }

        [HttpPost]
        public async Task<IActionResult> Attended(int memberId, int sessionId, CancellationToken ct)
        {
            var result = await _bookingServices.MarkAttendedAsync(memberId, sessionId, ct);

            if (result.IsSuccess)
                TempData["SuccessMessage"] = "Marked As Attended Successfully";
            else
                TempData["FailedMessage"] = result.errorMessage;

            return RedirectToAction(nameof(GetMembersForOngoingSessions), new { id = sessionId });
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int memberId, int sessionId, CancellationToken ct)
        {
            var result = await _bookingServices.CancelBookingAsync(memberId, sessionId, ct);

            if (result.IsSuccess)
                TempData["SuccessMessage"] = "Booking Cancelled Successfully";
            else
                TempData["FailedMessage"] = result.errorMessage;

            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = sessionId });
        }
    }
}
