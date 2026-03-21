namespace guest_house_management_backend.DTOs.Paging
{
    public class Paging<T>
    {
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
        public PagingMetaData MetaData { get; set; } = new();
    }

    public class PagingMetaData
    {
        public int TotalCount { get; set;}
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public Boolean HasNext => CurrentPage < TotalPages;
        public Boolean HasPrev => CurrentPage > 1;
    }
}