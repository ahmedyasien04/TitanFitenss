using MediatR;
using Microsoft.EntityFrameworkCore;
using TitanFitenss.Application.Common.Exceptions;
using TitanFitenss.Application.Features.Domains.Trainers.DTOs;
using TitanFitenss.Domain.TrainerAggregate;
using TitanFitenss.Infrastructure.Persistence;
namespace TitanFitenss.Application.Features.Domains.Trainers.Queries;
public record GetTrainerByIdQuery(int TrainerId):IRequest<TrainerDTO>;

public class GetTrainerByIdQueryHandler:IRequestHandler<GetTrainerByIdQuery, TrainerDTO>
{
    private readonly TitanFitnessDbContext _context;

    public GetTrainerByIdQueryHandler(TitanFitnessDbContext context)
    {
        _context=context;
    }
    public async Task<TrainerDTO> Handle(GetTrainerByIdQuery request, CancellationToken cancellationToken)
    {
        var trainer=await _context.Trainers
            .AsNoTracking()
            .Where(t=>t.TrainerId==request.TrainerId)
            .Select(t=>new TrainerDTO(t.TrainerId, t.TrainerName, t.Email, t.Phone, t.IsActive))
            .FirstOrDefaultAsync(cancellationToken);

        if (trainer is null)
            throw new NotFoundException(nameof(Trainer), request.TrainerId);

        return trainer;
    }
}
