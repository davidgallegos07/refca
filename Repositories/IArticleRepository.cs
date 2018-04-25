using System.Collections.Generic;
using System.Threading.Tasks;
using refca.Models;
using refca.Models.QueryFilters;
using refca.Resources;

namespace refca.Repositories
{
    public interface IArticleRepository
    {
        Task<QueryResult<Article>> GetArticles(ArticleQuery filter);
        Task<QueryResult<Article>> GetAdminArticles(ArticleQuery filter);
        Task<QueryResult<Article>> GetTeacherArticles(string userId, ArticleQuery filter);
        Task<int> GetNumberOfArticlesByTeacher(string userId);
        void Add(Article article);
        void Remove(Article article);
        Task<Article> GetArticle(int id);
    }
}