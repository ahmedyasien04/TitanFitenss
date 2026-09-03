using TitanFitenss.Domain.BranchAggregate;
namespace TitanFitenss.Domain.Interfaces;
    // repo interface for managing Branch aggregate
    public interface IBranchRepository
    {
        //get a specific branch using its primary key
        Task<Branch?> GetByIdAsync(int branchId, CancellationToken cancellationToken=default);
        //get a list of all branches 
        Task<IReadOnlyList<Branch>>GetAllAsync(CancellationToken cancellationToken=default);
        //add a branch to the chain
        Task AddAsync(Branch branch, CancellationToken cancellationToken=default);
        void Update(Branch branch);
        void Delete(Branch branch);
    }