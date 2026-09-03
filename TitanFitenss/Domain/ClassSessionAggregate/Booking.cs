namespace TitanFitenss.Domain.ClassSessionAggregate;

    public class Booking
    {
    public int BookingId{get;private set;}
    public int SessionId{get;private set;}            
    public int MemberId{get;private set;}            
    public DateTime BookedOn{get;private set;}        
    public BookingStatus Status{get;private set;}     
    public int? WaitlistPosition{get;private set;}   
    public string? NotesForTrainer{get;private set;}  
    private Booking(){}
    internal Booking(
        int sessionId,
        int memberId,
        DateTime bookedOn,
        BookingStatus status,
        int? waitlistPosition=null,
        string? notesForTrainer=null)
    {
        SessionId=sessionId;
        MemberId=memberId;
        BookedOn=bookedOn;
        Status=status;
        WaitlistPosition=waitlistPosition;
        NotesForTrainer=notesForTrainer;
    }
    internal void ConfirmSpot()
    {
        Status=BookingStatus.Confirmed;
        WaitlistPosition=null;
    }
    internal void Cancel()
    {
        Status=BookingStatus.Cancelled;
        WaitlistPosition=null;
    }
    internal void UpdateWaitlistPosition(int newPosition)
    {
        if (Status!=BookingStatus.Waitlisted)
            throw new InvalidOperationException("Cannot update position for non-waitlisted booking");
        WaitlistPosition=newPosition;
    }    
    }