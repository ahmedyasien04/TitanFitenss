using TitanFitenss.Domain.ValueObjects;
namespace TitanFitenss.Domain.MemberAggregate;
    public class Member
    {
        public int MemberId{get;private set;}
        public MembershipNumber MembershipNumber{get;private set;}=null!;
        public string FullName{get;private set;}=null!;
        public string Email{get;private set;}=null!;
        public string? Phone{get;private set;}
        public Address Address{get;private set;}=null!;
        public DateOnly JoinDate{get;private set;}
         public int HomeBranchId{get;private set;}
        public string? Photo{get;private set;}
        private Member(){}
        public Member(
        MembershipNumber membershipNumber,string fullName,string email,string phone,
        Address address,DateOnly joinDate, int homeBranchId,string? photo=null)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name is required.",nameof(fullName));
        if (homeBranchId<=0)
            throw new ArgumentException("Home branch ID must be valid.",nameof(homeBranchId));
        MembershipNumber=membershipNumber??throw new ArgumentNullException(nameof(membershipNumber));
        FullName=fullName;
        Email=email;
        Phone=phone;
        Address=address ?? throw new ArgumentNullException(nameof(address));
        JoinDate=joinDate;
        HomeBranchId=homeBranchId;
        Photo=photo;
    }
    public void UpdateProfile(string fullName,string email,string phone,
    Address address,string? photo)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name is required.",nameof(fullName));
        FullName=fullName;
        Email=email;
        Phone=phone;
        Address=address??throw new ArgumentNullException(nameof(address));
        Photo=photo;
    }
    public void ChangeHomeBranch(int newBranchId)
    {
        if (newBranchId<=0)
            throw new ArgumentException("Branch ID must be valid.",nameof(newBranchId));

        HomeBranchId=newBranchId;
    }
    }
