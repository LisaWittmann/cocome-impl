using System;
using System.Collections.Generic;

namespace CocomeStore.Models.Transfer.Pagination
{
    /// <summary>
    /// class <c>PagedResponse</c> is a data transfer object to send large
    /// enumerables of data as a paged response with meta data
    /// </summary>
    /// <typeparam name="T">type of data to send</typeparam>
    public class PagedResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Uri FirstPage { get; set; }
        public Uri LastPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }
        public IEnumerable<T> Data { get; set; }
    }

    /// <summary>
    /// class <c>PaginationFilter</c> defines the structure of the request query
    /// to convert requested data into a <see cref="PagedResponse{T}"/>
    /// </summary>
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public PaginationFilter()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize > 30 ? 10 : pageSize;
        }
    }
}
