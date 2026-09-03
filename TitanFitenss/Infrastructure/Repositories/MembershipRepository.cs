using Microsoft.EntityFrameworkCore;
using TitanFitenss.Domain.MembershipAggregate;
using TitanFitenss.Domain.Interfaces;
using TitanFitenss.Infrastructure.Persistence;
namespace TitanFitenss.Infrastructure.Repositories;
    public class MembershipRepository:IMembershipRepository
    {
        private readonly TitanFitnessDbContext _context;
        public MembershipRepository(TitanFitnessDbContext context)
        {
            _context=context;
        }
    public async Task<Membership?> GetByIdAsync(int membershipId, CancellationToken cancellationToken=default)
    {
        return await _context.Memberships.FirstOrDefaultAsync(m=>m.MembershipId==membershipId,cancellationToken);
    }
    public async Task<Membership?> GetActiveMembershipByMemberIdAsync(int memberId, CancellationToken cancellationToken=default)
    {
        return await _context.Memberships
        .FirstOrDefaultAsync(m=>m.MemberId==memberId && m.Status==MembershipStatus.Active,cancellationToken);
    }
    public async Task<IReadOnlyList<Membership>> GetMembershipsByMemberIdAsync(int memberId, CancellationToken cancellationToken=default)
    {
        return await _context.Memberships.Where(m=>m.MemberId==memberId).ToListAsync(cancellationToken);
    }
    public async Task AddAsync(Membership membership, CancellationToken cancellationToken=default)
    {
        await _context.Memberships.AddAsync(membership,cancellationToken);
    }
    public async void Update(Membership membership)=>_context.Memberships.Update(membership);
    }