using refca.Extensions;
namespace refca.Models.QueryFilters
{
    public class ResearchQuery : IQueryObject
    {
        public ResearchQuery()
        {
            IsApproved = true;
        }
        public int? AcademicBodyId { get; set; }
        public int? KnowledgeAreaId { get; set; }
        public int? ResearchLineId { get; set; }
        public string SearchTerm { get; set; }
        public int Page { get; set; }
        public byte PageSize { get; set; }
        public bool IsApproved { get; set; }
    }
}