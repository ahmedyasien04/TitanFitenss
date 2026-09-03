using TitanFitenss.Domain.ValueObjects;
namespace TitanFitenss.Domain.MembershipAggregate;
    public record AgreedTerms(
        decimal PricePaid,
        int DurationInMonths,
        int MaxFreezeDays,
        int MaxNumberOfFreezeDays,
        int GuestPassQuota,
        AccessScope AccessScope
    );