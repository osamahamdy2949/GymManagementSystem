using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Controllers
{
    public class PlansController : Controller
    {
        //private readonly GymDbContext context;
        private readonly IPlanRepository planRepository;

        public PlansController(IPlanRepository repository)
        {
            planRepository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var Plans = await planRepository.GetAllPlansAsync();

            if(!Plans.Any() )
                return View();

            return View(Plans);
        }

        public async Task<IActionResult> Details(int id)
        {
            var plan = await planRepository.GetPlanByIdAsync(id);
            if (plan == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
    }
}
