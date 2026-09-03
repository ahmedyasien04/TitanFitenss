using MediatR;
using FluentValidation;
using TitanFitenss.Application.Common.Exceptions;
using TitanFitenss.Domain.MembershipAggregate;
using TitanFitenss.Domain.Interfaces;
namespace TitanFitenss.Application.Features.Domains.Memberships.Commands;
public record UseGuestPassCommand(int MembershipId, int GuestPassId, string GuestName):IRequest;

public class UseGuestPassCommandValidator:AbstractValidator<UseGuestPassCommand>
{
    public UseGuestPassCommandValidator()
    {
        RuleFor(x=>x.MembershipId).GreaterThan(0);
        RuleFor(x=>x.GuestPassId).GreaterThan(0);
        RuleFor(x=>x.GuestName).NotEmpty().MaximumLength(100);
    }
}
public class UseGuestPassCommandHandler:IRequestHandler<UseGuestPassCommand>
{
    private readonly IMembershipRepository _membershipRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UseGuestPassCommandHandler(IMembershipRepository membershipRepository, IUnitOfWork unitOfWork)
    {
        _membershipRepository=membershipRepository;
        _unitOfWork=unitOfWork;
    }

    public async Task Handle(UseGuestPassCommand request, CancellationToken cancellationToken)
    {
        var membership=await _membershipRepository.GetByIdAsync(request.MembershipId, cancellationToken)
            ?? throw new NotFoundException(nameof(Membership), request.MembershipId);
        var guestPass=membership.GuestPasses.FirstOrDefault(g=>g.GuestPassId==request.GuestPassId)
            ?? throw new NotFoundException("GuestPass", request.GuestPassId);

        guestPass.UsePass(request.GuestName, DateTime.UtcNow);

        _membershipRepository.Update(membership);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
