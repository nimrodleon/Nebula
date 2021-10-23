namespace Nebula.Data.Helpers
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Query { get; set; }

        public PaginationFilter()
        {
            PageNumber = 1;
            PageSize = 50;
            Query = string.Empty;
        }

        public PaginationFilter(int pageNumber, int pageSize, string query)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize > 100 ? 100 : pageSize;
            Query = query ?? string.Empty;
        }
    }
}
