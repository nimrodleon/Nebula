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
    public int? PreviousPage { get; set; } = null;
    public List<PaginationLink> Pages { get; set; } = new List<PaginationLink>();
    public int? NextPage { get; set; } = null;

    public void GeneratePageLinks(int maxVisiblePages = 6)
    {
        int startPage = Math.Max(1, Math.Min(CurrentPage - maxVisiblePages / 2, TotalPages - maxVisiblePages + 1));
        int endPage = Math.Min(startPage + maxVisiblePages - 1, TotalPages);

        Pages = Enumerable.Range(startPage, endPage - startPage + 1)
            .Select(p => new PaginationLink { Page = p }).ToList();

        PreviousPage = CurrentPage > 1 ? CurrentPage - 1 : null;
        NextPage = CurrentPage < TotalPages ? CurrentPage + 1 : null;
    }
}

public class PaginationLink
{
    public int Page { get; set; } = 1;
}
