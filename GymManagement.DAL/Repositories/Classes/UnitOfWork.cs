using GymManagement.DAL.Data.DbContexts;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _gymDbContext;
        private readonly Dictionary<string , object> _repositories = new Dictionary<string, object>();

        public ISessionRepository SessionRepository { get; }
        public IMembershipRepository MembershipRepository { get; }
        public IBookingRepository BookingRepository { get; }

        public UnitOfWork(GymDbContext gymDbContext , ISessionRepository sessionRepository, 
            IMembershipRepository membershipRepository , IBookingRepository bookingRepository)
        {
            _gymDbContext = gymDbContext;
            SessionRepository = sessionRepository;
            MembershipRepository = membershipRepository;
            BookingRepository = bookingRepository;
            BookingRepository = bookingRepository;
        }

        public IGenericRepository<IEntity> GetRepository<IEntity>() where IEntity : BaseEntity, new()
        {
            //check if the repository for the given entity type already exists in the context
            var typename = typeof(IEntity).Name;

            //if it does, return it
            if(_repositories.TryGetValue(typename, out object? repository))
            {
                return (IGenericRepository<IEntity>)repository;
            }

            //otherwise create a new one and add it to the context and return it
            var newRepository = new GenericRepository<IEntity>(_gymDbContext);
            _repositories[typename] = newRepository;

            return newRepository;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default) => _gymDbContext.SaveChangesAsync(ct);
    }
}
