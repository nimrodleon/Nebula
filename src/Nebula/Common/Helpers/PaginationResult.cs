namespace Nebula.Common.Helpers;

public class PaginationResult<T>
{
    public PaginationInfo Pagination { get; set; } = new PaginationInfo();
    public List<T> Data { get; set; } = new List<T>();
}

public class PaginationInfo
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public PaginationLink? PreviousPage { get; set; } = new PaginationLink();
    public List<PaginationLink> Pages { get; set; } = new List<PaginationLink>();
    public PaginationLink? NextPage { get; set; } = new PaginationLink();

    public void GeneratePageLinks(int maxVisiblePages, string urlController)
    {
        int startPage = Math.Max(1, Math.Min(CurrentPage - maxVisiblePages / 2, TotalPages - maxVisiblePages + 1));
        int endPage = Math.Min(startPage + maxVisiblePages - 1, TotalPages);

        Pages = Enumerable.Range(startPage, endPage - startPage + 1)
            .Select(p => new PaginationLink { Url = $"{urlController}?page={p}", Label = p.ToString() })
            .ToList();
    }
}

public class PaginationLink
{
    public string Url { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
}
