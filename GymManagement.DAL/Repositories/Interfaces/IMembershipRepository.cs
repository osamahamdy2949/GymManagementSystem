using GymManagement.DAL.Data.Models;
using System.Linq.Expressions;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IMembershipRepository : IGenericRepository<Membership>
    {
        public Task<IEnumerable<Membership>> GetAllMembershipsWithMembersAndPlans(
            Expression<Func<Membership, bool>>? predicate = null, CancellationToken ct = default);
    }
}
