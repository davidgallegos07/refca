using refca.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace refca.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Thesis> Thesis { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Research> Research { get; set; }
        public DbSet<Chapterbook> Chapterbooks { get; set; }
        public DbSet<Presentation> Presentations { get; set; }
        public DbSet<Magazine> Magazines { get; set; }
        public DbSet<EducationProgram> EducationPrograms { get; set; }
        public DbSet<KnowledgeArea> KnowledgeAreas { get; set; }
        public DbSet<ResearchLine> ResearchLines { get; set; }
        public DbSet<AcademicBody> AcademicBodies { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ConsolidationGrade> ConsolidationGrades { get; set; }
        public DbSet<TeacherThesis> TeacherTheses { get; set; }
        public DbSet<TeacherBook> TeacherBooks { get; set; }
        public DbSet<TeacherResearch> TeacherResearch { get; set; }
        public DbSet<TeacherChapterbook> TeacherChapterbooks { get; set; }
        public DbSet<TeacherArticle> TeacherArticles { get; set; }
        public DbSet<TeacherPresentation> TeacherPresentations { get; set; }
        public DbSet<TeacherMagazine> TeacherMagazines { get; set; }
        public DbSet<EducationProgramResearchLine> EducationProgramResearchLine { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<TeacherThesis>()
                .HasKey(tt => new { tt.TeacherId, tt.ThesisId });

            builder.Entity<TeacherBook>()
                .HasKey(tb => new { tb.TeacherId, tb.BookId });

            builder.Entity<TeacherChapterbook>()
                .HasKey(tc => new { tc.TeacherId, tc.ChapterbookId });

            builder.Entity<TeacherArticle>()
                .HasKey(ta => new { ta.TeacherId, ta.ArticleId });

            builder.Entity<TeacherResearch>()
                .HasKey(tr => new { tr.TeacherId, tr.ResearchId });

            builder.Entity<TeacherPresentation>()
              .HasKey(tp => new { tp.TeacherId, tp.PresentationId });

            builder.Entity<TeacherMagazine>()
              .HasKey(tm => new { tm.TeacherId, tm.MagazineId });

            builder.Entity<EducationProgramResearchLine>()
                .HasKey(er => new { er.EducationProgramId, er.ResearchLineId });
        }
    }
}