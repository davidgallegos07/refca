using System.Collections.Generic;
using System.Threading.Tasks;
using refca.Models;
using refca.Models.QueryFilters;

namespace refca.Repositories
{
    public interface IBookRepository
    {
        Task<QueryResult<Book>> GetBooks(BookQuery filter);
        Task<QueryResult<Book>> GetAdminBooks(BookQuery filter);
        Task<QueryResult<Book>> GetTeacherBooks(string userId, BookQuery filter);
        Task<int> GetNumberOfBooksByTeacher(string userId);
        void Add(Book book);
        void Remove(Book book);
        Task<Book> GetBook(int id);
    } 
}