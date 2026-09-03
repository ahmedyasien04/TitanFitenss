
using Microsoft.EntityFrameworkCore;
using TitanFitenss.Infrastructure.Persistence;
using TitanFitenss.Domain.PlanAggregate;
using TitanFitenss.Domain.Interfaces;

namespace TitanFitenss.Infrastructure.Repositories;
    public class PlanRepository:IPlanRepository
    {
        private readonly TitanFitnessDbContext _context;
        public PlanRepository(TitanFitnessDbContext context)
    {
        _context=context;
    }
    public async Task<Plan?> GetByIdAsync(int planId, CancellationToken cancellationToken=default)
    {
        return await _context.Plans.FirstOrDefaultAsync(p=>p.PlanId==planId,cancellationToken);
    }
    public async Task<IReadOnlyList<Plan>> GetAllAsync(CancellationToken cancellationToken=default)
    {
        return await _context.Plans.ToListAsync(cancellationToken);
    }
    public async Task<IReadOnlyList<Plan>> GetPublishedPlansAsync(CancellationToken cancellationToken=default)
    {
        return await _context.Plans.Where(p=>p.IsPublished).ToListAsync(cancellationToken);
    }
    public async Task AddAsync(Plan plan, CancellationToken cancellationToken = default)
    {
        await _context.Plans.AddAsync(plan,cancellationToken);
    }
    public async void Update(Plan plan)=>_context.Plans.Update(plan);
        
    }