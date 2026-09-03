using MediatR;
using Microsoft.EntityFrameworkCore;
using TitanFitenss.Application.Common.Models;
using TitanFitenss.Application.Features.Domains.Plans.DTOs;
using TitanFitenss.Infrastructure.Persistence;
namespace TitanFitenss.Application.Features.Domains.Plans.Queries;
public record GetPlansListQuery(int PageNumber=1, int PageSize=20):IRequest<PaginatedList<PlanDTO>>;

public class GetPlansListQueryHandler:IRequestHandler<GetPlansListQuery, PaginatedList<PlanDTO>>
{
    private readonly TitanFitnessDbContext _context;

    public GetPlansListQueryHandler(TitanFitnessDbContext context)
    {
        _context=context;
    }
    public async Task<PaginatedList<PlanDTO>> Handle(GetPlansListQuery request, CancellationToken cancellationToken)
    {
        var query=_context.Plans
            .AsNoTracking()
            .OrderBy(p=>p.PlanName)
            .Select(p=>new PlanDTO(
                p.PlanId, p.PlanName, p.Price, p.DurationInMonths,
                p.MaxFreezeDays, p.MaxNumberOfFreezes, p.GuestPassQuota,
                p.AccessScope.ToString(), p.IsPublished));

        return await PaginatedList<PlanDTO>.CreateAsync(
            query, request.PageNumber, request.PageSize, cancellationToken);
    }
}
