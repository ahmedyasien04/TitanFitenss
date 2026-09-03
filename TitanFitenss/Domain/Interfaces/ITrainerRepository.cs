using TitanFitenss.Domain.TrainerAggregate;
namespace TitanFitenss.Domain.Interfaces;
    public interface ITrainerRepository
    {
      Task<Trainer?> GetByIdAsync(int trainerId, CancellationToken cancellationToken=default);
      Task<IReadOnlyList<Trainer>> GetAllAsync(CancellationToken cancellationToken=default);
      Task<IReadOnlyList<Trainer>> GetActiveTrainersAsync(CancellationToken cancellationToken=default);
      Task AddAsync(Trainer trainer, CancellationToken cancellationToken=default);
      void Update(Trainer trainer);
        
    }