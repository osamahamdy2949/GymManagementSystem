using GymManagement.BLL.Services.Interfaces;
using GymManagement.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GymManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAnalyticsServices _analyticsServices;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IAnalyticsServices analyticsServices, ILogger<HomeController> logger)
        {
            _analyticsServices = analyticsServices;
            _logger = logger;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            return View(await _analyticsServices.GetAnalyticsAsync(ct));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
