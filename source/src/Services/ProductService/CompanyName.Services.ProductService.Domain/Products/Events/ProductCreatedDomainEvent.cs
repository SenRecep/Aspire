using CSharpEssentials.Interfaces;
using CompanyName.Services.ProductService.Domain.Products.Fields;

namespace CompanyName.Services.ProductService.Domain.Products.Events;
public sealed record ProductCreatedDomainEvent(ProductId Id) : IDomainEvent;


