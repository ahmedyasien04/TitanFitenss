using MediatR;
using Microsoft.EntityFrameworkCore;
using TitanFitenss.Application.Features.Domains.Trainers.DTOs;
using TitanFitenss.Infrastructure.Persistence;
namespace TitanFitenss.Application.Features.Domains.Trainers.Queries;
public record GetActiveTrainersLookupQuery:IRequest<List<TrainerLookupDTO>>;
public class GetActiveTrainersLookupQueryHandler
    :IRequestHandler<GetActiveTrainersLookupQuery, List<TrainerLookupDTO>>
{
    private readonly TitanFitnessDbContext _context;

    public GetActiveTrainersLookupQueryHandler(TitanFitnessDbContext context)
    {
        _context=context;
    }
    public async Task<List<TrainerLookupDTO>> Handle(
        GetActiveTrainersLookupQuery request, CancellationToken cancellationToken)
    {
        return await _context.Trainers
            .AsNoTracking()
            .Where(t=>t.IsActive)
            .OrderBy(t=>t.TrainerName)
            .Select(t=>new TrainerLookupDTO(t.TrainerId, t.TrainerName))
            .ToListAsync(cancellationToken);
    }
}
