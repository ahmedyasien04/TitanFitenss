using MediatR;
using Microsoft.EntityFrameworkCore;
using TitanFitenss.Application.Common.Exceptions;
using TitanFitenss.Application.Features.Domains.Plans.DTOs;
using TitanFitenss.Domain.PlanAggregate;
using TitanFitenss.Infrastructure.Persistence;
namespace TitanFitenss.Application.Features.Domains.Plans.Queries;
public record GetPlanByIdQuery(int PlanId):IRequest<PlanDTO>;

public class GetPlanByIdQueryHandler:IRequestHandler<GetPlanByIdQuery, PlanDTO>
{
    private readonly TitanFitnessDbContext _context;

    public GetPlanByIdQueryHandler(TitanFitnessDbContext context)
    {
        _context=context;
    }
    public async Task<PlanDTO> Handle(GetPlanByIdQuery request, CancellationToken cancellationToken)
    {
        var plan=await _context.Plans
            .AsNoTracking()
            .Where(p => p.PlanId==request.PlanId)
            .Select(p=>new PlanDTO(
                p.PlanId, p.PlanName, p.Price, p.DurationInMonths,
                p.MaxFreezeDays, p.MaxNumberOfFreezes, p.GuestPassQuota,
                p.AccessScope.ToString(), p.IsPublished))
            .FirstOrDefaultAsync(cancellationToken);

        if (plan is null)
            throw new NotFoundException(nameof(Plan), request.PlanId);

        return plan;
    }
}
