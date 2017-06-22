using refca.Data;
using refca.Dtos;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace refca.Models
{
    public class TeacherRepository : ITeacherRepository
    {
        private IHostingEnvironment _environment;
        private ApplicationDbContext _context;

        public TeacherRepository(IHostingEnvironment environment, ApplicationDbContext context)
        {
            _environment = environment;
            _context = context;
        }

        public void TeacherArticles(IEnumerable<Article> articles, string id)
        {
            foreach (var article in articles)
            {
                var items = article.TeacherArticles.Count();
                if (items > 1)
                {
                    var teacher = article.TeacherArticles.Where(t => t.TeacherId != id).FirstOrDefault();
                    var currentPath = $@"/bucket/{teacher.Teacher.Id}/article/";
                    var fileName = Path.GetFileName(article.ArticlePath);
                    var sourcePath = $@"{_environment.WebRootPath}{article.ArticlePath}";
                    var destPath = $@"{_environment.WebRootPath}{currentPath}{fileName}";
                    var physicalPath = $@"{_environment.WebRootPath}{currentPath}";

                    if (!Directory.Exists(physicalPath))
                        Directory.CreateDirectory(physicalPath);

                    if (!System.IO.File.Exists(destPath))
                    {
                        System.IO.File.Move(sourcePath, destPath);
                        article.ArticlePath = currentPath = Path.Combine(currentPath, fileName);
                        _context.SaveChanges();

                    }

                }
            }
        }

        public void TeacherBooks(IEnumerable<Book> books, string id)
        {
            foreach (var book in books)
            {
                var items = book.TeacherBooks.Count();
                if (items > 1)
                {
                    var teacher = book.TeacherBooks.Where(t => t.TeacherId != id).FirstOrDefault();
                    var currentPath = $@"/bucket/{teacher.Teacher.Id}/book/";
                    var fileName = Path.GetFileName(book.BookPath);
                    var sourcePath = $@"{_environment.WebRootPath}{book.BookPath}";
                    var destPath = $@"{_environment.WebRootPath}{currentPath}{fileName}";
                    var physicalPath = $@"{_environment.WebRootPath}{currentPath}";

                    if (!Directory.Exists(physicalPath))
                        Directory.CreateDirectory(physicalPath);

                    if (!System.IO.File.Exists(destPath))
                    {
                        System.IO.File.Move(sourcePath, destPath);
                        book.BookPath = currentPath = Path.Combine(currentPath, fileName);
                        _context.SaveChanges();

                    }

                }
            }
        }

        public void TeacherChapterbooks(IEnumerable<Chapterbook> chapterbooks, string id)
        {
            foreach (var chapterbook in chapterbooks)
            {
                var items = chapterbook.TeacherChapterbooks.Count();
                if (items > 1)
                {
                    var teacher = chapterbook.TeacherChapterbooks.Where(t => t.TeacherId != id).FirstOrDefault();
                    var currentPath = $@"/bucket/{teacher.Teacher.Id}/chapterbook/";
                    var fileName = Path.GetFileName(chapterbook.ChapterbookPath);
                    var sourcePath = $@"{_environment.WebRootPath}{chapterbook.ChapterbookPath}";
                    var destPath = $@"{_environment.WebRootPath}{currentPath}{fileName}";
                    var physicalPath = $@"{_environment.WebRootPath}{currentPath}";

                    if (!Directory.Exists(physicalPath))
                        Directory.CreateDirectory(physicalPath);

                    if (!System.IO.File.Exists(destPath))
                    {
                        System.IO.File.Move(sourcePath, destPath);
                        chapterbook.ChapterbookPath = currentPath = Path.Combine(currentPath, fileName);
                        _context.SaveChanges();

                    }

                }
            }
        }

        public void TeacherPresentations(IEnumerable<Presentation> presentations, string id)
        {
            foreach (var presentation in presentations)
            {
                var items = presentation.TeacherPresentations.Count();
                if (items > 1)
                {
                    var teacher = presentation.TeacherPresentations.Where(t => t.TeacherId != id).FirstOrDefault();
                    var currentPath = $@"/bucket/{teacher.Teacher.Id}/presentation/";
                    var fileName = Path.GetFileName(presentation.PresentationPath);
                    var sourcePath = $@"{_environment.WebRootPath}{presentation.PresentationPath}";
                    var destPath = $@"{_environment.WebRootPath}{currentPath}{fileName}";
                    var physicalPath = $@"{_environment.WebRootPath}{currentPath}";

                    if (!Directory.Exists(physicalPath))
                        Directory.CreateDirectory(physicalPath);

                    if (!System.IO.File.Exists(destPath))
                    {
                        System.IO.File.Move(sourcePath, destPath);
                        presentation.PresentationPath = currentPath = Path.Combine(currentPath, fileName);
                        _context.SaveChanges();

                    }

                }
            }
        }

        public void TeacherResearch(IEnumerable<Research> research, string id)
        {
            foreach (var iresearch in research)
            {
                var items = iresearch.TeacherResearch.Count();
                if (items > 1)
                {
                    var teacher = iresearch.TeacherResearch.Where(t => t.TeacherId != id).FirstOrDefault();
                    var currentPath = $@"/bucket/{teacher.Teacher.Id}/research/";
                    var fileName = Path.GetFileName(iresearch.ResearchPath);
                    var sourcePath = $@"{_environment.WebRootPath}{iresearch.ResearchPath}";
                    var destPath = $@"{_environment.WebRootPath}{currentPath}{fileName}";
                    var physicalPath = $@"{_environment.WebRootPath}{currentPath}";

                    if (!Directory.Exists(physicalPath))
                        Directory.CreateDirectory(physicalPath);

                    if (!System.IO.File.Exists(destPath))
                    {
                        System.IO.File.Move(sourcePath, destPath);
                        iresearch.ResearchPath = currentPath = Path.Combine(currentPath, fileName);
                        _context.SaveChanges();

                    }

                }
            }
        }

        public void TeacherMagazines(IEnumerable<Magazine> magazines, string id)
        {
            foreach (var magazine in magazines)
            {
                var items = magazine.TeacherMagazines.Count();
                if (items > 1)
                {
                    var teacher = magazine.TeacherMagazines.Where(t => t.TeacherId != id).FirstOrDefault();
                    var currentPath = $@"/bucket/{teacher.Teacher.Id}/magazine/";
                    var fileName = Path.GetFileName(magazine.MagazinePath);
                    var sourcePath = $@"{_environment.WebRootPath}{magazine.MagazinePath}";
                    var destPath = $@"{_environment.WebRootPath}{currentPath}{fileName}";
                    var physicalPath = $@"{_environment.WebRootPath}{currentPath}";

                    if (!Directory.Exists(physicalPath))
                        Directory.CreateDirectory(physicalPath);

                    if (!System.IO.File.Exists(destPath))
                    {
                        System.IO.File.Move(sourcePath, destPath);
                        magazine.MagazinePath = currentPath = Path.Combine(currentPath, fileName);
                        _context.SaveChanges();

                    }

                }
            }
        }

        public void TeacherThesis(IEnumerable<Thesis> theses, string id)
        {
            foreach (var thesis in theses)
            {
                var items = thesis.TeacherTheses.Count();
                if (items > 1)
                {
                    var teacher = thesis.TeacherTheses.Where(t => t.TeacherId != id).FirstOrDefault();
                    var currentPath = $@"/bucket/{teacher.Teacher.Id}/thesis/";
                    var fileName = Path.GetFileName(thesis.ThesisPath);
                    var sourcePath = $@"{_environment.WebRootPath}{thesis.ThesisPath}";
                    var destPath = $@"{_environment.WebRootPath}{currentPath}{fileName}";
                    var physicalPath = $@"{_environment.WebRootPath}{currentPath}";

                    if (!Directory.Exists(physicalPath))
                        Directory.CreateDirectory(physicalPath);

                    if (!System.IO.File.Exists(destPath))
                    {
                        System.IO.File.Move(sourcePath, destPath);
                        thesis.ThesisPath = currentPath = Path.Combine(currentPath, fileName);
                        _context.SaveChanges();

                    }

                }
            }
        }

    }
}