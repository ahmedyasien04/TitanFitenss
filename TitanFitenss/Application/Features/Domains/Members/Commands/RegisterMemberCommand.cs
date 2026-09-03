using MediatR;
using FluentValidation;
using TitanFitenss.Application.Common.Exceptions;
using TitanFitenss.Domain.BranchAggregate;
using TitanFitenss.Domain.MemberAggregate;
using TitanFitenss.Domain.ValueObjects;
using TitanFitenss.Domain.Interfaces;
namespace TitanFitenss.Application.Features.Domains.Members.Commands;
public record RegisterMemberCommand(
    string FullName,
    string Email,
    string Phone,
    string City,
    string Street,
    int ApartmentNumber,
    DateOnly JoinDate,
    int HomeBranchId,
    string? Photo
):IRequest<int>;

public class RegisterMemberCommandValidator:AbstractValidator<RegisterMemberCommand>
{
    public RegisterMemberCommandValidator()
    {
        RuleFor(x=>x.FullName).NotEmpty().MaximumLength(100);
        RuleFor(x=>x.Email).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(x=>x.Phone).NotEmpty().MaximumLength(20);
        RuleFor(x=>x.City).NotEmpty().MaximumLength(100);
        RuleFor(x=>x.Street).NotEmpty().MaximumLength(97);
        RuleFor(x=>x.ApartmentNumber).GreaterThan(0);
        RuleFor(x=>x.HomeBranchId).GreaterThan(0);
    }
}

public class RegisterMemberCommandHandler:IRequestHandler<RegisterMemberCommand, int>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterMemberCommandHandler(
        IMemberRepository memberRepository,
        IBranchRepository branchRepository,
        IUnitOfWork unitOfWork)
    {
        _memberRepository=memberRepository;
        _branchRepository=branchRepository;
        _unitOfWork=unitOfWork;
    }

    public async Task<int> Handle(RegisterMemberCommand request, CancellationToken cancellationToken)
    {
        var homeBranch=await _branchRepository.GetByIdAsync(request.HomeBranchId, cancellationToken);
        if (homeBranch is null)
            throw new NotFoundException(nameof(Branch), request.HomeBranchId);

        var membershipNumber=await GenerateUniqueMembershipNumberAsync(cancellationToken);
        var address=new Address(request.City, request.Street, request.ApartmentNumber);

        var member=new Member(
            membershipNumber,
            request.FullName,
            request.Email,
            request.Phone,
            address,
            request.JoinDate,
            request.HomeBranchId,
            request.Photo);

        await _memberRepository.AddAsync(member, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return member.MemberId;
    }
    private async Task<MembershipNumber> GenerateUniqueMembershipNumberAsync(CancellationToken cancellationToken)
    {
        var random=new Random();

        for (var attempt=0; attempt<10; attempt++)
        {
            var candidateValue=$"TF-{random.Next(0, 9_999_999):D7}";
            var candidate=new MembershipNumber(candidateValue);

            var alreadyExists=await _memberRepository.MembershipNumberExistsAsync(candidate, cancellationToken);
            if (!alreadyExists)
                return candidate;
        }

        throw new InvalidOperationException("Could not generate a unique membership number, please try again.");
    }
}
