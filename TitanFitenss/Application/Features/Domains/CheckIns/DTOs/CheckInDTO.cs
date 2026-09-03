namespace TitanFitenss.Application.Features.Domains.CheckIns.DTOs;
public record CheckInDTO(
    int CheckInId, int MemberId, int BranchId, string BranchName,
    DateTime CheckInDateTime, string Result, string? RefusalReason);
