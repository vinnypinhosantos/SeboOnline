using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SeboOnline.Models;

namespace SeboOnline.Data.Mappings
{
    public class TransactionMap : IEntityTypeConfiguration<Transaction>
    {
       public void Configure(EntityTypeBuilder<Transaction> builder)
       {
           builder.ToTable("Transaction");

           builder.HasKey(t => t.Id);

           builder.Property(t => t.IdBuyer)
               .IsRequired();

           builder.Property(t => t.IdSeller)
               .IsRequired();

           builder.Property(t => t.IdItem)
              .IsRequired();

                builder.Property(t => t.Date)
                    .IsRequired();

            builder.Property(t => t.Value)
                .IsRequired()
                .HasColumnType("decimal(18, 2)");

            builder.HasOne(t => t.Buyer)
                    .WithMany()
                    .HasForeignKey(t => t.IdBuyer)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(t => t.Seller)
                    .WithMany()
                    .HasForeignKey(t => t.IdSeller)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(t => t.Item)
                    .WithMany()
                    .HasForeignKey(t => t.IdItem)
                    .OnDelete(DeleteBehavior.Restrict);
            }
        }
    }
