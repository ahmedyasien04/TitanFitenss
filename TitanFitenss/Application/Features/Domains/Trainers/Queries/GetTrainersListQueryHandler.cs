using MediatR;
using Microsoft.EntityFrameworkCore;
using TitanFitenss.Application.Common.Models;
using TitanFitenss.Application.Features.Domains.Trainers.DTOs;
using TitanFitenss.Infrastructure.Persistence;
namespace TitanFitenss.Application.Features.Domains.Trainers.Queries;
public record GetTrainersListQuery(int PageNumber=1,int PageSize=20):IRequest<PaginatedList<TrainerDTO>>;

public class GetTrainersListQueryHandler:IRequestHandler<GetTrainersListQuery, PaginatedList<TrainerDTO>>
{
    private readonly TitanFitnessDbContext _context;

    public GetTrainersListQueryHandler(TitanFitnessDbContext context)
    {
        _context=context;
    }
    public async Task<PaginatedList<TrainerDTO>> Handle(GetTrainersListQuery request, CancellationToken cancellationToken)
    {
        var query=_context.Trainers
            .AsNoTracking()
            .OrderBy(t=>t.TrainerName)
            .Select(t=>new TrainerDTO(t.TrainerId, t.TrainerName, t.Email, t.Phone, t.IsActive));

        return await PaginatedList<TrainerDTO>.CreateAsync(
            query, request.PageNumber, request.PageSize, cancellationToken);
    }
}
