namespace TitanFitenss.Domain.ValueObjects;

    public record MembershipNumber
{
    public string Value{get;}
    private MembershipNumber()
    {
        Value=null!;
    }
    public MembershipNumber(string value)
    {
        if(string.IsNullOrWhiteSpace(value))
          throw new ArgumentException("Membership Number is required",nameof(value));
        if(value.Length>10 ||value.Length<10)
          throw new ArgumentException("Membership Number has to exactly 10 digits");  
        Value=value;
    }
    public override string ToString()=>Value;
    //converting the value object record into a standard string 
    public static implicit operator string(MembershipNumber membershipNumber)=>membershipNumber.Value;
}
