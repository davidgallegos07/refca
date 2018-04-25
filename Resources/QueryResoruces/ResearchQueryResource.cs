namespace refca.Resources.QueryResources
{
    public class ResearchQueryResource
    {
        public int? AcademicBodyId { get; set; }
        public int? KnowledgeAreaId { get; set; }
        public int? ResearchLineId { get; set; }
        public string SearchTerm { get; set; }
        public int Page { get; set; }
        public bool IsApproved { get; set; }

    }
}