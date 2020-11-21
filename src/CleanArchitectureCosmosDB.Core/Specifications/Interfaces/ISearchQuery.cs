namespace CleanArchitectureCosmosDB.Core.Specifications.Interfaces
{
    public interface ISearchQuery
    {
        int Start { get; set; }
        int PageSize { get; set; }
        string SortColumn { get; set; }
        SortDirection? SortDirection { get; set; }
    }

    public enum SortDirection
    {
        Ascending = 0,
        Descending = 1
    }
}
