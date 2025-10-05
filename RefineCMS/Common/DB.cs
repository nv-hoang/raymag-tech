using Microsoft.EntityFrameworkCore;
using RefineCMS.Models;
using RefineCMS.Seeders;

namespace RefineCMS.Common;

public class DB(DbContextOptions<DB> options) : DbContext(options)
{
    public DbSet<Option> Options { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserMeta> UserMeta { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostMeta> PostMeta { get; set; }
    public DbSet<TermTaxonomy> TermTaxonomy { get; set; }
    public DbSet<Term> Terms { get; set; }
    public DbSet<TermMeta> TermMeta { get; set; }
    public DbSet<TermRelationship> TermRelationships { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserSeeder());
    }
}
