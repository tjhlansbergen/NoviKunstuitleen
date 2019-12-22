using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NoviKunstuitleen.Data;
using NoviKunstuitleen.Services;

namespace NoviKunstuitleen
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
            services.AddDbContext<NoviArtDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<NoviArtUser, NoviArtRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<NoviArtDbContext>()
                .AddDefaultTokenProviders()
                .AddClaimsPrincipalFactory<NoviArtUserClaims>();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.AddControllersWithViews();

            // forceer verplicht inloggen voor alle controllers
            services.AddControllers(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            // definieer rollen op basis van gebruikerstype
            services.AddAuthorization(options =>
            {
                // voeg rol toe op basis van Claim-naam
                options.AddPolicy("StudentOnly", policy => policy.RequireClaim("Type", NoviUserType.Root.ToString(), NoviUserType.Admin.ToString(), NoviUserType.Student.ToString()));
                options.AddPolicy("MedewerkerOnly", policy => policy.RequireClaim("Type", NoviUserType.Root.ToString(), NoviUserType.Admin.ToString(), NoviUserType.Medewerker.ToString()));
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Type", NoviUserType.Root.ToString(), NoviUserType.Admin.ToString()));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

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
