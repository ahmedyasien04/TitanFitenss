namespace TitanFitenss.Application.Features.Domains.Dashboard.DTOs;
public record TodaysSessionDTO(
    int SessionId, string ClassName, TimeSpan StartTime, string StudioName,
    string TrainerName, int ConfirmedCount, int CapacityLimit, string Status);
public record DashboardStatsDTO(
    int CheckInsToday,
    int ActiveMembershipsCount,
    int TotalBookingsToday,
    double AverageFillRatePercent,
    List<TodaysSessionDTO> TodaysSessions
);
