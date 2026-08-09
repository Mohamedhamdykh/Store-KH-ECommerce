using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.KH.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.KH.Repository.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(P => P.Name).HasMaxLength(200).IsRequired();
            builder.Property(P => P.PictureUrl).IsRequired(true);
            builder.Property(P => P.Price).HasColumnType("decimal(18,2)");

            builder.HasOne(P => P.ProductBrand)
                   .WithMany()
                   .HasForeignKey(P => P.ProductBrandId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(P => P.ProductType)
                  .WithMany()
                  .HasForeignKey(P => P.ProductTypeId)
                  .OnDelete(DeleteBehavior.SetNull);

            builder.Property(P => P.ProductBrandId).IsRequired(false);
            builder.Property(P => P.ProductTypeId).IsRequired(false);
        }
    }
}
