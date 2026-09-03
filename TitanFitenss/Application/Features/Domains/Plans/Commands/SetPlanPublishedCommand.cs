using MediatR;
using FluentValidation;
using TitanFitenss.Application.Common.Exceptions;
using TitanFitenss.Domain.PlanAggregate;
using TitanFitenss.Domain.Interfaces;
namespace TitanFitenss.Application.Features.Domains.Plans.Commands;
public record SetPlanPublishedCommand(int PlanId, bool IsPublished):IRequest;

public class SetPlanPublishedCommandValidator:AbstractValidator<SetPlanPublishedCommand>
{
    public SetPlanPublishedCommandValidator()
    {
        RuleFor(x=>x.PlanId).GreaterThan(0);
    }
}
public class SetPlanPublishedCommandHandler:IRequestHandler<SetPlanPublishedCommand>
{
    private readonly IPlanRepository _planRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetPlanPublishedCommandHandler(IPlanRepository planRepository, IUnitOfWork unitOfWork)
    {
        _planRepository=planRepository;
        _unitOfWork=unitOfWork;
    }
    public async Task Handle(SetPlanPublishedCommand request, CancellationToken cancellationToken)
    {
        var plan=await _planRepository.GetByIdAsync(request.PlanId, cancellationToken)
            ?? throw new NotFoundException(nameof(Plan), request.PlanId);

        if (request.IsPublished) plan.Publish(); else plan.Unpublish();

        _planRepository.Update(plan);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
