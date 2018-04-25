using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using refca.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using refca.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace refca.Models
{
    public static class SeedDatabase
    {
        const string superUserEmail = "SuperUserSettings:SuperUserEmail";
        const string superUserPassword = "SuperUserSettings:SuperUserPassword"; 
        public static async Task InsertTestData(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<RefcaDbContext>();
                var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
                var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
                var roleStore = serviceProvider.GetService<RoleStore<IdentityRole>>();
                var env = serviceProvider.GetService<IHostingEnvironment>();

                var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables();
                var configuration = builder.Build();

                if (await userManager.FindByEmailAsync(configuration[superUserEmail]) == null)
                {
                    var superUser = new ApplicationUser
                    {
                        UserName = configuration[superUserEmail],
                        Email = configuration[superUserEmail]
                    };

                    string userPassword = configuration[superUserPassword];
                    await userManager.CreateAsync(superUser, userPassword);
                    if (!context.Levels.Any())
                    {
                        context.AddRange(
                            new Level { Id = 1, Name = "Candidato" },
                            new Level { Id = 2, Name = "Nivel 1" },
                            new Level { Id = 3, Name = "Nivel 2" },
                            new Level { Id = 4, Name = "Nivel 3" }
                            );
                        context.SaveChanges();
                    }
                    if (!context.ConsolidationGrades.Any())
                    {
                        context.AddRange(
                            new ConsolidationGrade { Id = 1, Name = "Consolidado" },
                            new ConsolidationGrade { Id = 2, Name = "En consolidación" },
                            new ConsolidationGrade { Id = 3, Name = "En formación" }
                         );
                        context.SaveChanges();
                    }
                    if (!roleManager.RoleExistsAsync(Roles.Teacher).Result)
                    {
                        await roleManager.CreateAsync(new IdentityRole { Name = Roles.Teacher });
                        context.SaveChanges();
                    }
                    if (!roleManager.RoleExistsAsync(Roles.Admin).Result)
                    {
                        await roleManager.CreateAsync(new IdentityRole { Name = Roles.Admin });
                        await userManager.AddToRoleAsync(superUser, Roles.Admin);
                        context.SaveChanges();
                    }
                    if (!roleManager.RoleExistsAsync(Roles.Owner).Result)
                    {
                        await roleManager.CreateAsync(new IdentityRole { Name = Roles.Owner });
                        await userManager.AddToRoleAsync(superUser, Roles.Owner);
                        context.SaveChanges();
                    }
                    if (!context.EducationPrograms.Any())
                    {
                        context.EducationPrograms.AddRange(
                        new EducationProgram { Name = "Licenciatura en Contaduría", Grade = "Licenciatura" },
                        new EducationProgram { Name = "Licenciatura en Administración", Grade = "Licenciatura" },
                        new EducationProgram { Name = "Licenciatura en Informática", Grade = "Licenciatura" },
                        new EducationProgram { Name = "Licenciatura en Negocios Internacionales", Grade = "Licenciatura" },
                        new EducationProgram { Name = "Especialidad en Dirección Financiera", Grade = "Posgrado" },
                        new EducationProgram { Name = "Maestría en Impuestos", Grade = "Posgrado" },
                        new EducationProgram { Name = "Maestria en Administración", Grade = "Posgrado" },
                        new EducationProgram { Name = "Maestría en gestión de las Tecnologías de la Información y la Comunicación", Grade = "Posgrado" },
                        new EducationProgram { Name = "Doctorado en Ciencias Administrativas", Grade = "Posgrado" }

                        );
                        context.SaveChanges();
                    }
                    if (!context.AcademicBodies.Any())
                    {
                        context.AddRange(
                        new AcademicBody { Name = "Micro, pequeña y mediana empresa", ConsolidationGradeId = 1, PromepCode = "UABC-CA-154" },
                        new AcademicBody { Name = "Innovación y desarrollo regional", ConsolidationGradeId = 1, PromepCode = "UABC-CA-155" },
                        new AcademicBody { Name = "Sistemas de información y gestión empresarial", ConsolidationGradeId = 1, PromepCode = "UABC-CA-156" },
                        new AcademicBody { Name = "Productividad, Competitividad y capital humano en las organizaciones", ConsolidationGradeId = 1, PromepCode = "UABC-CA-157" },
                        new AcademicBody { Name = "Administración y gestión del conocimiento en entornos globalizados", ConsolidationGradeId = 1, PromepCode = "UABC-CA-158" }
                        );
                        context.SaveChanges();
                    }
                    if (!context.ResearchLines.Any())
                    {
                        context.ResearchLines.AddRange(
                            new ResearchLine { Name = "Sistemas de información contable, financiera y fiscal", AcademicBodyId = null },
                            new ResearchLine { Name = "Mercadotecnia", AcademicBodyId = null },
                            new ResearchLine { Name = "Administración de operaciones", AcademicBodyId = null },
                            new ResearchLine { Name = "Tecnologías de la información y la comunicación", AcademicBodyId = null },
                            new ResearchLine { Name = "Innovación y gestión tecnológica", AcademicBodyId = null },
                            new ResearchLine { Name = "Administración y gestión de negocios en el entorno global", AcademicBodyId = null },
                            new ResearchLine { Name = "Características, funcionamiento y desarrollo de las MIPYMES", AcademicBodyId = null },
                            new ResearchLine { Name = "Sistemas de información financiera fiscal", AcademicBodyId = null }, // 1
                            new ResearchLine { Name = "Estrategias fiscales para la competitividad", AcademicBodyId = null },
                            new ResearchLine { Name = "Análisis fiscal y financiero para el desarrollo de las MIPYMES", AcademicBodyId = null },
                            new ResearchLine { Name = "Estudios en Competitividad", AcademicBodyId = null },
                            new ResearchLine { Name = "Administración y desarrollo organizacional", AcademicBodyId = null },
                            new ResearchLine { Name = "Tecnologias de la información", AcademicBodyId = null },
                            new ResearchLine { Name = "Gestion tecnológica", AcademicBodyId = null },
                            new ResearchLine { Name = "Telemática", AcademicBodyId = null },
                            new ResearchLine { Name = "Administración y Desarrollo de las Organizaciones", AcademicBodyId = null },
                            new ResearchLine { Name = "Estudios para el Impulso de la Competitividad", AcademicBodyId = null },
                            new ResearchLine { Name = "Desarrollo Regional y Sistemas de Innovación", AcademicBodyId = null },
                            new ResearchLine { Name = "Competitividad MIPYME como estrategia de desarrollo local", AcademicBodyId = 1 }, //19
                            new ResearchLine { Name = "Sistema de Información Financiera y Fiscal", AcademicBodyId = 1 }, // 1 el mismo?
                            new ResearchLine { Name = "Competitividad y desarrollo regional", AcademicBodyId = 2 },
                            new ResearchLine { Name = "Sistemas de Innovación", AcademicBodyId = 2 },
                            new ResearchLine { Name = "Tecnologías de la Información y Comunicación en la toma de decisiones", AcademicBodyId = 3 },
                            new ResearchLine { Name = "Productividad y gestión del talento humano en las organizaciones", AcademicBodyId = 4 },
                            new ResearchLine { Name = "Administración y gestión del conocimiento", AcademicBodyId = 5 }
                        );
                        context.SaveChanges();
                    }
                    if (!context.KnowledgeAreas.Any())
                    {
                        context.AddRange(
                            new KnowledgeArea { Name = "Contabilidad Básica", Career = "Lic. Contaduría" },
                            new KnowledgeArea { Name = "Contabilidad Avanzada", Career = "Lic. Contaduría" },
                            new KnowledgeArea { Name = "Contabilidad de Costos", Career = "Lic. Contaduría" },
                            new KnowledgeArea { Name = "Finanzas", Career = "Lic. Contaduría" },
                            new KnowledgeArea { Name = "Auditoría", Career = "Lic. Contaduría" },
                            new KnowledgeArea { Name = "Impuestos", Career = "Lic. Contaduría" },
                            new KnowledgeArea { Name = "Mercadotecnia", Career = "Lic. Administración" },
                            new KnowledgeArea { Name = "Recursos Humanos", Career = "Lic. Administración" },
                            new KnowledgeArea { Name = "Producción", Career = "Lic. Administración" },
                            new KnowledgeArea { Name = "Administración Básica", Career = "Lic. Administración" },
                            new KnowledgeArea { Name = "Administración Avanzada", Career = "Lic. Administración" },
                            new KnowledgeArea { Name = "Informática", Career = "Lic. Informática" },
                            new KnowledgeArea { Name = "Matemáticas", Career = "Lic. Informática" },
                            new KnowledgeArea { Name = "Programación", Career = "Lic. Informática" },
                            new KnowledgeArea { Name = "Sistemas de Información", Career = "Lic. Informática" },
                            new KnowledgeArea { Name = "Ciencias computacionales", Career = "Lic. Informática" },
                            new KnowledgeArea { Name = "Economía", Career = "Lic. En Negocios Internacionales" },
                            new KnowledgeArea { Name = "Comercio Exterior", Career = "Lic. En Negocios Internacionales" },
                            new KnowledgeArea { Name = "Derecho", Career = "Lic. En Negocios Internacionales" },
                            new KnowledgeArea { Name = "Emprendedores", Career = "Lic. En Negocios Internacionales" },
                            new KnowledgeArea { Name = "Asignaturas de Apoyo", Career = "Lic. En Negocios Internacionales" }

                            );
                        context.SaveChanges();

                    }
                    if (!context.EducationProgramResearchLine.Any())
                    {
                        context.EducationProgramResearchLine.AddRange(
                            new EducationProgramResearchLine { EducationProgramId = 1, ResearchLineId = 1 },
                            new EducationProgramResearchLine { EducationProgramId = 5, ResearchLineId = 1 },
                            new EducationProgramResearchLine { EducationProgramId = 2, ResearchLineId = 2 },
                            new EducationProgramResearchLine { EducationProgramId = 9, ResearchLineId = 2 },
                            new EducationProgramResearchLine { EducationProgramId = 2, ResearchLineId = 3 },
                            new EducationProgramResearchLine { EducationProgramId = 2, ResearchLineId = 4 },
                            new EducationProgramResearchLine { EducationProgramId = 3, ResearchLineId = 5 },
                            new EducationProgramResearchLine { EducationProgramId = 3, ResearchLineId = 6 },
                            new EducationProgramResearchLine { EducationProgramId = 4, ResearchLineId = 7 },
                            new EducationProgramResearchLine { EducationProgramId = 5, ResearchLineId = 8 },
                            new EducationProgramResearchLine { EducationProgramId = 5, ResearchLineId = 9 },
                            new EducationProgramResearchLine { EducationProgramId = 6, ResearchLineId = 10 },
                            new EducationProgramResearchLine { EducationProgramId = 6, ResearchLineId = 11 },
                            new EducationProgramResearchLine { EducationProgramId = 6, ResearchLineId = 12 },
                            new EducationProgramResearchLine { EducationProgramId = 7, ResearchLineId = 13 },
                            new EducationProgramResearchLine { EducationProgramId = 7, ResearchLineId = 14 },
                            new EducationProgramResearchLine { EducationProgramId = 8, ResearchLineId = 15 },
                            new EducationProgramResearchLine { EducationProgramId = 8, ResearchLineId = 16 },
                            new EducationProgramResearchLine { EducationProgramId = 8, ResearchLineId = 17 },
                            new EducationProgramResearchLine { EducationProgramId = 9, ResearchLineId = 18 },
                            new EducationProgramResearchLine { EducationProgramId = 9, ResearchLineId = 19 },
                            new EducationProgramResearchLine { EducationProgramId = 9, ResearchLineId = 20 }
                            );
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}