using System;
using System.Linq;
using CocomeStore.Models.Transfer.Pagination;
using System.Collections.Generic;

namespace CocomeStore.Services
{
    public class ResponseBuilder<T>
    {
        public PagedResponse<T> PaginateData(IEnumerable<T> data, PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = data.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / filter.PageSize);
            var paginatedData = data
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToArray();
            return new PagedResponse<T>()
            {
                PageNumber = validFilter.PageNumber,
                PageSize = validFilter.PageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords,
                Data = paginatedData
            };
        }
    }
}
