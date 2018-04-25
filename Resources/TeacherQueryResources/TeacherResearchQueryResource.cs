namespace refca.Resources.TeacherQueryResources
{
    public class TeacherResearchQueryResource
    {
        public int? AcademicBodyId { get; set; }
        public int? KnowledgeAreaId { get; set; }
        public int? ResearchLineId { get; set; }
        public string SearchTerm { get; set; }
        public int Page { get; set; }

    }
}