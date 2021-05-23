using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImage");
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id).UseIdentityColumn();
            builder.HasOne(d => d.Product).WithMany(d => d.ProductImages).HasForeignKey(d => d.ProductId);
            builder.Property(d => d.ImagePath).IsRequired();
            builder.Property(d => d.Caption).HasMaxLength(50).IsRequired();
            builder.Property(d => d.IsDefault);
            builder.Property(d => d.DateCreated).IsRequired();
            builder.Property(d => d.SortOrder).IsRequired();
            builder.Property(d => d.FileSize).IsRequired();


        }
    }
}
