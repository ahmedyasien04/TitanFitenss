namespace TitanFitenss.Domain.ClassSessionAggregate;
    public class ClassSession
    {
    public int SessionId{get;private set;}
    public string ClassName{get;private set;}=null!;    
    public int BranchId{get;private set;}                
    public int StudioId{get;private set;}               
    public int TrainerId{get;private set;}               
    public DateTime SessionDate{get;private set;}      
    public TimeSpan StartTime{get;private set;}          
    public int DurationInMinutes{get;private set;}       
    public int CapacityLimit{get;private set;}          
    public SessionStatus Status{get;private set;}         
    public string? Description{get;private set;}          
    private readonly List<Booking> _bookings=new();
    public IReadOnlyCollection<Booking> Bookings=>_bookings.AsReadOnly();
    private ClassSession(){}
    public ClassSession(
        string className,
        int branchId,
        int studioId,
        int trainerId,
        DateTime sessionDate,
        TimeSpan startTime,
        int durationInMinutes,
        int capacityLimit,
        string? description=null)
    {
        if (string.IsNullOrWhiteSpace(className))
            throw new ArgumentException("Class name is required",nameof(className));
        if (capacityLimit<=0)
            throw new ArgumentException("Capacity limit must be greater than zero",nameof(capacityLimit));
        if (durationInMinutes<=0)
            throw new ArgumentException("Duration must be greater than zero",nameof(durationInMinutes));
        ClassName=className;
        BranchId=branchId;
        StudioId=studioId;
        TrainerId=trainerId;
        SessionDate=sessionDate.Date;
        StartTime=startTime;
        DurationInMinutes=durationInMinutes;
        CapacityLimit=capacityLimit;
        Status=SessionStatus.Scheduled;
        Description=description;
    }
    public Booking BookMember(int memberId,DateTime bookedOn,string? notesForTrainer=null)
    {
        if (Status!=SessionStatus.Scheduled)
            throw new InvalidOperationException("Cannot book a session that is not active or scheduled");
            
        var sessionStartsAt=SessionDate.Date+StartTime;
        if (sessionStartsAt<=bookedOn)
        throw new InvalidOperationException("This session has already started or finished");    

        if (_bookings.Any(b=>b.MemberId==memberId&&b.Status!=BookingStatus.Cancelled))
            throw new InvalidOperationException("Member already has an active booking or waitlist entry for this session");
        int activeConfirmedCount=_bookings.Count(b=>b.Status==BookingStatus.Confirmed);

        Booking booking;
        if (activeConfirmedCount<CapacityLimit)
        {
            booking=new Booking(SessionId,memberId,bookedOn,BookingStatus.Confirmed, null,notesForTrainer);
        }
        else
        {
            int currentWaitlistCount=_bookings.Count(b=>b.Status==BookingStatus.Waitlisted);
            booking=new Booking(SessionId,memberId,bookedOn,BookingStatus.Waitlisted,currentWaitlistCount+1,notesForTrainer);
        }
        _bookings.Add(booking);
        return booking;
    }
    public void CancelBooking(int bookingId)
    {
        var booking=_bookings.FirstOrDefault(b=>b.BookingId==bookingId);
        if (booking==null)
            throw new InvalidOperationException("Booking not found");
        if (booking.Status==BookingStatus.Cancelled)
            return;

        bool wasConfirmed=booking.Status==BookingStatus.Confirmed;
        booking.Cancel();
        if (wasConfirmed)
        {
            PromoteFromWaitlist();
        }
        else
        {
            ReorderWaitlist();
        }
    }
    private void PromoteFromWaitlist()
    {
        var nextInLine=_bookings
            .Where(b=>b.Status==BookingStatus.Waitlisted)
            .OrderBy(b=>b.WaitlistPosition)
            .FirstOrDefault();
        if (nextInLine!=null)
        {
            nextInLine.ConfirmSpot();
            ReorderWaitlist();
        }
    }
    private void ReorderWaitlist()
    {
        var waitlisted=_bookings
            .Where(b=>b.Status==BookingStatus.Waitlisted)
            .OrderBy(b=>b.WaitlistPosition)
            .ToList();
        for (int i=0;i<waitlisted.Count;i++)
        {
            waitlisted[i].UpdateWaitlistPosition(i+1);
        }
    }
    public void CancelSession()
    {
        Status=SessionStatus.Cancelled;
        foreach(var booking in _bookings.Where(b=>b.Status!=BookingStatus.Cancelled))
        {
            booking.Cancel();
        }
    }
    }