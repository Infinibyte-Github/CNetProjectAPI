namespace CNetProjectAPI.Data;
using Microsoft.EntityFrameworkCore;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

    public DbSet<Book> Books { get; set; }
    public DbSet<Movie> Movies { get; set; }
}