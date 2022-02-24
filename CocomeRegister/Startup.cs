using System;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using CocomeStore.Models.Authorization;
using CocomeStore.Models.Database;
using CocomeStore.Services;
using CocomeStore.Services.Authorization;
using CocomeStore.Services.Bank;
using CocomeStore.Services.Mapping;
using CocomeStore.Services.Pagination;
using CocomeStore.Services.Printer;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RazorLight;

namespace CocomeRegister
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlite().AddDbContext<CocomeDbContext>(ServiceLifetime.Transient, ServiceLifetime.Singleton);
            services.AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequiredLength = 8;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<CocomeDbContext>();

            services.AddIdentityServer()
                 .AddApiAuthorization<ApplicationUser, CocomeDbContext>()
                 .AddProfileService<ProfileService>();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer()
                .AddIdentityServerJwt();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("enterprise", policy => policy.RequireClaim(ClaimTypes.Role, "Administrator"));
                options.AddPolicy("store", policy => policy.RequireClaim(ClaimTypes.Role, "Filialleiter"));
                options.AddPolicy("cashdesk", policy => policy.RequireClaim(ClaimTypes.Role, "Kassierer"));
            });

            services.AddTransient<ICashDeskService, CashDeskService>();
            services.AddTransient<IEnterpriseService, EnterpriseService>();
            services.AddTransient<IStoreService, StoreService>();
            services.AddTransient<IModelMapper, ModelMapper>();
            services.AddTransient<IExchangeService, ExchangeService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IBankService, BankService>();
            services.AddTransient<ClaimManager>();

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });

            services.AddScoped<IRazorLightEngine>(sp =>
            {
                var engine = new RazorLightEngineBuilder()
                    .UseFilesystemProject(Path.Combine(Directory.GetCurrentDirectory(), "Templates"))
                    .UseMemoryCachingProvider()
                    .Build();
                return engine;
            });
            services.AddScoped<IPrinterService, PrinterService>();

            services.AddHttpContextAccessor();
            services.AddControllersWithViews();
            services.AddDirectoryBrowser();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CoCoME",
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CocomeDbContext context)
        {
            if (env.IsDevelopment())
            {
                context.Database.EnsureCreated();
                context.Database.Migrate();
                app.UseDeveloperExceptionPage();
                app.UseHsts();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }
           
            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(env.ContentRootPath, "StaticFiles")),
                RequestPath = "/StaticFiles",
                EnableDirectoryBrowsing = true
            });

            app.UseRouting();
            app.UseHttpsRedirection();
            
            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
