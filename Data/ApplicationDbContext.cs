using CsPostApi.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CsPostApi.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): DbContext(options)
{
    public DbSet<Post> Posts { get; set; }
}