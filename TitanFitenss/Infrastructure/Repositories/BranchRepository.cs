
using Microsoft.EntityFrameworkCore;
using TitanFitenss.Domain.BranchAggregate;
using TitanFitenss.Domain.Interfaces;
using TitanFitenss.Infrastructure.Persistence;
namespace TitanFitenss.Infrastructure.Repositories;
    public class BranchRepository:IBranchRepository
    {
        private readonly TitanFitnessDbContext _context;
        public BranchRepository(TitanFitnessDbContext context)
        {
            _context=context;
        }
        public async Task<Branch?> GetByIdAsync(int branchId, CancellationToken cancellationToken=default)
    {
        return await _context.Branches
        .FirstOrDefaultAsync(b=>b.BranchId==branchId, cancellationToken);
    }
    public async Task<IReadOnlyList<Branch>>GetAllAsync(CancellationToken cancellationToken=default)
    {
        return await _context.Branches.ToListAsync(cancellationToken);
    }
      public async Task AddAsync(Branch branch, CancellationToken cancellationToken=default)
    {
        await _context.Branches.AddAsync(branch, cancellationToken);
    }
    public async void Update(Branch branch)=>_context.Branches.Update(branch);
    public async void Delete(Branch branch)=>_context.Branches.Remove(branch);
    }
