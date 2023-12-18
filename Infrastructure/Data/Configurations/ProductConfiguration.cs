using Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Id)
                .IsRequired();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AI");

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("SQL_Latin1_General_CP1_CI_AI");

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.PictureUrl)
                .IsRequired();

            builder.HasOne(pb => pb.ProductBrand).WithMany()
                .HasForeignKey(p => p.ProductBrandId);

            builder.HasOne(pt => pt.ProductType).WithMany()
               .HasForeignKey(p => p.ProductTypeId);


        }
    }
}
