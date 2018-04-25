using refca.Extensions;
namespace refca.Models.QueryFilters
{
    public class MagazineQuery : IQueryObject
    {
        public MagazineQuery()
        {
            IsApproved = true;
        }
        public string SearchTerm { get; set; }
        public int Page { get; set; }
        public byte PageSize { get; set; }
        public bool IsApproved { get; set; }
    }
}