using System;
using CocomeStore.Models.Transfer.Pagination;

namespace CocomeStore.Services.Pagination
{
    public interface IUriService
    {
        Uri GetPageUri(PaginationFilter filter, string route);
    }
}
