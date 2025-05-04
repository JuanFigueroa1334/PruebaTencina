using PruebaTencina.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PruebaTencina.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<BusquedaGif> BusquedaGifs { get; set; }
    }
}
