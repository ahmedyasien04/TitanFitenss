using TitanFitenss.Domain.CheckInAggregate;
namespace TitanFitenss.Domain.Interfaces;
    public interface ICheckInRepository
    {
      Task<CheckIn?> GetByIdAsync(int checkInId, CancellationToken cancellationToken=default);
      Task<IReadOnlyList<CheckIn>> GetCheckInsByMemberIdAsync(int memberId, CancellationToken cancellationToken=default);
      Task<IReadOnlyList<CheckIn>> GetCheckInsByBranchIdAsync(int branchId, DateTime date, CancellationToken cancellationToken=default);
      Task AddAsync(CheckIn checkIn, CancellationToken cancellationToken=default);
    }