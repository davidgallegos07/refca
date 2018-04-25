namespace refca.Resources.QueryResources
{
    public class ArticleQueryResource
    {
        public string SearchTerm { get; set; }
        public int Page { get; set; }
        public bool IsApproved { get; set; }
    }
}