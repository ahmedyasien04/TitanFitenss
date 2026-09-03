using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TitanFitenss.Domain.MembershipAggregate;
namespace TitanFitenss.Infrastructure.Persistence;
    public class MembershipConfiguration:IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> b)
    {
        b.ToTable("Memberships");
        b.HasKey(m=>m.MembershipId);
        b.Property(m=>m.MemberId).IsRequired();
        b.Property(m=>m.PlanId).IsRequired();
        b.Property(m=>m.PurchaseDate).HasColumnType("datetime2").IsRequired();
        b.Property(m=>m.StartDate).HasColumnType("date").IsRequired();
        b.Property(m=>m.EndDate).HasColumnType("date").IsRequired();
        b.Property(m=>m.Status).HasConversion<int>().IsRequired();
        b.OwnsOne(m=>m.AgreedTerms, at =>
        {
            at.Property(a=>a.PricePaid).HasColumnName("PricePaid").HasPrecision(5,2).IsRequired();
            at.Property(a=>a.DurationInMonths).HasColumnName("DurationInMonths").IsRequired();
            at.Property(a=>a.MaxFreezeDays).HasColumnName("MaxFreezeDays").IsRequired();
            at.Property(a=>a.MaxNumberOfFreezeDays).HasColumnName("MaxNumberOfFreezes").IsRequired();
            at.Property(a=>a.GuestPassQuota).HasColumnName("GuestPassQuota").IsRequired(); 
            at.Property(a=>a.AccessScope).HasConversion<int>().IsRequired();    
        });
        b.OwnsMany(m=>m.Freezes,f=>
        {
            f.ToTable("Freezes");
            f.HasKey(z=>z.FreezeId);
            f.WithOwner().HasForeignKey("MembershipId");
            f.Property(z=>z.MembershipId);
            f.Property(z=>z.StartDate).HasColumnType("date").IsRequired();
            f.Property(z=>z.EndDate).HasColumnType("date").IsRequired();
            f.Property(z=>z.DurationInMonths).IsRequired();
            f.Property(z=>z.Reason).HasConversion<int>().IsRequired();
            f.Property(z=>z.AdditionalNotes).HasMaxLength(200);
            f.Property(z=>z.RequestedOn).HasColumnType("datetime2").IsRequired();
        });
        b.Navigation(m=>m.Freezes).Metadata.SetPropertyAccessMode(PropertyAccessMode.Field);

        b.OwnsMany(m=>m.GuestPasses,p=>
        {
            p.ToTable("GuestPasses");
            p.HasKey(g=>g.GuestPassId);
            p.WithOwner().HasForeignKey("MembershipId");
            p.Property(g=>g.MembershipId);
            p.Property(g=>g.IssuedOn).HasColumnType("date").IsRequired();
            p.Property(g=>g.UsedOn).HasColumnType("date").IsRequired(false);
            p.Property(g=>g.GuestName).HasMaxLength(100).IsRequired(false);
        });
        b.Navigation(m=>m.GuestPasses).Metadata.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
    }