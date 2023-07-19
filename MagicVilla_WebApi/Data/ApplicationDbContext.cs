using MagicVilla_WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_WebApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Villa> Villas { get; set; }
    }
}
