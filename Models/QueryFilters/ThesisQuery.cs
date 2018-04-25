using refca.Extensions;

namespace refca.Models.QueryFilters
{
    public class ThesisQuery: IQueryObject
    {
        public ThesisQuery()
        {
            IsApproved = true;
        }
        public int? EducationProgramId { get; set; }
        public int? ResearchLineId { get; set; }
        public string SearchTerm { get; set; }
        public int Page { get; set; }
        public byte PageSize { get; set; }
        public bool IsApproved { get; set; }
    }
}