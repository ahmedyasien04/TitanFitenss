using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TitanFitenss.Domain.ClassSessionAggregate;
namespace TitanFitenss.Infrastructure.Persistence
{
    public class ClassSessionConfiguration:IEntityTypeConfiguration<ClassSession>
    {
        public void Configure(EntityTypeBuilder<ClassSession> b)
        {

            b.ToTable("ClassSessions");
            b.HasKey(c=>c.SessionId);
            b.Property(c=>c.ClassName).HasMaxLength(100).IsRequired();
            b.Property(c=>c.BranchId).IsRequired();
            b.Property(c=>c.StudioId).IsRequired();
            b.Property(c=>c.TrainerId).IsRequired();
            b.Property(c=>c.SessionDate).HasColumnType("date").IsRequired();
            b.Property(c=>c.StartTime).IsRequired();
            b.Property(c=>c.DurationInMinutes).IsRequired();
            b.Property(c=>c.CapacityLimit).IsRequired();
            b.Property(c=>c.Status).HasConversion<int>().IsRequired();
            b.Property(c=>c.Description).HasMaxLength(250).IsRequired(false);

            b.OwnsMany(c=>c.Bookings,bo=>
        {
            bo.ToTable("Bookings");
            bo.HasKey(b=>b.BookingId);
            bo.WithOwner().HasForeignKey("SessionId");
            bo.Property(b=>b.SessionId);
            bo.Property(b=>b.MemberId).IsRequired();
            bo.Property(b=>b.BookedOn).HasColumnType("datetime2").IsRequired();
            bo.Property(b=>b.Status).HasConversion<int>().IsRequired();
            bo.Property(b=>b.WaitlistPosition).IsRequired(false);
            bo.Property(b=>b.NotesForTrainer).HasMaxLength(500).IsRequired(false);
        });
        b.Navigation(c=>c.Bookings).Metadata.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}