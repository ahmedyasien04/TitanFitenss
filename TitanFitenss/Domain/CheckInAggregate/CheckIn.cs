namespace TitanFitenss.Domain.CheckInAggregate;
    public class CheckIn
    {
    public int CheckInId{get;private set;}
    public int MemberId{get;private set;}           
    public int BranchId{get;private set;}            
    public DateTime CheckInDateTime{get;private set;} 
    public CheckInResult Result{get;private set;}    
    public string? RefusalReason{get;private set;}
    private CheckIn(){}
    private CheckIn(int memberId,int branchId,DateTime checkInDateTime,CheckInResult result,
     string? refusalReason=null)
    {
        if (memberId<=0)
            throw new ArgumentException("Valid Member ID is required",nameof(memberId));
        if (branchId<=0)
            throw new ArgumentException("Valid Branch ID is required",nameof(branchId));

        MemberId=memberId;
        BranchId=branchId;
        CheckInDateTime=checkInDateTime;
        Result=result;
        RefusalReason=refusalReason;
    }
    public static CheckIn CreateGranted(int memberId, int branchId, DateTime checkInDateTime)
    {
        return new CheckIn(memberId, branchId, checkInDateTime, CheckInResult.Granted);
    }
    public static CheckIn CreateRefused(int memberId, int branchId, DateTime checkInDateTime,
     string refusalReason)
    {
        if (string.IsNullOrWhiteSpace(refusalReason))
            throw new ArgumentException("A reason must be provided when access is refused",nameof(refusalReason));

        return new CheckIn(memberId,branchId,checkInDateTime,CheckInResult.Refused,refusalReason);
    }
    }