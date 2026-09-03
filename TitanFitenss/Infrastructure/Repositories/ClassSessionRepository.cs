using Microsoft.EntityFrameworkCore;
using TitanFitenss.Domain.ClassSessionAggregate;
using TitanFitenss.Domain.Interfaces;
using TitanFitenss.Infrastructure.Persistence;
namespace TitanFitenss.Infrastructure.Repositories;
    public class ClassSessionRepository:IClassSessionRepository
    {
        private readonly TitanFitnessDbContext _context;
        public ClassSessionRepository(TitanFitnessDbContext context)
    {
        _context=context;
    }
    public async Task<ClassSession?> GetByIdAsync(int sessionId, CancellationToken cancellationToken = default)
    {
        return await _context.ClassSessions.FirstOrDefaultAsync(c=>c.SessionId==sessionId,cancellationToken);
    }
    public async Task<IReadOnlyList<ClassSession>> GetUpcomingSessionsByBranchAsync(int branchId, DateTime startDate, CancellationToken cancellationToken = default)
    {
         return await _context.ClassSessions
         .Where(c=>c.BranchId==branchId &&c.SessionDate>=startDate.Date).ToListAsync(cancellationToken);
    }
    public async Task<IReadOnlyList<ClassSession>> GetSessionsByTrainerAsync(int trainerId, DateTime date, CancellationToken cancellationToken = default)
    {
        return await _context.ClassSessions
         .Where(c=>c.TrainerId==trainerId &&c.SessionDate.Date==date.Date).ToListAsync(cancellationToken);
    }
    public async Task AddAsync(ClassSession classSession, CancellationToken cancellationToken = default)
    {
        await _context.ClassSessions.AddAsync(classSession,cancellationToken);
    }
    public async void Update(ClassSession classSession)=>_context.ClassSessions.Update(classSession);
    }