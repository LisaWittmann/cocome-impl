using System;
using CocomeStore.Models.Transfer.Pagination;
using Microsoft.AspNetCore.WebUtilities;

namespace CocomeStore.Services.Pagination
{
    /// <summary>
    /// class <c>UriService</c> is an implementation of
    /// <see cref="IUriService"/>
    /// and provides functionality to create uris based on the
    /// applications baseUri
    /// </summary>
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        /// <summary>
        /// method <c>GetPageUri</c> creates a page uri to use in
        /// <see cref="PagedResponse{T}"/> based on a <see cref="PaginationFilter"/>
        /// and the endpoints route
        /// </summary>
        /// <param name="filter">
        /// pagination filter of request params
        /// </param>
        /// <param name="route">
        /// route of the endpoint
        /// </param>
        /// <returns></returns>
        public Uri GetPageUri(PaginationFilter filter, string route)
        {
            var _enpointUri = new Uri(string.Concat(_baseUri, route));
            var modifiedUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "pageNumber", filter.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());
            return new Uri(modifiedUri);
        }
    }
}
