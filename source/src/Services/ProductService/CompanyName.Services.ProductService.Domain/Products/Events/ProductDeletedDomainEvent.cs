using CSharpEssentials.Interfaces;
using CompanyName.Services.ProductService.Domain.Products.Fields;

namespace CompanyName.Services.ProductService.Domain.Products.Events;

public sealed record ProductDeletedDomainEvent(ProductId Id) : IDomainEvent;


