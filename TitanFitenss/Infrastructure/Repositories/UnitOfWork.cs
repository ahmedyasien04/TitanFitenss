using TitanFitenss.Domain.Interfaces;
using TitanFitenss.Infrastructure.Persistence;
namespace TitanFitenss.Infrastructure.Repositories
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly TitanFitnessDbContext _context;
        public UnitOfWork(TitanFitnessDbContext context)
        {
            _context=context;
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken=default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}