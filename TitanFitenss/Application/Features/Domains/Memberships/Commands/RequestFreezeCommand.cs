using MediatR;
using FluentValidation;
using TitanFitenss.Application.Common.Exceptions;
using TitanFitenss.Domain.MembershipAggregate;
using TitanFitenss.Domain.Interfaces;
namespace TitanFitenss.Application.Features.Domains.Memberships.Commands;
public record RequestFreezeCommand(
    int MembershipId,
    DateOnly StartDate,
    int DurationInMonths,
    Reason Reason,
    string? AdditionalNotes
):IRequest;
public class RequestFreezeCommandValidator:AbstractValidator<RequestFreezeCommand>
{
    public RequestFreezeCommandValidator()
    {
        RuleFor(x=>x.MembershipId).GreaterThan(0);
        RuleFor(x=>x.DurationInMonths).GreaterThan(0);
        RuleFor(x=>x.Reason).IsInEnum();
        RuleFor(x=>x.AdditionalNotes).MaximumLength(200);
    }
}
public class RequestFreezeCommandHandler:IRequestHandler<RequestFreezeCommand>
{
    private readonly IMembershipRepository _membershipRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RequestFreezeCommandHandler(IMembershipRepository membershipRepository, IUnitOfWork unitOfWork)
    {
        _membershipRepository=membershipRepository;
        _unitOfWork=unitOfWork;
    }
    public async Task Handle(RequestFreezeCommand request, CancellationToken cancellationToken)
    {
        var membership=await _membershipRepository.GetByIdAsync(request.MembershipId, cancellationToken)
            ?? throw new NotFoundException(nameof(Membership), request.MembershipId);

        var requestedOn=DateTime.UtcNow;
        var startDate=request.StartDate.ToDateTime(TimeOnly.MinValue);
        var endDate=startDate.AddMonths(request.DurationInMonths);

        membership.RequestFreeze(
            startDate, endDate, request.DurationInMonths, request.Reason, requestedOn, request.AdditionalNotes);

        _membershipRepository.Update(membership);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
