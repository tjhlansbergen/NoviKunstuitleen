using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NoviKunstuitleen.Data
{
    public class NoviArtDbContext : IdentityDbContext<NoviUser>
    {
        public NoviArtDbContext(DbContextOptions<NoviArtDbContext> options)
            : base(options)
        {
        }

        public DbSet<NoviArtPiece> NoviArtPieces { get; set; }
    }
}
