using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TitanFitenss.Domain.BranchAggregate;
namespace TitanFitenss.Infrastructure.Persistence;
    public class BranchConfiguration:IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> b)
    {
        //map to a db table
        b.ToTable("Branches");
        //primary key
        b.HasKey(b=>b.BranchId);
        b.Property(b=>b.BranchName).HasMaxLength(50).IsRequired();
        b.Property(b=>b.OpeningTime).HasColumnType("datetime2").IsRequired();
        b.Property(b=>b.ClosingTime).HasColumnType("datetime2").IsRequired();
        //building an address columnn in the table (owned type, vo)
        b.OwnsOne(b=>b.Address, address =>
        {
            address.Property(a=>a.City).HasColumnName("City").HasMaxLength(100).IsRequired();
            address.Property(a=>a.Street).HasColumnName("Street").HasMaxLength(97).IsRequired();
            address.Property(a=>a.ApartmentNumber).HasColumnName("ApartmentNumber").HasMaxLength(3).IsRequired();
        });
        b.HasMany(b => b.Studios)
       .WithOne(s => s.Branch) 
       .HasForeignKey(s => s.BranchId) 
       .OnDelete(DeleteBehavior.Cascade);

        b.Navigation(b=>b.Studios).Metadata.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}