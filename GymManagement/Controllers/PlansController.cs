using GymManagement.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Controllers
{
    public class PlansController : Controller
    {
        private readonly GymDbContext context;

        public PlansController()
        {
            context = new GymDbContext();
        }

        public async Task<IActionResult> Index()
        {
            var Plans = await context.Plans.ToListAsync();

            if(!Plans.Any() )
                return View();

            return View(Plans);
        }

        public async Task<IActionResult> Details(int id)
        {
            var plan = await context.Plans.FindAsync(id);
            if (plan == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
    }
}
