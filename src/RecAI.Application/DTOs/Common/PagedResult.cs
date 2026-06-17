namespace RecAI.Application.DTOs.Common;

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public string? NextCursor { get; set; }   // null = no more pages
}