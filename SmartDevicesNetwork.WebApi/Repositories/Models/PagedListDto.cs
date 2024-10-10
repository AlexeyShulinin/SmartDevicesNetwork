using System.Collections.Generic;

namespace SmartDevicesNetwork.WebApi.Repositories.Models;

public class PagedListDto<T>
{
    public PagedListDto(IEnumerable<T> items, int currentPage, int total)
    {
        Items = items;
        CurrentPage = currentPage;
        Total = total;
    }
    
    public IEnumerable<T> Items { get; set; }
    public int CurrentPage { get; set; }
    public int Total { get; set; }
}