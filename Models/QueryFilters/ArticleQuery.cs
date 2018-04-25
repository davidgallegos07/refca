using refca.Extensions;
namespace refca.Models.QueryFilters
{
    public class ArticleQuery : IQueryObject
    {
        public ArticleQuery()
        {   
            IsApproved = true;    
        }
        public string SearchTerm { get; set; }
        public int Page { get; set; }
        public byte PageSize { get; set; }
        public bool IsApproved { get; set; }
    }
}