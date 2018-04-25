namespace refca.Resources.TeacherQueryResources
{
    public class TeacherThesisQueryResource
    {
        public int? EducationProgramId { get; set; }
        public int? ResearchLineId { get; set; }
        public string SearchTerm { get; set; }
        public int Page { get; set; }
    }
}