using refca.Extensions;
namespace refca.Models.QueryFilters
{
    public class BookQuery : IQueryObject
    {
        public BookQuery()
        {
            IsApproved = true;
        }
        public string SearchTerm { get; set; }
        public int Page { get; set; }
        public byte PageSize { get; set; }
        public bool IsApproved { get; set; }
    }
}