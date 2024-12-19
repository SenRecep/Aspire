using CompanyName.Services.CategoryService.Domain.Categories.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyName.Services.CategoryService.Persistence.EntityFrameworkCore.Configurations.Read;
internal sealed class CategoryReadModelConfigurations : IEntityTypeConfiguration<CategoryReadModel>
{
    public void Configure(EntityTypeBuilder<CategoryReadModel> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
