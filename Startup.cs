using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using refca.Data;
using refca.Models;
using refca.Services;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Newtonsoft.Json;
using AutoMapper;
using refca.Dtos;
namespace refca
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                );

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.CookieHttpOnly = true;
            });

            services.AddMvc(o => o.Conventions.Add(new FeatureConvention()))
                .AddRazorOptions(options =>
                {
                    options.ViewLocationFormats.Clear();
                    options.ViewLocationFormats.Add("/Features/{3}/{1}/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Features/{3}/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Features/Shared/{0}.cshtml");

                    options.ViewLocationExpanders.Add(new FeatureViewLocationExpander());
                }).AddJsonOptions(options => { options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; }); //added

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;
            }
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseIdentity();
            app.UseSession();
            Mapper.Initialize(config =>
            {
                config.CreateMap<Teacher, TeacherDto>();
                config.CreateMap<Thesis, ThesisDto>();
                config.CreateMap<Research, ResearchDto>();
                config.CreateMap<Book, BookDto>();
                config.CreateMap<Chapterbook, ChapterbookDto>();
                config.CreateMap<Article, ArticleDto>();
                config.CreateMap<Magazine, MagazineDto>();
                config.CreateMap<EducationProgram, EducationProgramDto>();
                config.CreateMap<ResearchLine, ResearchLineDto>();
                config.CreateMap<AcademicBody, AcademicBodyDto>();
                config.CreateMap<KnowledgeArea, KnowledgeAreaDto>();
                config.CreateMap<ConsolidationGrade, ConsolidationGradeDto>();

                config.CreateMap<Thesis, ThesisWithTeachersDto>()
                .ForMember(dto => dto.TeacherTheses, opt => opt.MapFrom(s => s.TeacherTheses.Select(t => t.Teacher)));
                config.CreateMap<Research, ResearchWithTeachersDto>()
                .ForMember(dto => dto.TeacherResearch, opt => opt.MapFrom(s => s.TeacherResearch.Select(t => t.Teacher)));
                config.CreateMap<Chapterbook, ChapterbookWithTeachersDto>()
                .ForMember(dto => dto.TeacherChapterbooks, opt => opt.MapFrom(s => s.TeacherChapterbooks.Select(t => t.Teacher)));
                config.CreateMap<Book, BookWithTeachersDto>()
                .ForMember(dto => dto.TeacherBooks, opt => opt.MapFrom(s => s.TeacherBooks.Select(t => t.Teacher)));
                config.CreateMap<Article, ArticleWithTeachersDto>()
                .ForMember(dto => dto.TeacherArticles, opt => opt.MapFrom(s => s.TeacherArticles.Select(t => t.Teacher)));
                config.CreateMap<Presentation, PresentationWithTeachersDto>()
                .ForMember(dto => dto.TeacherPresentations, opt => opt.MapFrom(s => s.TeacherPresentations.Select(t => t.Teacher)));
                config.CreateMap<Magazine, MagazineWithTeachersDto>()
                .ForMember(dto => dto.TeacherMagazines, opt => opt.MapFrom(s => s.TeacherMagazines.Select(t => t.Teacher)));

            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
            SeedDatabase.InsertTestData(app.ApplicationServices).Wait();

        }
    }
}