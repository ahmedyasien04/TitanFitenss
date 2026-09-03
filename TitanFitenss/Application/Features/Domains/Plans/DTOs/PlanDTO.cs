namespace TitanFitenss.Application.Features.Domains.Plans.DTOs;
public record PlanDTO(
    int PlanId,
    string PlanName,
    decimal Price,
    int DurationInMonths,
    int MaxFreezeDays,
    int MaxNumberOfFreezes,
    int GuestPassQuota,
    string AccessScope,
    bool IsPublished
);
