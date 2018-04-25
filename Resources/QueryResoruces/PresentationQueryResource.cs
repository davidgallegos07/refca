namespace refca.Resources.QueryResources
{
    public class PresentationQueryResource
    {
        public string SearchTerm { get; set; }
        public int Page { get; set; }
        public bool IsApproved { get; set; }
    }
}