using MediatR;
using FluentValidation;
using TitanFitenss.Application.Common.Exceptions;
using TitanFitenss.Domain.ClassSessionAggregate;
using TitanFitenss.Domain.Interfaces;
namespace TitanFitenss.Application.Features.Domains.ClassSessions.Commands;
public record CancelClassSessionCommand(int SessionId):IRequest;

public class CancelClassSessionCommandValidator:AbstractValidator<CancelClassSessionCommand>
{
    public CancelClassSessionCommandValidator()
    {
        RuleFor(x=>x.SessionId).GreaterThan(0);
    }
}
public class CancelClassSessionCommandHandler:IRequestHandler<CancelClassSessionCommand>
{
    private readonly IClassSessionRepository _classSessionRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CancelClassSessionCommandHandler(IClassSessionRepository classSessionRepository, IUnitOfWork unitOfWork)
    {
        _classSessionRepository=classSessionRepository;
        _unitOfWork=unitOfWork;
    }
    public async Task Handle(CancelClassSessionCommand request, CancellationToken cancellationToken)
    {
        var session=await _classSessionRepository.GetByIdAsync(request.SessionId, cancellationToken)
            ?? throw new NotFoundException(nameof(ClassSession), request.SessionId);

        session.CancelSession();

        _classSessionRepository.Update(session);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
