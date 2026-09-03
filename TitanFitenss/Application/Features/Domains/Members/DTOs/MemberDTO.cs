namespace TitanFitenss.Application.Features.Domains.Members.DTOs;
public record MemberDTO(
    int MemberId,
    string MembershipNumber,
    string FullName,
    string Email,
    string? Phone,
    string City,
    string Street,
    int ApartmentNumber,
    DateOnly JoinDate,
    int HomeBranchId,
    string HomeBranchName,
    string? Photo
);
