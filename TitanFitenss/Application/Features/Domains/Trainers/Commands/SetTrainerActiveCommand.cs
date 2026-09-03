using MediatR;
using FluentValidation;
using TitanFitenss.Application.Common.Exceptions;
using TitanFitenss.Domain.TrainerAggregate;
using TitanFitenss.Domain.Interfaces;
namespace TitanFitenss.Application.Features.Domains.Trainers.Commands;
public record SetTrainerActiveCommand(int TrainerId, bool IsActive):IRequest;
public class SetTrainerActiveCommandValidator:AbstractValidator<SetTrainerActiveCommand>
{
    public SetTrainerActiveCommandValidator()
    {
        RuleFor(x=>x.TrainerId).GreaterThan(0);
    }
}
public class SetTrainerActiveCommandHandler:IRequestHandler<SetTrainerActiveCommand>
{
    private readonly ITrainerRepository _trainerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetTrainerActiveCommandHandler(ITrainerRepository trainerRepository, IUnitOfWork unitOfWork)
    {
        _trainerRepository=trainerRepository;
        _unitOfWork=unitOfWork;
    }

    public async Task Handle(SetTrainerActiveCommand request, CancellationToken cancellationToken)
    {
        var trainer=await _trainerRepository.GetByIdAsync(request.TrainerId, cancellationToken)
            ?? throw new NotFoundException(nameof(Trainer), request.TrainerId);

        if (request.IsActive) trainer.Activate(); else trainer.Deactivate();

        _trainerRepository.Update(trainer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
