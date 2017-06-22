using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace refca.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ArticlePath = table.Column<string>(nullable: true),
                    EditionDate = table.Column<DateTime>(nullable: false),
                    ISSN = table.Column<int>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    Magazine = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Abstract = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    BookPath = table.Column<string>(nullable: true),
                    Edition = table.Column<short>(nullable: false),
                    EditionDate = table.Column<DateTime>(nullable: false),
                    Editorial = table.Column<string>(nullable: true),
                    Genre = table.Column<string>(nullable: true),
                    ISBN = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false),
                    PrintLength = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Year = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Chapterbooks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    BookTitle = table.Column<string>(nullable: true),
                    ChapterbookPath = table.Column<string>(nullable: true),
                    Editorial = table.Column<string>(nullable: true),
                    ISBN = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false),
                    PublishedDate = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapterbooks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConsolidationGrades",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsolidationGrades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EducationPrograms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Grade = table.Column<string>(nullable: true),
                    IsCertified = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ProgramCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationPrograms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KnowledgeAreas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Career = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Levels",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Magazines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Edition = table.Column<short>(nullable: false),
                    EditionDate = table.Column<DateTime>(nullable: false),
                    Editor = table.Column<string>(nullable: true),
                    ISSN = table.Column<int>(nullable: false),
                    Index = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false),
                    MagazinePath = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Magazines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Presentations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Congress = table.Column<string>(nullable: true),
                    EditionDate = table.Column<DateTime>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    PresentationPath = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presentations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AcademicBodies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConsolidationGradeId = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PromepCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicBodies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicBodies_ConsolidationGrades_ConsolidationGradeId",
                        column: x => x.ConsolidationGradeId,
                        principalTable: "ConsolidationGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResearchLines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AcademicBodyId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResearchLines_AcademicBodies_AcademicBodyId",
                        column: x => x.AcademicBodyId,
                        principalTable: "AcademicBodies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AcademicBodyId = table.Column<int>(nullable: false),
                    Avatar = table.Column<string>(nullable: true),
                    Biography = table.Column<string>(nullable: true),
                    CVPath = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    FacebookProfile = table.Column<string>(nullable: true),
                    HasProdep = table.Column<bool>(nullable: false),
                    IsResearchTeacher = table.Column<bool>(nullable: false),
                    KnowledgeAreaId = table.Column<int>(nullable: false),
                    LevelId = table.Column<byte>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SNI = table.Column<bool>(nullable: false),
                    TeacherCode = table.Column<int>(nullable: false),
                    TwitterProfile = table.Column<string>(nullable: true),
                    WebSite = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teachers_AcademicBodies_AcademicBodyId",
                        column: x => x.AcademicBodyId,
                        principalTable: "AcademicBodies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Teachers_KnowledgeAreas_KnowledgeAreaId",
                        column: x => x.KnowledgeAreaId,
                        principalTable: "KnowledgeAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Teachers_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EducationProgramResearchLine",
                columns: table => new
                {
                    EducationProgramId = table.Column<int>(nullable: false),
                    ResearchLineId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationProgramResearchLine", x => new { x.EducationProgramId, x.ResearchLineId });
                    table.ForeignKey(
                        name: "FK_EducationProgramResearchLine_EducationPrograms_EducationProgramId",
                        column: x => x.EducationProgramId,
                        principalTable: "EducationPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EducationProgramResearchLine_ResearchLines_ResearchLineId",
                        column: x => x.ResearchLineId,
                        principalTable: "ResearchLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Research",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AcademicBodyId = table.Column<int>(nullable: false),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    FinalPeriod = table.Column<string>(nullable: true),
                    InitialPeriod = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false),
                    KnowledgeAreaId = table.Column<int>(nullable: false),
                    ResearchDuration = table.Column<byte>(nullable: false),
                    ResearchLineId = table.Column<int>(nullable: false),
                    ResearchPath = table.Column<string>(nullable: true),
                    ResearchType = table.Column<string>(nullable: true),
                    Sector = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Research", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Research_AcademicBodies_AcademicBodyId",
                        column: x => x.AcademicBodyId,
                        principalTable: "AcademicBodies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Research_KnowledgeAreas_KnowledgeAreaId",
                        column: x => x.KnowledgeAreaId,
                        principalTable: "KnowledgeAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Research_ResearchLines_ResearchLineId",
                        column: x => x.ResearchLineId,
                        principalTable: "ResearchLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Thesis",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    EducationProgramId = table.Column<int>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    PublishedDate = table.Column<DateTime>(nullable: false),
                    ResearchLineId = table.Column<int>(nullable: false),
                    StudentName = table.Column<string>(nullable: true),
                    ThesisPath = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thesis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Thesis_EducationPrograms_EducationProgramId",
                        column: x => x.EducationProgramId,
                        principalTable: "EducationPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Thesis_ResearchLines_ResearchLineId",
                        column: x => x.ResearchLineId,
                        principalTable: "ResearchLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherArticles",
                columns: table => new
                {
                    TeacherId = table.Column<string>(nullable: false),
                    ArticleId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherArticles", x => new { x.TeacherId, x.ArticleId });
                    table.ForeignKey(
                        name: "FK_TeacherArticles_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherArticles_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherBooks",
                columns: table => new
                {
                    TeacherId = table.Column<string>(nullable: false),
                    BookId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherBooks", x => new { x.TeacherId, x.BookId });
                    table.ForeignKey(
                        name: "FK_TeacherBooks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherBooks_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherChapterbooks",
                columns: table => new
                {
                    TeacherId = table.Column<string>(nullable: false),
                    ChapterbookId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherChapterbooks", x => new { x.TeacherId, x.ChapterbookId });
                    table.ForeignKey(
                        name: "FK_TeacherChapterbooks_Chapterbooks_ChapterbookId",
                        column: x => x.ChapterbookId,
                        principalTable: "Chapterbooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherChapterbooks_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherMagazines",
                columns: table => new
                {
                    TeacherId = table.Column<string>(nullable: false),
                    MagazineId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherMagazines", x => new { x.TeacherId, x.MagazineId });
                    table.ForeignKey(
                        name: "FK_TeacherMagazines_Magazines_MagazineId",
                        column: x => x.MagazineId,
                        principalTable: "Magazines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherMagazines_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherPresentations",
                columns: table => new
                {
                    TeacherId = table.Column<string>(nullable: false),
                    PresentationId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherPresentations", x => new { x.TeacherId, x.PresentationId });
                    table.ForeignKey(
                        name: "FK_TeacherPresentations_Presentations_PresentationId",
                        column: x => x.PresentationId,
                        principalTable: "Presentations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherPresentations_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherResearch",
                columns: table => new
                {
                    TeacherId = table.Column<string>(nullable: false),
                    ResearchId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherResearch", x => new { x.TeacherId, x.ResearchId });
                    table.ForeignKey(
                        name: "FK_TeacherResearch_Research_ResearchId",
                        column: x => x.ResearchId,
                        principalTable: "Research",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherResearch_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherTheses",
                columns: table => new
                {
                    TeacherId = table.Column<string>(nullable: false),
                    ThesisId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherTheses", x => new { x.TeacherId, x.ThesisId });
                    table.ForeignKey(
                        name: "FK_TeacherTheses_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherTheses_Thesis_ThesisId",
                        column: x => x.ThesisId,
                        principalTable: "Thesis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicBodies_ConsolidationGradeId",
                table: "AcademicBodies",
                column: "ConsolidationGradeId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EducationProgramResearchLine_ResearchLineId",
                table: "EducationProgramResearchLine",
                column: "ResearchLineId");

            migrationBuilder.CreateIndex(
                name: "IX_Research_AcademicBodyId",
                table: "Research",
                column: "AcademicBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_Research_KnowledgeAreaId",
                table: "Research",
                column: "KnowledgeAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Research_ResearchLineId",
                table: "Research",
                column: "ResearchLineId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchLines_AcademicBodyId",
                table: "ResearchLines",
                column: "AcademicBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_AcademicBodyId",
                table: "Teachers",
                column: "AcademicBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_KnowledgeAreaId",
                table: "Teachers",
                column: "KnowledgeAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_LevelId",
                table: "Teachers",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherArticles_ArticleId",
                table: "TeacherArticles",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherBooks_BookId",
                table: "TeacherBooks",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherChapterbooks_ChapterbookId",
                table: "TeacherChapterbooks",
                column: "ChapterbookId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherMagazines_MagazineId",
                table: "TeacherMagazines",
                column: "MagazineId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPresentations_PresentationId",
                table: "TeacherPresentations",
                column: "PresentationId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherResearch_ResearchId",
                table: "TeacherResearch",
                column: "ResearchId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherTheses_ThesisId",
                table: "TeacherTheses",
                column: "ThesisId");

            migrationBuilder.CreateIndex(
                name: "IX_Thesis_EducationProgramId",
                table: "Thesis",
                column: "EducationProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Thesis_ResearchLineId",
                table: "Thesis",
                column: "ResearchLineId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "EducationProgramResearchLine");

            migrationBuilder.DropTable(
                name: "TeacherArticles");

            migrationBuilder.DropTable(
                name: "TeacherBooks");

            migrationBuilder.DropTable(
                name: "TeacherChapterbooks");

            migrationBuilder.DropTable(
                name: "TeacherMagazines");

            migrationBuilder.DropTable(
                name: "TeacherPresentations");

            migrationBuilder.DropTable(
                name: "TeacherResearch");

            migrationBuilder.DropTable(
                name: "TeacherTheses");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Chapterbooks");

            migrationBuilder.DropTable(
                name: "Magazines");

            migrationBuilder.DropTable(
                name: "Presentations");

            migrationBuilder.DropTable(
                name: "Research");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Thesis");

            migrationBuilder.DropTable(
                name: "KnowledgeAreas");

            migrationBuilder.DropTable(
                name: "Levels");

            migrationBuilder.DropTable(
                name: "EducationPrograms");

            migrationBuilder.DropTable(
                name: "ResearchLines");

            migrationBuilder.DropTable(
                name: "AcademicBodies");

            migrationBuilder.DropTable(
                name: "ConsolidationGrades");
        }
    }
}
