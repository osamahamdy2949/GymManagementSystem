using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DaL.Models;
using GymManagement.DbContexts;

namespace GymManagement.Repositories.Classes
{
    public class PlanRepository : IPlanRepository
    {
        private readonly GymDbContext _context;

        public PlanRepository(GymDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<int> AddPlanAsync(Plan plan, CancellationToken ct = default)
        {
            _context.Add(plan);
            return await _context.SaveChangesAsync(ct);
        }

        public async Task<int> DeletePlanAsync(int id, CancellationToken ct = default)
        {
            var plan = await _context.Plans.FindAsync(id);
            if (plan == null)
            {
                return 0;
            }
            _context.Plans.Remove(plan);
            return await _context.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<Plan>> GetAllPlansAsync(bool tracking = false, CancellationToken ct = default)
        {
            //if(tracking)
            //{
            //    return await _context.Plans.ToListAsync(ct);
            //}
            //else
            //{
            //    return await _context.Plans.AsNoTracking().ToListAsync(ct);
            //}

            IQueryable<Plan> query = tracking ? _context.Plans : _context.Plans.AsNoTracking();
            return await query.ToListAsync(ct);
        }

        public async Task<Plan?> GetPlanByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Plans.FindAsync(id , ct); 
        }

        public Task<int> UpdatePlanAsync(Plan plan, CancellationToken ct = default)
        {
            var existingPlan = _context.Plans.Find(plan.Id);
            if (existingPlan != null)
            {
                existingPlan.Name = plan.Name;
                existingPlan.Description = plan.Description;
                existingPlan.Price = plan.Price;
                existingPlan.DurationDays = plan.DurationDays;
                existingPlan.IsActive = plan.IsActive;
                existingPlan.UpdatedAt = DateTime.UtcNow;
                return _context.SaveChangesAsync(ct);
            }
            else
            {
                throw new KeyNotFoundException($"Plan with ID {plan.Id} not found.");
            }
        }
    }
}
