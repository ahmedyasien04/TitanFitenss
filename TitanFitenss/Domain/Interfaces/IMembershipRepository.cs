using TitanFitenss.Domain.MembershipAggregate;
namespace TitanFitenss.Domain.Interfaces;
    public interface IMembershipRepository
    {
      Task<Membership?> GetByIdAsync(int membershipId, CancellationToken cancellationToken=default);
      Task<Membership?> GetActiveMembershipByMemberIdAsync(int memberId, CancellationToken cancellationToken=default);
      Task<IReadOnlyList<Membership>> GetMembershipsByMemberIdAsync(int memberId, CancellationToken cancellationToken=default);
      Task AddAsync(Membership membership, CancellationToken cancellationToken=default);
      void Update(Membership membership);
    }