using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Controllers
{
    public class PlansController : Controller
    {
        //private readonly GymDbContext context;
        private readonly IGenericRepository<Plan> _planRepository;

        public PlansController(IGenericRepository<Plan> repository)
        {
            _planRepository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var Plans = await _planRepository.GetAllAsync();

            if(!Plans.Any() )
                return View();

            return View(Plans);
        }

        public async Task<IActionResult> Details(int id)
        {
            var plan = await _planRepository.GetByIdAsync(id);
            if (plan == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
    }
}
