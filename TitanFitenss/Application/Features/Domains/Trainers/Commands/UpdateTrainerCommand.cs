using MediatR;
using FluentValidation;
using TitanFitenss.Application.Common.Exceptions;
using TitanFitenss.Domain.TrainerAggregate;
using TitanFitenss.Domain.Interfaces;
namespace TitanFitenss.Application.Features.Domains.Trainers.Commands;
public record UpdateTrainerCommand(int TrainerId, string TrainerName, string Email, string Phone):IRequest;
public class UpdateTrainerCommandValidator:AbstractValidator<UpdateTrainerCommand>
{
    public UpdateTrainerCommandValidator()
    {
        RuleFor(x=>x.TrainerId).GreaterThan(0);
        RuleFor(x=>x.TrainerName).NotEmpty().MaximumLength(100);
        RuleFor(x=>x.Email).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(x=>x.Phone).NotEmpty().MaximumLength(20);
    }
}
public class UpdateTrainerCommandHandler:IRequestHandler<UpdateTrainerCommand>
{
    private readonly ITrainerRepository _trainerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTrainerCommandHandler(ITrainerRepository trainerRepository, IUnitOfWork unitOfWork)
    {
        _trainerRepository=trainerRepository;
        _unitOfWork=unitOfWork;
    }
    public async Task Handle(UpdateTrainerCommand request, CancellationToken cancellationToken)
    {
        var trainer=await _trainerRepository.GetByIdAsync(request.TrainerId, cancellationToken)
            ?? throw new NotFoundException(nameof(Trainer), request.TrainerId);

        trainer.UpdateDetails(request.TrainerName, request.Email, request.Phone);

        _trainerRepository.Update(trainer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
