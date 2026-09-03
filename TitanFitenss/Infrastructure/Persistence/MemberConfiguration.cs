using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TitanFitenss.Domain.MemberAggregate;
using TitanFitenss.Domain.ValueObjects;
namespace TitanFitenss.Infrastructure.Persistence;
    public class MemberConfiguration:IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> b)
    {
        b.ToTable("Members");
        b.HasKey(b=>b.MemberId);
        b.Property(m=>m.MembershipNumber)
        //mapping the value object into a a string and ensuring uniqueness
        .HasConversion(vo=>vo.Value, value => new MembershipNumber(value))
        .HasColumnName("MembershipNumber").HasMaxLength(10).IsRequired();
        b.HasIndex(m => m.MembershipNumber).IsUnique();

        b.Property(m=>m.FullName).HasMaxLength(100).IsRequired();
        b.Property(m=>m.Email).HasMaxLength(100).IsRequired();
        b.Property(m=>m.Phone).HasMaxLength(20).IsRequired();
        b.Property(m=>m.JoinDate).HasColumnType("date").IsRequired();
        b.Property(m=>m.Photo);
        b.Property(m=>m.HomeBranchId).IsRequired();
        b.OwnsOne(m => m.Address, address =>
         {
            address.Property(a=>a.City).HasColumnName("City").HasMaxLength(100).IsRequired();
            address.Property(a=>a.Street).HasColumnName("Street").HasMaxLength(97).IsRequired();
            address.Property(a=>a.ApartmentNumber).HasColumnName("ApartmentNumber").HasMaxLength(3).IsRequired();
        });   
    }     
    }