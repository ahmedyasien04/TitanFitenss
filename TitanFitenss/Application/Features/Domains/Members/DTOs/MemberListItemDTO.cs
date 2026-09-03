namespace TitanFitenss.Application.Features.Domains.Members.DTOs;
public record MemberListItemDTO(
    int MemberId,
    string MembershipNumber,
    string FullName,
    string BranchName,
    string? MembershipStatus
);
