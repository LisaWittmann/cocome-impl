using System;
using System.Linq;
using CocomeStore.Models.Transfer.Pagination;
using System.Collections.Generic;
using CocomeStore.Services.Pagination;

namespace CocomeStore.Services
{
    /// <summary>
    /// class <c>ResponseBuilder</c> is a service class that provides
    /// functionalities to convert data into a response class
    /// </summary>
    /// <typeparam name="T">type of data to covert</typeparam>
    public class ResponseBuilder<T>
    {
        /// <summary>
        /// method <c>CreatePagedResponse</c> converts an enumerable dataset into
        /// a paged response according to the given pagination filter
        /// </summary>
        /// <param name="data">data of the reponse</param>
        /// <param name="filter">containing requested page and amount</param>
        /// <param name="uriService">service to create uris of pagiantion</param>
        /// <param name="route">route of endpoint that received the request</param>
        /// <returns>new paged response instance</returns>
        public PagedResponse<T> CreatePagedResponse(
            IEnumerable<T> data, PaginationFilter filter, IUriService uriService, string route
        )
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = data.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / filter.PageSize);
            var paginatedData = data
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToArray();
            var response = new PagedResponse<T>()
            {
                PageNumber = validFilter.PageNumber,
                PageSize = validFilter.PageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords,
                Data = paginatedData
            };
            response.NextPage =
                   validFilter.PageNumber >= 1 && validFilter.PageNumber < totalPages
                ? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber + 1, validFilter.PageSize), route)
                : null;
            response.PreviousPage =
                validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= totalPages
                ? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber - 1, validFilter.PageSize), route)
                : null;
            response.FirstPage = uriService.GetPageUri(new PaginationFilter(1, validFilter.PageSize), route);
            response.LastPage = uriService.GetPageUri(new PaginationFilter(totalPages, validFilter.PageSize), route);
            return response;
        }
    }
}
