using MediatR;
using FluentValidation;
using TitanFitenss.Domain.TrainerAggregate;
using TitanFitenss.Domain.Interfaces;
namespace TitanFitenss.Application.Features.Domains.Trainers.Commands;
public record CreateTrainerCommand(string TrainerName, string Email, string Phone):IRequest<int>;
public class CreateTrainerCommandValidator:AbstractValidator<CreateTrainerCommand>
{
    public CreateTrainerCommandValidator()
    {
        RuleFor(x=>x.TrainerName).NotEmpty().MaximumLength(100);
        RuleFor(x=>x.Email).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(x=>x.Phone).NotEmpty().MaximumLength(20);
    }
}
public class CreateTrainerCommandHandler:IRequestHandler<CreateTrainerCommand, int>
{
    private readonly ITrainerRepository _trainerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTrainerCommandHandler(ITrainerRepository trainerRepository, IUnitOfWork unitOfWork)
    {
        _trainerRepository=trainerRepository;
        _unitOfWork=unitOfWork;
    }
    public async Task<int> Handle(CreateTrainerCommand request, CancellationToken cancellationToken)
    {
        var trainer=new Trainer(request.TrainerName, request.Email, request.Phone);

        await _trainerRepository.AddAsync(trainer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return trainer.TrainerId;
    }
}
