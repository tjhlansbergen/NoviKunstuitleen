/*
    NoviArtBdContext.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 10 feb 2020
*/

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NoviKunstuitleen.Data
{

    /// <summary>
    /// Override de ASP Identity IdentityRole klasse om int's in plaats van guid's te gebruiken
    /// </summary>
    public class NoviArtRole : IdentityRole<int> { }
 
    /// <summary>
    /// Klasse voor de database-context (ORM)
    /// </summary>
    public class NoviArtDbContext : IdentityDbContext<NoviArtUser, NoviArtRole, int>
    {
        // constructor
        public NoviArtDbContext(DbContextOptions<NoviArtDbContext> options)
            : base(options)
        {
        }

        // DB-sets
        public DbSet<NoviArtPiece> NoviArtPieces { get; set; }
        public DbSet<NoviArtWallet> NoviArtWallets { get; set; }
    }
}
