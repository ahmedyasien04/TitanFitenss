namespace TitanFitenss.Domain.MembershipAggregate;
    public class GuestPass
    {
    public int GuestPassId{get;private set;}
    public int MembershipId{get;private set;}
    public DateTime IssuedOn{get;private set;}
    public DateTime? UsedOn{get;private set;}     
    public string? GuestName{get;private set;}    
    private GuestPass(){}
    internal GuestPass(int membershipId, DateTime issuedOn)
    {
        MembershipId=membershipId;
        IssuedOn=issuedOn.Date;
    }
    public void UsePass(string guestName, DateTime usedOn)
    {
        if (UsedOn.HasValue)
            throw new InvalidOperationException("This guest pass has already been used");

        if (string.IsNullOrWhiteSpace(guestName))
            throw new ArgumentException("Guest name is required when using a pass",nameof(guestName));
        GuestName=guestName;
        UsedOn=usedOn.Date;
    }
    }