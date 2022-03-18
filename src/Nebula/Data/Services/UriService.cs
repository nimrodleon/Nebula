using Microsoft.AspNetCore.WebUtilities;
using Nebula.Data.Helpers;

namespace Nebula.Data.Services;

public class UriService : IUriService
{
    private readonly string _baseUri;

    public UriService(string baseUri)
    {
        _baseUri = baseUri;
    }

    public Uri GetPageUri(PaginationFilter filter, string route)
    {
        var endpointUri = new Uri(string.Concat(_baseUri, route));
        var modifiedUri =
            QueryHelpers.AddQueryString(endpointUri.ToString(), "pageNumber", filter.PageNumber.ToString());
        modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());
        modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "query", filter.Query);
        return new Uri(modifiedUri);
    }
}
