using Dfe.Wizard.Prototype.Application.Questionnaires;
using Dfe.Wizard.Prototype.Application.Services;
using Dfe.Wizard.Prototype.Handlers;
using Dfe.Wizard.Prototype.Models;
using Dfe.Wizard.Prototype.Models.Questions;
using Dfe.Wizard.Prototype.Models.Rules;
using Dfe.Wizard.Prototype.TagHelpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;

namespace Dfe.Wizard.Prototype
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(config =>
            {
                // Action methods have to explicitly opt out to allow anonymous access
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddFeatureManagement();
            services.AddHttpContextAccessor();
            services.AddSession();
            services.AddResponseCaching();
            services.AddResponseCompression();

            // configure basic authentication
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            services.Configure<BasicAuthOptions>(Configuration.GetSection("BasicAuth"));

            services.AddScoped<Questionnaire, HomeQuestionnaire>();
            services.AddScoped<Questionnaire, HealthQuestionnaire>();

            services.AddScoped<IExaminer, HomeExaminer>();
            services.AddScoped<IExaminer, HealthExaminer>();

            services.AddSingleton<IFileUploadService, FakeUploadService>();
            services.AddSingleton<IEvidenceService, EvidenceService>();
            services.AddSingleton<IQuestionnaireService, QuestionnaireService>();
            services.AddTransient<IHtmlGenerator, HtmlGenerator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                var provider = new FileExtensionContentTypeProvider();

                // Add .scss mapping
                provider.Mappings[".scss"] = "text/css";
                app.UseStaticFiles(new StaticFileOptions()
                {
                    ContentTypeProvider = provider
                });
            }
            else
            {
                app.UseDeveloperExceptionPage();
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

                app.UseStaticFiles();
            }

            app.UseHttpsRedirection();
            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
