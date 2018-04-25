namespace refca.Resources.QueryResources
{
    public class ThesisQueryResource
    {
        public int? EducationProgramId { get; set; }
        public int? ResearchLineId { get; set; }
        public string SearchTerm { get; set; }
        public int Page { get; set; }
        public bool IsApproved { get; set; }
    }
}