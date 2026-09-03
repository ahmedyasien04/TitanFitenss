namespace TitanFitenss.Domain.MembershipAggregate;
    public class Freeze
    {
    public int FreezeId{get;private set;}
    public int MembershipId{get;private set;}
    public DateTime StartDate{get;private set;}       
    public DateTime EndDate{get;private set;}         
    public int DurationInMonths{get;private set;}     
    public Reason Reason{get;private set;}   
    public string? AdditionalNotes{get;private set;}  
    public DateTime RequestedOn{get;private set;}     
    private Freeze(){}
    internal Freeze(
        int membershipId,
        DateTime startDate,
        DateTime endDate,
        int durationInMonths,
        Reason reason,
        DateTime requestedOn,
        string? additionalNotes=null)
    {
        if (startDate>=endDate)
            throw new ArgumentException("Freeze start date must be before end date",nameof(startDate));
        if (durationInMonths<=0)
            throw new ArgumentException("Freeze duration must be at least 1 month",nameof(durationInMonths));

        MembershipId=membershipId;
        StartDate=startDate.Date;
        EndDate=endDate.Date;
        DurationInMonths=durationInMonths;
        Reason=reason;
        RequestedOn=requestedOn;
        AdditionalNotes=additionalNotes;
    }
    }