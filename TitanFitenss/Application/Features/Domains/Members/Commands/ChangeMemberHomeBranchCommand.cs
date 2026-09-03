using MediatR;
using FluentValidation;
using TitanFitenss.Application.Common.Exceptions;
using TitanFitenss.Domain.BranchAggregate;
using TitanFitenss.Domain.MemberAggregate;
using TitanFitenss.Domain.Interfaces;
namespace TitanFitenss.Application.Features.Domains.Members.Commands;
public record ChangeMemberHomeBranchCommand(int MemberId, int NewBranchId):IRequest;
public class ChangeMemberHomeBranchCommandValidator:AbstractValidator<ChangeMemberHomeBranchCommand>
{
    public ChangeMemberHomeBranchCommandValidator()
    {
        RuleFor(x=>x.MemberId).GreaterThan(0);
        RuleFor(x=>x.NewBranchId).GreaterThan(0);
    }
}
public class ChangeMemberHomeBranchCommandHandler:IRequestHandler<ChangeMemberHomeBranchCommand>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeMemberHomeBranchCommandHandler(
        IMemberRepository memberRepository,IBranchRepository branchRepository,IUnitOfWork unitOfWork)
    {
        _memberRepository=memberRepository;
        _branchRepository=branchRepository;
        _unitOfWork=unitOfWork;
    }

    public async Task Handle(ChangeMemberHomeBranchCommand request, CancellationToken cancellationToken)
    {
        var member=await _memberRepository.GetByIdAsync(request.MemberId, cancellationToken)
            ?? throw new NotFoundException(nameof(Member), request.MemberId);

        var newBranch=await _branchRepository.GetByIdAsync(request.NewBranchId, cancellationToken)
            ?? throw new NotFoundException(nameof(Branch), request.NewBranchId);

        member.ChangeHomeBranch(newBranch.BranchId);

        _memberRepository.Update(member);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
