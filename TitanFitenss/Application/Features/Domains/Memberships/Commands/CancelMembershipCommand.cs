using MediatR;
using FluentValidation;
using TitanFitenss.Application.Common.Exceptions;
using TitanFitenss.Domain.MembershipAggregate;
using TitanFitenss.Domain.Interfaces;
namespace TitanFitenss.Application.Features.Domains.Memberships.Commands;
public record CancelMembershipCommand(int MembershipId):IRequest;

public class CancelMembershipCommandValidator:AbstractValidator<CancelMembershipCommand>
{
    public CancelMembershipCommandValidator()
    {
        RuleFor(x=>x.MembershipId).GreaterThan(0);
    }
}

public class CancelMembershipCommandHandler:IRequestHandler<CancelMembershipCommand>
{
    private readonly IMembershipRepository _membershipRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelMembershipCommandHandler(IMembershipRepository membershipRepository, IUnitOfWork unitOfWork)
    {
        _membershipRepository=membershipRepository;
        _unitOfWork=unitOfWork;
    }
    public async Task Handle(CancelMembershipCommand request, CancellationToken cancellationToken)
    {
        var membership=await _membershipRepository.GetByIdAsync(request.MembershipId, cancellationToken)
            ?? throw new NotFoundException(nameof(Membership), request.MembershipId);

        membership.Cancel();

        _membershipRepository.Update(membership);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
