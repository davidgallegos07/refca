namespace refca.Resources.QueryResources
{
    public class MagazineQueryResource
    {
        public string SearchTerm { get; set; }
        public int Page { get; set; }
        public bool IsApproved { get; set; }
    }
}