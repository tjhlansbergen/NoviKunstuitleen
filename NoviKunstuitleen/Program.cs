/*
    Program.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 24 dec 2019
*/

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace NoviKunstuitleen
{

    /// <summary>
    /// Klasse voor entry-point van de applicatie
    /// </summary>
    public class Program
    {

        /// <summary>
        /// Entry-point van de applicatie
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Roep startup aan
        /// </summary>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
