using MediatR;
using FluentValidation;
using TitanFitenss.Domain.PlanAggregate;
using TitanFitenss.Domain.ValueObjects;
using TitanFitenss.Domain.Interfaces;
namespace TitanFitenss.Application.Features.Domains.Plans.Commands;
public record CreatePlanCommand(
    string PlanName,
    decimal Price,
    int DurationInMonths,
    int MaxFreezeDays,
    int MaxNumberOfFreezes,
    int GuestPassQuota,
    AccessScope AccessScope
):IRequest<int>;
public class CreatePlanCommandValidator:AbstractValidator<CreatePlanCommand>
{
    public CreatePlanCommandValidator()
    {
        RuleFor(x=>x.PlanName).NotEmpty().MaximumLength(50);
        RuleFor(x=>x.Price).GreaterThanOrEqualTo(0);
        RuleFor(x=>x.DurationInMonths).GreaterThan(0);
        RuleFor(x=>x.MaxFreezeDays).GreaterThanOrEqualTo(0);
        RuleFor(x=>x.MaxNumberOfFreezes).GreaterThanOrEqualTo(0);
        RuleFor(x=>x.GuestPassQuota).GreaterThanOrEqualTo(0);
        RuleFor(x=>x.AccessScope).IsInEnum();
    }
}
public class CreatePlanCommandHandler:IRequestHandler<CreatePlanCommand, int>
{
    private readonly IPlanRepository _planRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePlanCommandHandler(IPlanRepository planRepository, IUnitOfWork unitOfWork)
    {
        _planRepository=planRepository;
        _unitOfWork=unitOfWork;
    }
    public async Task<int> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
    {
        var plan=new Plan(
            request.PlanName,
            request.Price,
            request.DurationInMonths,
            request.MaxFreezeDays,
            request.MaxNumberOfFreezes,
            request.GuestPassQuota,
            request.AccessScope);

        await _planRepository.AddAsync(plan, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return plan.PlanId;
    }
}
