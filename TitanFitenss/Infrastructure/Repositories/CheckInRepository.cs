using Microsoft.EntityFrameworkCore;
using TitanFitenss.Domain.CheckInAggregate;
using TitanFitenss.Domain.Interfaces;
using TitanFitenss.Infrastructure.Persistence;
namespace TitanFitenss.Infrastructure.Repositories;
    public class CheckInRepository:ICheckInRepository
    {
        private readonly TitanFitnessDbContext _context;
        public CheckInRepository(TitanFitnessDbContext context)
    {
        _context=context;
    }
    public async Task<CheckIn?> GetByIdAsync(int checkInId, CancellationToken cancellationToken = default)
    {
        return await _context.CheckIns.FirstOrDefaultAsync(c=>c.CheckInId==checkInId,cancellationToken);
    }
    public async Task<IReadOnlyList<CheckIn>> GetCheckInsByMemberIdAsync(int memberId, CancellationToken cancellationToken = default)
    {
        return await _context.CheckIns.Where(c=>c.MemberId==memberId).ToListAsync(cancellationToken);
    }
    public async Task<IReadOnlyList<CheckIn>> GetCheckInsByBranchIdAsync(int branchId, DateTime date, CancellationToken cancellationToken = default)
    {
        return await _context.CheckIns
        .Where(c=>c.BranchId==branchId&&c.CheckInDateTime==date.Date).ToListAsync(cancellationToken);
    }
    public async Task AddAsync(CheckIn checkIn, CancellationToken cancellationToken = default)
    {
        await _context.CheckIns.AddAsync(checkIn,cancellationToken);
    }
        
    }