using MediatR;
using FluentValidation;
using TitanFitenss.Domain.BranchAggregate;
using TitanFitenss.Domain.ValueObjects;
using TitanFitenss.Domain.Interfaces;
namespace TitanFitenss.Application.Features.Domains.Branches.Commands;
public record CreateStudioCommand(string StudioName, int Capacity);

public record CreateBranchCommand(
    string BranchName,
    DateTime OpeningTime,
    DateTime ClosingTime,
    string City,
    string Street,
    int ApartmentNumber,
    List<CreateStudioCommand> Studios
):IRequest<int>;
public class CreateBranchCommandValidator:AbstractValidator<CreateBranchCommand>
{
    public CreateBranchCommandValidator()
    {
        RuleFor(x=>x.BranchName).NotEmpty().MaximumLength(50);
        RuleFor(x=>x.City).NotEmpty().MaximumLength(100);
        RuleFor(x=>x.Street).NotEmpty().MaximumLength(97);
        RuleFor(x=>x.ApartmentNumber).GreaterThan(0);
        RuleFor(x=>x.ClosingTime).GreaterThan(x=>x.OpeningTime)
            .WithMessage("Closing time must be after opening time.");

        RuleForEach(x=>x.Studios).ChildRules(studio=>
        {
            studio.RuleFor(s=>s.StudioName).NotEmpty().MaximumLength(50);
            studio.RuleFor(s=>s.Capacity).GreaterThan(0);
        });
    }
}
public class CreateBranchCommandHandler:IRequestHandler<CreateBranchCommand, int>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateBranchCommandHandler(IBranchRepository branchRepository, IUnitOfWork unitOfWork)
    {
        _branchRepository=branchRepository;
        _unitOfWork=unitOfWork;
    }
    public async Task<int> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
    {
        var address=new Address(request.City, request.Street, request.ApartmentNumber);
        var branch=new Branch(request.BranchName,address,request.OpeningTime,request.ClosingTime);

        foreach (var studio in request.Studios)
        {
            branch.AddStudio(studio.StudioName, studio.Capacity);
        }

        await _branchRepository.AddAsync(branch, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return branch.BranchId;
    }
}
