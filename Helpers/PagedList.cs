namespace ETA.API.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => (CurrentPage > 1);
        public bool HasNext => (CurrentPage < TotalPages);

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }
        public static PagedList<T> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var response = source.AsEnumerable().GroupBy(x => 1).Select(x => new
            {
                count = x.Count(),
                items = x.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList()
            }).FirstOrDefault();

            //var count = source.Count();
            //var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(response?.items ?? new List<T>(), response?.count ?? 0, pageNumber, pageSize);
        }
    }
}
