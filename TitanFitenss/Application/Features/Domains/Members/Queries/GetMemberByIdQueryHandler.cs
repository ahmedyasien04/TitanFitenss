using MediatR;
using Microsoft.EntityFrameworkCore;
using TitanFitenss.Application.Common.Exceptions;
using TitanFitenss.Application.Features.Domains.Members.DTOs;
using TitanFitenss.Domain.MemberAggregate;
using TitanFitenss.Infrastructure.Persistence;
namespace TitanFitenss.Application.Features.Domains.Members.Queries;

public record GetMemberByIdQuery(int MemberId):IRequest<MemberDTO>;

public class GetMemberByIdQueryHandler:IRequestHandler<GetMemberByIdQuery, MemberDTO>
{
    private readonly TitanFitnessDbContext _context;

    public GetMemberByIdQueryHandler(TitanFitnessDbContext context)
    {
        _context=context;
    }

    public async Task<MemberDTO> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var member=await _context.Members
            .AsNoTracking()
            .Where(m=>m.MemberId==request.MemberId)
            .Join(_context.Branches, m=>m.HomeBranchId, b=>b.BranchId, (m, b)=>new MemberDTO(
                m.MemberId,
                m.MembershipNumber.Value,
                m.FullName,
                m.Email,
                m.Phone,
                m.Address.City,
                m.Address.Street,
                m.Address.ApartmentNumber,
                m.JoinDate,
                m.HomeBranchId,
                b.BranchName,
                m.Photo
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (member is null)
            throw new NotFoundException(nameof(Member), request.MemberId);

        return member;
    }
}
