using MediatR;
using FluentValidation;
using TitanFitenss.Application.Common.Exceptions;
using TitanFitenss.Domain.MemberAggregate;
using TitanFitenss.Domain.ValueObjects;
using TitanFitenss.Domain.Interfaces;
namespace TitanFitenss.Application.Features.Domains.Members.Commands;
public record UpdateMemberProfileCommand(
    int MemberId,
    string FullName,
    string Email,
    string Phone,
    string City,
    string Street,
    int ApartmentNumber,
    string? Photo
):IRequest;

public class UpdateMemberProfileCommandValidator:AbstractValidator<UpdateMemberProfileCommand>
{
    public UpdateMemberProfileCommandValidator()
    {
        RuleFor(x=>x.MemberId).GreaterThan(0);
        RuleFor(x=>x.FullName).NotEmpty().MaximumLength(100);
        RuleFor(x=>x.Email).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(x=>x.Phone).NotEmpty().MaximumLength(20);
        RuleFor(x=>x.City).NotEmpty().MaximumLength(100);
        RuleFor(x=>x.Street).NotEmpty().MaximumLength(97);
        RuleFor(x=>x.ApartmentNumber).GreaterThan(0);
    }
}

public class UpdateMemberProfileCommandHandler:IRequestHandler<UpdateMemberProfileCommand>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMemberProfileCommandHandler(IMemberRepository memberRepository, IUnitOfWork unitOfWork)
    {
        _memberRepository=memberRepository;
        _unitOfWork=unitOfWork;
    }

    public async Task Handle(UpdateMemberProfileCommand request, CancellationToken cancellationToken)
    {
        var member=await _memberRepository.GetByIdAsync(request.MemberId, cancellationToken)
            ?? throw new NotFoundException(nameof(Member), request.MemberId);

        var address=new Address(request.City, request.Street, request.ApartmentNumber);

        member.UpdateProfile(request.FullName, request.Email, request.Phone, address, request.Photo);

        _memberRepository.Update(member);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
