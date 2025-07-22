using Microsoft.EntityFrameworkCore;
using ESGSimpleTracker.Models;

namespace ESGSimpleTracker.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    public DbSet<Company> Companies { get; set; }
}