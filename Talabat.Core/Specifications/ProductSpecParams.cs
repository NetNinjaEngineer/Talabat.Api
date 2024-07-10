namespace Talabat.Core.Specifications;

public class ProductSpecParams
{
    private int pageSize = 5;
    public string? Sort { get; set; }
    public int? BrandId { get; set; }
    public int? TypeId { get; set; }
    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get
        {
            return pageSize;
        }
        set
        {
            pageSize = (value > 10) ? 10 : value;
        }
    }

    private string? search;

    public string? Search
    {
        get { return search; }
        set { search = value?.ToLower(); }
    }


}
