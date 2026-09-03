namespace TitanFitenss.Application.Features.Domains.Branches.DTOs;
public record BranchDTO(
    int BranchId,
    string BranchName,
    DateTime OpeningTime,
    DateTime ClosingTime,
    string City,
    string Street,
    int ApartmentNumber,
    List<StudioDTO> Studios
);
