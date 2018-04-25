using refca.Extensions;

namespace refca.Models.QueryFilters
{
    public class TeacherQuery : IQueryObject
    {
        public string SearchTerm { get; set; }
        public int Page { get; set; }
        public byte PageSize { get; set; }
    }
}