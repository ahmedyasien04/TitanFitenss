using MediatR;
using FluentValidation;
using TitanFitenss.Application.Common.Exceptions;
using TitanFitenss.Domain.ClassSessionAggregate;
using TitanFitenss.Domain.Interfaces;
namespace TitanFitenss.Application.Features.Domains.ClassSessions.Commands;
public record CancelBookingCommand(int SessionId, int BookingId):IRequest;
public class CancelBookingCommandValidator:AbstractValidator<CancelBookingCommand>
{
    public CancelBookingCommandValidator()
    {
        RuleFor(x=>x.SessionId).GreaterThan(0);
        RuleFor(x=>x.BookingId).GreaterThan(0);
    }
}
public class CancelBookingCommandHandler:IRequestHandler<CancelBookingCommand>
{
    private readonly IClassSessionRepository _classSessionRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CancelBookingCommandHandler(IClassSessionRepository classSessionRepository, IUnitOfWork unitOfWork)
    {
        _classSessionRepository=classSessionRepository;
        _unitOfWork=unitOfWork;
    }
    public async Task Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        var session=await _classSessionRepository.GetByIdAsync(request.SessionId, cancellationToken)
            ?? throw new NotFoundException(nameof(ClassSession), request.SessionId);

        session.CancelBooking(request.BookingId);

        _classSessionRepository.Update(session);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
