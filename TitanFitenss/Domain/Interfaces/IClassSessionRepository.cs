using TitanFitenss.Domain.ClassSessionAggregate;
namespace TitanFitenss.Domain.Interfaces;
    public interface IClassSessionRepository
    {
      Task<ClassSession?> GetByIdAsync(int sessionId, CancellationToken cancellationToken=default);
      Task<IReadOnlyList<ClassSession>> GetUpcomingSessionsByBranchAsync(int branchId, DateTime startDate, CancellationToken cancellationToken=default);
      Task<IReadOnlyList<ClassSession>> GetSessionsByTrainerAsync(int trainerId, DateTime date, CancellationToken cancellationToken=default);
      Task AddAsync(ClassSession classSession, CancellationToken cancellationToken=default);
      void Update(ClassSession classSession);
        
    }