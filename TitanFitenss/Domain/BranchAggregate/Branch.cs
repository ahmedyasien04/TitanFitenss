using TitanFitenss.Domain.ValueObjects;
namespace TitanFitenss.Domain.BranchAggregate;
    public class Branch
    {
        public int BranchId{get;set;}
        public string BranchName{get;set;}=null!;
        public Address Address {get;set;}=null!;
        public DateTime OpeningTime{get;set;}
        public DateTime ClosingTime{get;set;}
        private readonly List<Studio> _studios=new();
        public IReadOnlyCollection<Studio> Studios=> _studios.AsReadOnly();
        private Branch(){}
        internal Branch(string branchName,Address address,DateTime openingTime,DateTime closingTime)
    {
        if (string.IsNullOrWhiteSpace(branchName))
            throw new ArgumentException("Branch Name is required",nameof(branchName));
        if (openingTime>=closingTime)
            throw new ArgumentException("Opening Time has to be earlier Closing Time");

        BranchName=branchName;
        Address=address?? throw new ArgumentException(nameof(address));
        OpeningTime=openingTime;
        ClosingTime=closingTime;
    }
        public void UpdateInformation(string branchName,Address address,DateTime openingTime,DateTime closingTime)
    {
        if (string.IsNullOrWhiteSpace(branchName))
            throw new ArgumentException("Branch name is required",nameof(branchName));
        if (openingTime>=closingTime)
            throw new ArgumentException("Opening Time has to be earlier Closing Time");
        BranchName=branchName;
        Address=address?? throw new ArgumentException(nameof(address));
        OpeningTime=openingTime;
        ClosingTime=closingTime; 
    }
    public Studio AddStudio(string studioName, int capacity)
    {
        var studio=new Studio(studioName,BranchId, capacity);
        _studios.Add(studio);
        return studio;
    }
    }
