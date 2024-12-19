using CSharpEssentials.EntityFrameworkCore;
using CompanyName.Services.ProductService.Domain.Products;
using CompanyName.Services.ProductService.Domain.Products.Fields;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyName.Services.ProductService.Persistence.EntityFrameworkCore.Configurations.Write;
internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.SoftDeletableEntityBaseMap<Product, ProductId>();

        builder
            .Property(p => p.Id)
            .HasConversion(id => id.Value, id => ProductId.From(id));

        builder
            .Property(p => p.Category)
            .HasConversion(id => id.Value, id => CategoryId.From(id));

        builder
            .Property(p => p.Name)
            .HasConversion(name => name.Value, name => ProductName.From(name))
            .HasMaxLength(ProductName.MaxLength);

        builder
            .Property(p => p.Description)
            .HasConversion(description => description.Value, description => ProductDescription.From(description))
            .HasMaxLength(ProductDescription.MaxLength);

        builder
            .ComplexProperty(p => p.Money, money =>
            {
                money.Property(m => m.Value).HasColumnName("price");
                money.Property(m => m.Currency).HasColumnName("currency");
            });

        builder.OptimisticConcurrencyVersionMap();

        builder.HasIndex(x => x.Category);
        builder.HasIndex(x => new { x.CreatedAt, x.IsDeleted });
    }
}
