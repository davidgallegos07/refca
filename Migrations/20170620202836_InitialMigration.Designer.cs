using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using refca.Data;

namespace refca.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170620202836_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("refca.Models.AcademicBody", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte>("ConsolidationGradeId");

                    b.Property<string>("Name");

                    b.Property<string>("PromepCode");

                    b.HasKey("Id");

                    b.HasIndex("ConsolidationGradeId");

                    b.ToTable("AcademicBodies");
                });

            modelBuilder.Entity("refca.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("refca.Models.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<string>("ArticlePath");

                    b.Property<DateTime>("EditionDate");

                    b.Property<int>("ISSN");

                    b.Property<bool>("IsApproved");

                    b.Property<string>("Magazine");

                    b.Property<string>("Title");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("refca.Models.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Abstract");

                    b.Property<DateTime>("AddedDate");

                    b.Property<string>("BookPath");

                    b.Property<short>("Edition");

                    b.Property<DateTime>("EditionDate");

                    b.Property<string>("Editorial");

                    b.Property<string>("Genre");

                    b.Property<string>("ISBN");

                    b.Property<bool>("IsApproved");

                    b.Property<int>("PrintLength");

                    b.Property<string>("Title");

                    b.Property<DateTime>("UpdatedDate");

                    b.Property<short>("Year");

                    b.HasKey("Id");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("refca.Models.Chapterbook", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<string>("BookTitle");

                    b.Property<string>("ChapterbookPath");

                    b.Property<string>("Editorial");

                    b.Property<string>("ISBN");

                    b.Property<bool>("IsApproved");

                    b.Property<DateTime>("PublishedDate");

                    b.Property<string>("Title");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.ToTable("Chapterbooks");
                });

            modelBuilder.Entity("refca.Models.ConsolidationGrade", b =>
                {
                    b.Property<byte>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("ConsolidationGrades");
                });

            modelBuilder.Entity("refca.Models.EducationProgram", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Grade");

                    b.Property<bool>("IsCertified");

                    b.Property<string>("Name");

                    b.Property<string>("ProgramCode");

                    b.HasKey("Id");

                    b.ToTable("EducationPrograms");
                });

            modelBuilder.Entity("refca.Models.EducationProgramResearchLine", b =>
                {
                    b.Property<int>("EducationProgramId");

                    b.Property<int>("ResearchLineId");

                    b.HasKey("EducationProgramId", "ResearchLineId");

                    b.HasIndex("ResearchLineId");

                    b.ToTable("EducationProgramResearchLine");
                });

            modelBuilder.Entity("refca.Models.KnowledgeArea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Career");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("KnowledgeAreas");
                });

            modelBuilder.Entity("refca.Models.Level", b =>
                {
                    b.Property<byte>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Levels");
                });

            modelBuilder.Entity("refca.Models.Magazine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<short>("Edition");

                    b.Property<DateTime>("EditionDate");

                    b.Property<string>("Editor");

                    b.Property<int>("ISSN");

                    b.Property<string>("Index");

                    b.Property<bool>("IsApproved");

                    b.Property<string>("MagazinePath");

                    b.Property<string>("Title");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.ToTable("Magazines");
                });

            modelBuilder.Entity("refca.Models.Presentation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<string>("Congress");

                    b.Property<DateTime>("EditionDate");

                    b.Property<bool>("IsApproved");

                    b.Property<string>("PresentationPath");

                    b.Property<string>("Title");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.ToTable("Presentations");
                });

            modelBuilder.Entity("refca.Models.Research", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AcademicBodyId");

                    b.Property<DateTime>("AddedDate");

                    b.Property<string>("Code");

                    b.Property<string>("FinalPeriod");

                    b.Property<string>("InitialPeriod");

                    b.Property<bool>("IsApproved");

                    b.Property<int>("KnowledgeAreaId");

                    b.Property<byte>("ResearchDuration");

                    b.Property<int>("ResearchLineId");

                    b.Property<string>("ResearchPath");

                    b.Property<string>("ResearchType");

                    b.Property<string>("Sector");

                    b.Property<string>("Title");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("AcademicBodyId");

                    b.HasIndex("KnowledgeAreaId");

                    b.HasIndex("ResearchLineId");

                    b.ToTable("Research");
                });

            modelBuilder.Entity("refca.Models.ResearchLine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AcademicBodyId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("AcademicBodyId");

                    b.ToTable("ResearchLines");
                });

            modelBuilder.Entity("refca.Models.Teacher", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AcademicBodyId");

                    b.Property<string>("Avatar");

                    b.Property<string>("Biography");

                    b.Property<string>("CVPath");

                    b.Property<string>("Email");

                    b.Property<string>("FacebookProfile");

                    b.Property<bool>("HasProdep");

                    b.Property<bool>("IsResearchTeacher");

                    b.Property<int>("KnowledgeAreaId");

                    b.Property<byte?>("LevelId");

                    b.Property<string>("Name");

                    b.Property<bool>("SNI");

                    b.Property<int>("TeacherCode");

                    b.Property<string>("TwitterProfile");

                    b.Property<string>("WebSite");

                    b.HasKey("Id");

                    b.HasIndex("AcademicBodyId");

                    b.HasIndex("KnowledgeAreaId");

                    b.HasIndex("LevelId");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("refca.Models.TeacherArticle", b =>
                {
                    b.Property<string>("TeacherId");

                    b.Property<int>("ArticleId");

                    b.Property<int>("Order");

                    b.HasKey("TeacherId", "ArticleId");

                    b.HasIndex("ArticleId");

                    b.ToTable("TeacherArticles");
                });

            modelBuilder.Entity("refca.Models.TeacherBook", b =>
                {
                    b.Property<string>("TeacherId");

                    b.Property<int>("BookId");

                    b.Property<int>("Order");

                    b.HasKey("TeacherId", "BookId");

                    b.HasIndex("BookId");

                    b.ToTable("TeacherBooks");
                });

            modelBuilder.Entity("refca.Models.TeacherChapterbook", b =>
                {
                    b.Property<string>("TeacherId");

                    b.Property<int>("ChapterbookId");

                    b.Property<int>("Order");

                    b.HasKey("TeacherId", "ChapterbookId");

                    b.HasIndex("ChapterbookId");

                    b.ToTable("TeacherChapterbooks");
                });

            modelBuilder.Entity("refca.Models.TeacherMagazine", b =>
                {
                    b.Property<string>("TeacherId");

                    b.Property<int>("MagazineId");

                    b.Property<int>("Order");

                    b.HasKey("TeacherId", "MagazineId");

                    b.HasIndex("MagazineId");

                    b.ToTable("TeacherMagazines");
                });

            modelBuilder.Entity("refca.Models.TeacherPresentation", b =>
                {
                    b.Property<string>("TeacherId");

                    b.Property<int>("PresentationId");

                    b.Property<int>("Order");

                    b.HasKey("TeacherId", "PresentationId");

                    b.HasIndex("PresentationId");

                    b.ToTable("TeacherPresentations");
                });

            modelBuilder.Entity("refca.Models.TeacherResearch", b =>
                {
                    b.Property<string>("TeacherId");

                    b.Property<int>("ResearchId");

                    b.Property<int>("Order");

                    b.HasKey("TeacherId", "ResearchId");

                    b.HasIndex("ResearchId");

                    b.ToTable("TeacherResearch");
                });

            modelBuilder.Entity("refca.Models.TeacherThesis", b =>
                {
                    b.Property<string>("TeacherId");

                    b.Property<int>("ThesisId");

                    b.Property<int>("Order");

                    b.HasKey("TeacherId", "ThesisId");

                    b.HasIndex("ThesisId");

                    b.ToTable("TeacherTheses");
                });

            modelBuilder.Entity("refca.Models.Thesis", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<int>("EducationProgramId");

                    b.Property<bool>("IsApproved");

                    b.Property<DateTime>("PublishedDate");

                    b.Property<int>("ResearchLineId");

                    b.Property<string>("StudentName");

                    b.Property<string>("ThesisPath");

                    b.Property<string>("Title");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("EducationProgramId");

                    b.HasIndex("ResearchLineId");

                    b.ToTable("Thesis");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("refca.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("refca.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("refca.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("refca.Models.AcademicBody", b =>
                {
                    b.HasOne("refca.Models.ConsolidationGrade", "ConsolidationGrade")
                        .WithMany("AcademicBodies")
                        .HasForeignKey("ConsolidationGradeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("refca.Models.EducationProgramResearchLine", b =>
                {
                    b.HasOne("refca.Models.EducationProgram", "EducationProgram")
                        .WithMany("EducationProgramResearchLine")
                        .HasForeignKey("EducationProgramId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("refca.Models.ResearchLine", "ResearchLine")
                        .WithMany("EducationProgramResearchLine")
                        .HasForeignKey("ResearchLineId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("refca.Models.Research", b =>
                {
                    b.HasOne("refca.Models.AcademicBody", "AcademicBody")
                        .WithMany("Research")
                        .HasForeignKey("AcademicBodyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("refca.Models.KnowledgeArea", "KnowledgeArea")
                        .WithMany()
                        .HasForeignKey("KnowledgeAreaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("refca.Models.ResearchLine", "ResearchLine")
                        .WithMany("Research")
                        .HasForeignKey("ResearchLineId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("refca.Models.ResearchLine", b =>
                {
                    b.HasOne("refca.Models.AcademicBody", "AcademicBody")
                        .WithMany("ResearchLines")
                        .HasForeignKey("AcademicBodyId");
                });

            modelBuilder.Entity("refca.Models.Teacher", b =>
                {
                    b.HasOne("refca.Models.AcademicBody", "AcademicBody")
                        .WithMany("Teachers")
                        .HasForeignKey("AcademicBodyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("refca.Models.KnowledgeArea", "KnowledgeArea")
                        .WithMany()
                        .HasForeignKey("KnowledgeAreaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("refca.Models.Level", "Level")
                        .WithMany("Teachers")
                        .HasForeignKey("LevelId");
                });

            modelBuilder.Entity("refca.Models.TeacherArticle", b =>
                {
                    b.HasOne("refca.Models.Article", "Article")
                        .WithMany("TeacherArticles")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("refca.Models.Teacher", "Teacher")
                        .WithMany("TeacherArticles")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("refca.Models.TeacherBook", b =>
                {
                    b.HasOne("refca.Models.Book", "Book")
                        .WithMany("TeacherBooks")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("refca.Models.Teacher", "Teacher")
                        .WithMany("TeacherBooks")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("refca.Models.TeacherChapterbook", b =>
                {
                    b.HasOne("refca.Models.Chapterbook", "Chapterbook")
                        .WithMany("TeacherChapterbooks")
                        .HasForeignKey("ChapterbookId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("refca.Models.Teacher", "Teacher")
                        .WithMany("TeacherChapterbooks")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("refca.Models.TeacherMagazine", b =>
                {
                    b.HasOne("refca.Models.Magazine", "Magazine")
                        .WithMany("TeacherMagazines")
                        .HasForeignKey("MagazineId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("refca.Models.Teacher", "Teacher")
                        .WithMany("TeacherMagazines")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("refca.Models.TeacherPresentation", b =>
                {
                    b.HasOne("refca.Models.Presentation", "Presentation")
                        .WithMany("TeacherPresentations")
                        .HasForeignKey("PresentationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("refca.Models.Teacher", "Teacher")
                        .WithMany("TeacherPresentations")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("refca.Models.TeacherResearch", b =>
                {
                    b.HasOne("refca.Models.Research", "Research")
                        .WithMany("TeacherResearch")
                        .HasForeignKey("ResearchId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("refca.Models.Teacher", "Teacher")
                        .WithMany("TeacherResearch")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("refca.Models.TeacherThesis", b =>
                {
                    b.HasOne("refca.Models.Teacher", "Teacher")
                        .WithMany("TeacherTheses")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("refca.Models.Thesis", "Thesis")
                        .WithMany("TeacherTheses")
                        .HasForeignKey("ThesisId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("refca.Models.Thesis", b =>
                {
                    b.HasOne("refca.Models.EducationProgram", "EducationProgram")
                        .WithMany("Theses")
                        .HasForeignKey("EducationProgramId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("refca.Models.ResearchLine", "ResearchLine")
                        .WithMany("Theses")
                        .HasForeignKey("ResearchLineId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
