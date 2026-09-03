using MediatR;
using Microsoft.EntityFrameworkCore;
using TitanFitenss.Application.Features.Domains.Plans.DTOs;
using TitanFitenss.Infrastructure.Persistence;
namespace TitanFitenss.Application.Features.Domains.Plans.Queries;
public record GetPublishedPlansLookupQuery:IRequest<List<PlanLookupDTO>>;
public class GetPublishedPlansLookupQueryHandler
    :IRequestHandler<GetPublishedPlansLookupQuery, List<PlanLookupDTO>>
{
    private readonly TitanFitnessDbContext _context;
    public GetPublishedPlansLookupQueryHandler(TitanFitnessDbContext context)
    {
        _context=context;
    }
    public async Task<List<PlanLookupDTO>> Handle(
        GetPublishedPlansLookupQuery request, CancellationToken cancellationToken)
    {
        return await _context.Plans
            .AsNoTracking()
            .Where(p=>p.IsPublished)
            .OrderBy(p=>p.Price)
            .Select(p=>new PlanLookupDTO(p.PlanId, p.PlanName, p.Price, p.DurationInMonths))
            .ToListAsync(cancellationToken);
    }
}
