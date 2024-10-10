using System.Collections.Generic;

namespace SmartDevicesNetwork.WebApi.Models.Responses;

public class PagedListResponse<T>
{
    public PagedListResponse(IEnumerable<T> items, int currentPage, int total, int? nextPage)
    {
        Items = items;
        CurrentPage = currentPage;
        Total = total;
        NextPage = nextPage;
    }
    
    public IEnumerable<T> Items { get; set; }
    public int CurrentPage { get; set; }
    public int Total { get; set; }
    public int? NextPage { get; set; }
}