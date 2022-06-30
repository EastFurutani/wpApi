using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PizzaStore.Models;
using Dapper;
using System.Data.SqlClient;
using System.Linq;
using System;
 
/* DapperMethod(){
  using(var connection = new SqlConnection(connectionstring)){

    var wp = connection.Query<DapperClass>(sql).ToList();
    //Console.WriteLine(connection.Query<DapperClass>(sql).ToList());
    Console.WriteLine(wp.Count);
    foreach(var wordinfo in wp){
      Console.WriteLine(wordinfo.ToString());
    }
  }
  return wp.ToString();
} */

/* public class DapperFunction{
  string sql = "select * from WPDapper";
  using(var connection = new SqlConnection(connectionstring)){

    var wp = connection.Query<DapperClass>(sql).ToList();
    //Console.WriteLine(connection.Query<DapperClass>(sql).ToList());
  }
} */

string MyAllow = "_MyAllow";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

//変更箇所
builder.Services.AddDbContext<WordDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TestDatabaseDbString"));
});

builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pizzas API", Description = "Pizza pizza", Version = "v1" });
});

//CORS
builder.Services.AddCors(options => {
    options.AddPolicy(name: MyAllow,
    builder => {
        builder.WithOrigins("*");
        builder.WithHeaders("Content-Type");
        //builder.WithMethods("GET, POST, PUSH, DELETE");
        builder.AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
  c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pizza API V1");
});

//CORS
app.UseCors(MyAllow);

app.MapGet("/", () => "Hello World!");

//app.MapGet("/word", async(WordDbContext db) => await db.WordInfos.ToListAsync());

//app.MapGet("/word", () => DapperMethod());

/* app.MapGet("/word", async () => {
  using(var connection = new SqlConnection(connectionstring)){

    string sql = "select * from WPDapper";
    var wp = await connection.QueryAsync<DapperClass>(sql).ConfigureAwait(false);
    //Console.WriteLine(connection.Query<DapperClass>(sql).ToList());
    Console.WriteLine(wp.Count());
    Console.WriteLine(wp.ToString());
    foreach(var wordinfo in wp){
      Console.WriteLine(wordinfo.ToString());
    }
    return wp.ToString();
  }
}); */

//app.MapGet("/word", async(DapperClass db) => await db..ToListAsync());

app.MapGet("/word/{id}", async ( WordDbContext db, int id) => await db.WordInfos.FindAsync(id));

app.MapGet("/word", async (GetConnection connectionGetter) =>
{
    using var con = await connectionGetter();
    return con.GetAll<WPModels>().ToList();
});

app.MapPost("/word", async(WordDbContext db, WordInfo wordinfo) => {
    await db.WordInfos.AddAsync(wordinfo);
    await db.SaveChangesAsync();
    return Results.Created($"/word/{wordinfo.id}", wordinfo);
});

app.MapPut("/word/{id}", async (WordDbContext db, WordInfo updatewordinfo, int id) =>
{
    var pizza = await db.WordInfos.FindAsync(id);
    if (pizza is null) return Results.NotFound();
    pizza.wordName = updatewordinfo.wordName;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/word/{id}", async (WordDbContext db, int id) =>
{
  var wordinfo = await db.WordInfos.FindAsync(id);
  if (wordinfo is null)
  {
    return Results.NotFound();
  }
  db.WordInfos.Remove(wordinfo);
  await db.SaveChangesAsync();
  return Results.Ok();
});

app.Run();
