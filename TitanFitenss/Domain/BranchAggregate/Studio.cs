namespace TitanFitenss.Domain.BranchAggregate;
    public class Studio
    {
        public int StudioId{get;set;}
        public string StudioName{get;set;}=null!;
        public int BranchId{get;set;}
        public Branch Branch{get;set;}=null!;
        public int Capacity{get;set;}  
        //private constructor for EF 
        private Studio(){}
        internal Studio(string studioName, int branchId, int capacity)
    {
        if (string.IsNullOrWhiteSpace(studioName))
        {
            throw new ArgumentException("Studio name is required",nameof(studioName));
        }
        if (capacity<=0)
        {
            throw new ArgumentException("Capacity has to be greater than zero",nameof(capacity));
        }
        StudioName=studioName;
        BranchId=branchId;
        Capacity=capacity;
    }
        public void  UpdateDetails(string studioName, int capacity)
    {
        if (string.IsNullOrWhiteSpace(studioName))
        {
            throw new ArgumentException("Studio name is required",nameof(studioName));
        }
        if (capacity<=0)
        {
            throw new ArgumentException("Capacity has to be greater than zero",nameof(capacity));
        }
        StudioName=studioName;
        Capacity=capacity;
        
    }

    }
