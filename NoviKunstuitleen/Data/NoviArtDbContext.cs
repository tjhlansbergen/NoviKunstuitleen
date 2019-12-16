using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NoviKunstuitleen.Data
{
    public class NoviArtRole : IdentityRole<int> { }
    public class NoviArtDbContext : IdentityDbContext<NoviArtUser, NoviArtRole, int>
    {
        public NoviArtDbContext(DbContextOptions<NoviArtDbContext> options)
            : base(options)
        {
        }

        public DbSet<NoviArtPiece> NoviArtPieces { get; set; }
    }
}
