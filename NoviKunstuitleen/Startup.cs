/*
    Startup.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 24 dec 2019
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

    /// <summary>
    /// Klasse voor configuratie van de applicatie tijdens starten
    /// </summary>
    public class Startup
    {
        // properties
        public IConfiguration Configuration { get; }

        // constructor
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // toevoegen van services aan de servicecontainer, deze methode wordt aangeroepen door de runtime
        public void ConfigureServices(IServiceCollection services)
        {
            // database context toevoegen
            services.AddDbContext<NoviArtDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            // ASP Identity toevoegen
            services.AddIdentity<NoviArtUser, NoviArtRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<NoviArtDbContext>()
                .AddDefaultTokenProviders()
                .AddClaimsPrincipalFactory<NoviArtUserClaims>();

            // Mail service toevoegen
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            // controllers toevoegen
            services.AddControllersWithViews();

            // cookie configuratie
            services.Configure<CookiePolicyOptions>(options =>
            {
                // maak cookie consent verplicht
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

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
                options.AddPolicy("StudentOnly", policy => policy.RequireClaim("Type", NoviUserType.Root.ToString(), NoviUserType.Student.ToString()));
                options.AddPolicy("MedewerkerOnly", policy => policy.RequireClaim("Type", NoviUserType.Root.ToString(), NoviUserType.Medewerker.ToString()));
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Type", NoviUserType.Root.ToString(), NoviUserType.Admin.ToString()));
            });
        }

        // HTTP Request pipeline configureren, deze methode wordt aangeroepen door de runtime
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // endpoints configureren
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
