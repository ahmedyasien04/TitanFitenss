using TitanFitenss.Domain.PlanAggregate;
using TitanFitenss.Domain.ValueObjects;
namespace TitanFitenss.Domain.MembershipAggregate;
    public class Membership
    {
    public int MembershipId{get;private set;}
    public int MemberId{get;private set;}
    public int PlanId{get;private set;}
    public DateTime PurchaseDate{get;private set;}
    public DateTime StartDate{get;private set;}
    public DateTime EndDate{get;private set;}
    public MembershipStatus Status{get;private set;}
    public AgreedTerms AgreedTerms{get;private set;}=null!;
    private readonly List<Freeze> _freezes=new();
    public IReadOnlyCollection<Freeze> Freezes=>_freezes.AsReadOnly();
    private readonly List<GuestPass> _guestPasses=new();
    public IReadOnlyCollection<GuestPass> GuestPasses=>_guestPasses.AsReadOnly();
    private Membership() { }
    public Membership(
        int memberId,
        Plan plan,
        DateTime purchaseDate,
        DateTime startDate)
    {
        if (memberId<=0)
            throw new ArgumentException("Valid Member ID is required",nameof(memberId));
        if (plan==null)
            throw new ArgumentNullException(nameof(plan));

        MemberId=memberId;
        PlanId=plan.PlanId;
        PurchaseDate=purchaseDate;
        StartDate=startDate.Date;
        EndDate=StartDate.AddMonths(plan.DurationInMonths);
        Status=startDate.Date>purchaseDate.Date?MembershipStatus.Pending:MembershipStatus.Active;
        AgreedTerms=new AgreedTerms(
            plan.Price,
            plan.DurationInMonths,
            plan.MaxFreezeDays,
            plan.MaxNumberOfFreezes,
            plan.GuestPassQuota,
            plan.AccessScope
        );
        for (int i=0;i<AgreedTerms.GuestPassQuota;i++)
        {
            _guestPasses.Add(new GuestPass(MembershipId, purchaseDate.Date));
        }
    }
    public void RequestFreeze(DateTime startDate,DateTime endDate,int durationInMonths,
     Reason reason,DateTime requestedOn,string? notes=null)
    {
        if (Status==MembershipStatus.Cancelled||Status==MembershipStatus.Expired)
            throw new InvalidOperationException("Cannot freeze an expired or cancelled membership");
        if (startDate<requestedOn.Date)
            throw new InvalidOperationException("A freeze cannot begin in the past");
        if (endDate>EndDate)
            throw new InvalidOperationException("A freeze cannot run past the end date of the membership");
        if (_freezes.Count>=AgreedTerms.MaxNumberOfFreezeDays)
            throw new InvalidOperationException($"Maximum allowed freezes ({AgreedTerms.MaxNumberOfFreezeDays}) reached");

        int totalFreezeDays=(int)(endDate.Date-startDate.Date).TotalDays;
        int currentFrozenDays=_freezes.Sum(f=>(int)(f.EndDate-f.StartDate).TotalDays);

        if (currentFrozenDays+totalFreezeDays>AgreedTerms.MaxFreezeDays)
            throw new InvalidOperationException($"Total freeze days would exceed limit of {AgreedTerms.MaxFreezeDays} days");

        var freeze=new Freeze(MembershipId,startDate,endDate,durationInMonths,reason,requestedOn,notes);
        _freezes.Add(freeze);

        EndDate=EndDate.AddDays(totalFreezeDays);
        Status=MembershipStatus.Frozen;
    }
    public void Cancel()
    {
        if (Status==MembershipStatus.Cancelled)
            throw new InvalidOperationException("Membership is already cancelled");
        Status=MembershipStatus.Cancelled;
    }
    public bool IsActiveOn(DateTime date)
    {
        return Status==MembershipStatus.Active && date.Date>=StartDate&&date.Date<=EndDate;
    }
    public bool CanAccessBranch(int branchId, int homeBranchId)
    {
        if (Status!=MembershipStatus.Active)
            return false;
        if (AgreedTerms.AccessScope==AccessScope.AllBranches)
            return true;
        return branchId==homeBranchId;
    }
    }