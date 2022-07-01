using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaStore.Models 
{
  public class WordInfo{
    public int id {get;set;}
    public string ? wordName {get; set;}
  }

  public class WordDbContext : DbContext{
    public WordDbContext(DbContextOptions options) : base(options) { }
    public DbSet<WordInfo> WordInfos {get; set;}

    /* 元々の部分（SqlServerとの接続箇所はProgram.csに記述）
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder
            .UseSqlServer("Server=localhost\\SQLEXPRESS;Database=WordPlaying;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WordInfo>().HasData(
                new WordInfo { id = 1, wordName = "AME"}
            );
    } */
  }
}

  
