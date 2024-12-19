using System.Reflection;

namespace CompanyName.Services.ProductService.Domain;
public static class DomainAssemblyReference
{
    public static Assembly Assembly => typeof(DomainAssemblyReference).Assembly;
}
