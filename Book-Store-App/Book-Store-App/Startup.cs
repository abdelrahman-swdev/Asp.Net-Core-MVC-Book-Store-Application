using Book_Store_App.Data;
using Book_Store_App.Helpers;
using Book_Store_App.Interfaces;
using Book_Store_App.Models;
using Book_Store_App.Repositories;
using Book_Store_App.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Book_Store_App
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // add support for controllers and views
            services.AddControllersWithViews();

#if DEBUG
            // enable razor pages runtime compilation Conditionally
            services.AddRazorPages().AddRazorRuntimeCompilation();
#endif

            // add DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlConnection"));
            });

            // add identity core
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // enable google authentication
            services.AddAuthentication()
            .AddGoogle(options =>
            {

                options.ClientId = "375549076168-hku4enon81v0nhutnd33mmlfs17baqfc.apps.googleusercontent.com";
                options.ClientSecret = "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";
            });


            // configure identity options
            services.Configure<IdentityOptions>(op =>
            {
                op.Password.RequiredLength = 8;
                op.Password.RequiredUniqueChars = 1;
                op.Password.RequireNonAlphanumeric = true;
                op.Password.RequireUppercase = false;
                op.Password.RequireLowercase = false;
                op.Password.RequireDigit = false;

                op.SignIn.RequireConfirmedEmail = true;
                op.User.RequireUniqueEmail = true;
            });

            // configure token provider options
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(1);
            });

            // configure application cookies
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = Configuration["Application:LoginPath"];
            });


            // add automapper service
            services.AddAutoMapper(typeof(Startup));

            // add bookRepository service
            services.AddScoped<IBookRepository, BookRepository>();

            // add LanguageRepository service
            services.AddScoped<ILanguageRepository,LanguageRepository>();

            // add Auth Repository service
            services.AddScoped<IAuthRepository, AuthRepository>();

            // add user service to IService Collection
            services.AddScoped<IUserServices, UserServices>();

            // add custom user cliams principal factory
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipleFactory>();

            // add email services
            services.AddScoped<IEmailServices, EmailServices>();

            // add SMTPConfigModel options 
            services.Configure<SMTPConfigModel>(Configuration.GetSection("SMTPConfig"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("first");
            //    await next();
            //    await context.Response.WriteAsync("third");
            //});

            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("second");
            //});


            // enables static files middleware
            app.UseStaticFiles();

            // enable another static files folder instead of wwwroot
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "fileName")),
            //    RequestPath = "/fileName"
            //});


            // enable routing
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // add endpoints to controllers and set default {Home/Index/id?}
                //endpoints.MapDefaultControllerRoute();


                // endpoints associated with controller actions.
                endpoints.MapControllers();

            });
        }
    }
}
