namespace Talabat.Api.Helpers
{
    public class Pagination<T>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; } = [];

        public Pagination(int pageNumber, int pageSize, IReadOnlyList<T> data, int count)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Data = data;
            Count = count;
        }


    }
}
