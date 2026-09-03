using TitanFitenss.Domain.MemberAggregate;
using TitanFitenss.Domain.ValueObjects;

namespace TitanFitenss.Domain.Interfaces;
    public interface IMemberRepository
    {
        Task<Member?>GetByIdAsync(int memberId, CancellationToken cancellationToken=default);
        Task<Member?>GetByMembershipNumberAsync(MembershipNumber membershipNumber, CancellationToken cancellationToken=default);
        Task<bool>MembershipNumberExistsAsync(MembershipNumber membershipNumber, CancellationToken cancellationToken=default);
        Task<IReadOnlyList<Member>>GetAllAsync(CancellationToken cancellationToken=default);
        Task AddAsync(Member member, CancellationToken cancellationToken=default);
        void Update(Member member);        
    }