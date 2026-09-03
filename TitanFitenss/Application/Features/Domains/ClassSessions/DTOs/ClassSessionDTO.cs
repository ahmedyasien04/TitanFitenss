namespace TitanFitenss.Application.Features.Domains.ClassSessions.DTOs;

public record BookingDTO(
    int BookingId, int MemberId, string MemberName, DateTime BookedOn,
    string Status, int? WaitlistPosition, string? NotesForTrainer);
public record ClassSessionDTO(
    int SessionId,
    string ClassName,
    int BranchId,
    string BranchName,
    int StudioId,
    string StudioName,
    int TrainerId,
    string TrainerName,
    DateTime SessionDate,
    TimeSpan StartTime,
    int DurationInMinutes,
    int CapacityLimit,
    int ConfirmedCount,
    int WaitlistCount,
    string Status,
    string? Description,
    List<BookingDTO> Bookings
);
