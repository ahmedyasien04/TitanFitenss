using Microsoft.EntityFrameworkCore;
using TitanFitenss.Domain.TrainerAggregate;
using TitanFitenss.Domain.Interfaces;
using TitanFitenss.Infrastructure.Persistence;
namespace TitanFitenss.Infrastructure.Repositories;
    public class TrainerRepository:ITrainerRepository
    {
        private readonly TitanFitnessDbContext _context;
        public TrainerRepository(TitanFitnessDbContext context)
    {
        _context=context;
    }
    public async Task<Trainer?> GetByIdAsync(int trainerId, CancellationToken cancellationToken=default)
    {
        return await _context.Trainers.FirstOrDefaultAsync(t=>t.TrainerId==trainerId,cancellationToken);
    }
    public async Task<IReadOnlyList<Trainer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Trainers.ToListAsync(cancellationToken);
    }
    public async Task<IReadOnlyList<Trainer>> GetActiveTrainersAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Trainers
        .Where(t=>t.IsActive).ToListAsync(cancellationToken);
    }
    public async Task AddAsync(Trainer trainer, CancellationToken cancellationToken = default)
    {
        await _context.Trainers.AddAsync(trainer,cancellationToken);
    }
    public async void Update(Trainer trainer)=>_context.Trainers.Update(trainer);
        
    }