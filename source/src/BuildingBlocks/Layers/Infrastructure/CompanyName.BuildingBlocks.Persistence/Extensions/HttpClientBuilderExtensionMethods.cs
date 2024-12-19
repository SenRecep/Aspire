using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.BuildingBlocks.Persistence.Extensions;

public static class HttpClientBuilderExtensionMethods
{
    public static IHttpClientBuilder ConfigureHttpClientBaseUrl(this IHttpClientBuilder builder, string baseUrl) =>
        builder.ConfigureHttpClientBaseUrl(new Uri(baseUrl));

    public static IHttpClientBuilder ConfigureHttpClientWithServiceName(this IHttpClientBuilder builder, string serviceName) =>
        builder.ConfigureHttpClientBaseUrl(new Uri($"https+http://{serviceName}"));
    public static IHttpClientBuilder ConfigureHttpClientBaseUrl(this IHttpClientBuilder builder, Uri baseUrl) =>
        builder.ConfigureHttpClient(client => client.BaseAddress = baseUrl);
}
