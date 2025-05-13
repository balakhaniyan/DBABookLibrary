using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace DBABookLibrary.Utils.ApiCaller;

public sealed class ApiCaller
{
    public required string Url { get; set; }
    public Dictionary<string, string?>? QueryParameters { get; set; }

    public readonly List<string> RouteParameters = [];

    public string? Content { get; set; }
    public HttpMethod? Method { get; set; }

    public static ApiCallerBuilder CreateBuilder(string url) =>
        new()
        {
            ApiCaller = new ApiCaller { Url = url }
        };

    public async Task<TResponse> Call<TResponse>()
    {
        using var client = new HttpClient();


        if (RouteParameters.Count != 0)
        {
            Url = Url + "/" + string.Join("/", RouteParameters);
        }

        if (QueryParameters is not null)
        {
            Url = QueryHelpers.AddQueryString(Url, QueryParameters ?? new Dictionary<string, string?>());
        }

        var request = new HttpRequestMessage(Method ?? HttpMethod.Get, Url);

        if (Content is not null)
        {
            request.Content = new StringContent(Content, Encoding.UTF8, "application/json");
        }


        var response = await client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResponse>(result) ??
                   throw new NullReferenceException("Response is null");
        }


        throw new Exception($"Api call failed with status code {response.StatusCode}");
    }
}

public sealed class ApiCallerBuilder
{
    public required ApiCaller ApiCaller { get; set; }
}

public static class ApiCallerBuilderExtensions
{
    public static ApiCallerBuilder AddQueryParameters<TQueryParameters>(this ApiCallerBuilder builder,
        TQueryParameters queryParameters)
    {
        var json = JsonConvert.SerializeObject(queryParameters);

        builder.ApiCaller.QueryParameters = JsonConvert.DeserializeObject<Dictionary<string, string?>>(json);

        return builder;
    }

    public static ApiCallerBuilder AddBody<TBody>(this ApiCallerBuilder builder,
        TBody body)
    {
        builder.ApiCaller.Content = JsonConvert.SerializeObject(body);

        return builder;
    }

    public static ApiCallerBuilder AddRouteParameter<T>(this ApiCallerBuilder builder, T parameter)
    {
        if (parameter is not null)
        {
            builder.ApiCaller.RouteParameters.Add(
                parameter.ToString()
                ?? throw new ArgumentException($"{nameof(parameter)} is invalid")
            );
        }

        return builder;
    }

    public static ApiCallerBuilder SetMethod(this ApiCallerBuilder builder, HttpMethod method)
    {
        builder.ApiCaller.Method = method;

        return builder;
    }


    public static async Task<TResult> Call<TResult>(this ApiCallerBuilder builder)
        => await builder.ApiCaller.Call<TResult>();
}