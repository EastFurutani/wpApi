using Microsoft.EntityFrameworkCore;

namespace PizzaStore.Models 
{
  public class WordInfo{
    public int id {get;set;}
    public string ? wordName {get; set;}
  }

  public class WordDbContext : DbContext{
    //public WordDbContext(DbContextOptions options) : base(options) { }
    public DbSet<WordInfo> WordInfos {get; set;}
    //public DbSet<WordInfo> WordInfos => Set<WordInfo>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder
            .UseSqlServer("Server=localhost\\SQLEXPRESS;Database=WordPlaying;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WordInfo>().HasData(
                new WordInfo { id = 1, wordName = "AME"}
            );
    }
  }
}