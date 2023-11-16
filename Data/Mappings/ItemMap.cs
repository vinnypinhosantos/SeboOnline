using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeboOnline.Models;

namespace SeboOnline.Data.Mappings
{
    public class ItemMap : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Item");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasColumnName("Title")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(100);

            builder.Property(x => x.Author)
                .IsRequired()
                .HasColumnName("Author")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(100);

            builder.Property(x => x.Price)
                .IsRequired()
                .HasColumnName("Price")
                .HasColumnType("DECIMAL(18,2)");

            builder.Property(x => x.Description)
                .HasColumnName("Description")
                .HasColumnType("TEXT");

            builder.Property(x => x.SellerId)
                .HasColumnName("SellerId")
                .IsRequired();

            builder
                .HasOne(x => x.Category)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.CategoryId)
                .HasConstraintName("FK_Item_CategoryId")
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Seller)
                .WithMany()
                .HasForeignKey(x => x.SellerId)
                .HasConstraintName("FK_Item_SellerId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
