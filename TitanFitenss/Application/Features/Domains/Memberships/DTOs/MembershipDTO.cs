namespace TitanFitenss.Application.Features.Domains.Memberships.DTOs;
public record FreezeDTO(
    int FreezeId, DateTime StartDate, DateTime EndDate,
    int DurationInMonths, string Reason, string? AdditionalNotes, DateTime RequestedOn);
public record GuestPassDTO(
    int GuestPassId, DateTime IssuedOn, DateTime? UsedOn, string? GuestName);
public record MembershipDTO(
    int MembershipId,
    int MemberId,
    int PlanId,
    string PlanName,
    DateTime PurchaseDate,
    DateTime StartDate,
    DateTime EndDate,
    string Status,
    decimal PricePaid,
    int AgreedDurationInMonths,
    int MaxFreezeDays,
    int MaxNumberOfFreezes,
    int GuestPassQuota,
    string AccessScope,
    List<FreezeDTO> Freezes,
    List<GuestPassDTO> GuestPasses
);
