using CSharpEssentials.Interfaces;
using CompanyName.Services.CategoryService.Domain.Categories.Fields;

namespace CompanyName.Services.CategoryService.Domain.Categories.Events;

public sealed record CategoryDeletedDomainEvent(CategoryId Id) : IDomainEvent;


