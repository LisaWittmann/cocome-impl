using System;
using System.Linq;
using CocomeStore.Models.Transfer.Pagination;
using System.Collections.Generic;
using CocomeStore.Services.Pagination;

namespace CocomeStore.Services
{
    public class ResponseBuilder<T>
    {
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
