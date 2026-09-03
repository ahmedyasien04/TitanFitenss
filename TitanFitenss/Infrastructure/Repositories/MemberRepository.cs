using Microsoft.EntityFrameworkCore;
using TitanFitenss.Domain.Interfaces;
using TitanFitenss.Domain.MemberAggregate;
using TitanFitenss.Domain.ValueObjects;
using TitanFitenss.Infrastructure.Persistence;

namespace TitanFitenss.Infrastructure.Repositories;
    public class MemberRepository:IMemberRepository
    {
        private readonly TitanFitnessDbContext _context;
        public MemberRepository(TitanFitnessDbContext context)
    {
        _context=context;
    }
    public async Task<Member?>GetByIdAsync(int memberId, CancellationToken cancellationToken=default)
    {
        return await _context.Members.FirstOrDefaultAsync(m=>m.MemberId==memberId,cancellationToken);
    }
    public async Task<Member?>GetByMembershipNumberAsync(MembershipNumber membershipNumber,CancellationToken cancellationToken = default)
    {
        return await _context.Members.FirstOrDefaultAsync(m=>m.MembershipNumber==membershipNumber,cancellationToken);
    }
        public async Task<bool>MembershipNumberExistsAsync(MembershipNumber membershipNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Members.AnyAsync(m=>m.MembershipNumber==membershipNumber,cancellationToken);
    }
        public async Task<IReadOnlyList<Member>>GetAllAsync(CancellationToken cancellationToken=default)
    {
        return await _context.Members.ToListAsync(cancellationToken);
    }
        public async Task AddAsync(Member member, CancellationToken cancellationToken=default)
    {
        await _context.Members.AddAsync(member,cancellationToken);
    }
        public async void Update(Member member)=>_context.Members.Update(member);
        
    }