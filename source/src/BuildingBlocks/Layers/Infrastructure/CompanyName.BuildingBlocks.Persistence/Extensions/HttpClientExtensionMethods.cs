namespace CompanyName.BuildingBlocks.Persistence.Extensions;

public static class HttpClientExtensionMethods
{
    public static async Task<HttpResponseMessage> SendFallowingAsync(this HttpClient client, HttpRequestMessage request,
        int maxRedirections = 5,
        CancellationToken cancellationToken = default)
    {
        while (maxRedirections-- > 0)
        {
            HttpResponseMessage response = await client.SendAsync(request, cancellationToken);
            if ((int)response.StatusCode / 100 != 3)
                return response;
            Uri? redirectUrl = response.Headers.Location;
            HttpRequestMessage newRequest = await CloneHttpRequestMessage(request);
            newRequest.RequestUri = redirectUrl;
            request = newRequest;
        }

        throw new RedirectionLimitExceededException($"Too many redirects to follow url: {request.RequestUri}");
    }

    private static async Task<HttpRequestMessage> CloneHttpRequestMessage(HttpRequestMessage request)
    {
        var clone = new HttpRequestMessage(request.Method, request.RequestUri);

        foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

        clone.Version = request.Version;

        if (request.Content == null)
            return clone;
        clone.Content = new StreamContent(await request.Content.ReadAsStreamAsync().ConfigureAwait(false));
        foreach (KeyValuePair<string, IEnumerable<string>> contentHeader in request.Content.Headers)
            clone.Content.Headers.TryAddWithoutValidation(contentHeader.Key, contentHeader.Value);

        return clone;
    }
}
public class RedirectionLimitExceededException(string message) : Exception(message);
