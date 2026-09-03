using TitanFitenss.Domain.PlanAggregate;
namespace TitanFitenss.Domain.Interfaces;
    public interface IPlanRepository
    {
      Task<Plan?> GetByIdAsync(int planId, CancellationToken cancellationToken=default);
      Task<IReadOnlyList<Plan>> GetAllAsync(CancellationToken cancellationToken=default);
      Task<IReadOnlyList<Plan>> GetPublishedPlansAsync(CancellationToken cancellationToken=default);
      Task AddAsync(Plan plan, CancellationToken cancellationToken=default);
      void Update(Plan plan);
    }