namespace TitanFitenss.Application.Features.Domains.Trainers.DTOs;
public record TrainerDTO(int TrainerId, string TrainerName, string Email, string Phone, bool IsActive);

public record TrainerLookupDTO(int TrainerId, string TrainerName);
