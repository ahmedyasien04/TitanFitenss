using TitanFitenss.Domain.ValueObjects;
namespace TitanFitenss.Domain.PlanAggregate;

    public class Plan
    {
        public int PlanId{get;private set;}
        public string PlanName{get;private set;}=null!;
        public decimal Price{get;private set;}
        public int DurationInMonths{get;private set;}
        public int MaxFreezeDays{get;private set;}
        public int MaxNumberOfFreezes{get;private set;}
        public int GuestPassQuota{get;private set;}
        public AccessScope AccessScope{get;private set;}
        public bool IsPublished{get;private set;}
        private Plan(){}
        public Plan(
        string planName,
        decimal price,
        int durationInMonths,
        int maxFreezeDays,
        int maxNumberOfFreezes,
        int guestPassQuota,
        AccessScope accessScope,
        bool isPublished=false)
    {
        if (string.IsNullOrWhiteSpace(planName))
            throw new ArgumentException("Plan name is required",nameof(planName));
        if (price<0)
            throw new ArgumentException("Price cannot be negative",nameof(price));
        if (durationInMonths<=0)
            throw new ArgumentException("Duration in months must be greater than zero",nameof(durationInMonths));
        if (maxFreezeDays<0)
            throw new ArgumentException("Max freeze days cannot be negative",nameof(maxFreezeDays));
        if (maxNumberOfFreezes<0)
            throw new ArgumentException("Max number of freezes cannot be negative",nameof(maxNumberOfFreezes));
        if (guestPassQuota<0)
            throw new ArgumentException("Guest pass quota cannot be negative",nameof(guestPassQuota));
        PlanName=planName;
        Price=price;
        DurationInMonths=durationInMonths;
        MaxFreezeDays=maxFreezeDays;
        MaxNumberOfFreezes=maxNumberOfFreezes;
        GuestPassQuota=guestPassQuota;
        AccessScope=accessScope;
        IsPublished=isPublished;
    }
    public void UpdatePlanDetails(
        string planName,
        decimal price,
        int durationInMonths,
        int maxFreezeDays,
        int maxNumberOfFreezes,
        int guestPassQuota,
        AccessScope accessScope)
    {
        if (string.IsNullOrWhiteSpace(planName))
            throw new ArgumentException("Plan name is required",nameof(planName));
        if (price<0)
            throw new ArgumentException("Price cannot be negative",nameof(price));
        if (durationInMonths<=0)
            throw new ArgumentException("Duration in months must be greater than zero",nameof(durationInMonths));
        PlanName=planName;
        Price=price;
        DurationInMonths=durationInMonths;
        MaxFreezeDays=maxFreezeDays;
        MaxNumberOfFreezes=maxNumberOfFreezes;
        GuestPassQuota=guestPassQuota;
        AccessScope=accessScope;
    }
    public void Publish()
    {
        IsPublished=true;
    }
    public void Unpublish()
    {
        IsPublished=false;
    }
    }
