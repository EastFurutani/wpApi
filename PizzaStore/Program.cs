using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PizzaStore.Models;

string MyAllow = "_MyAllow";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

//変更箇所
builder.Services.AddDbContext<WordDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TestDatabaseDbContext"));
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

app.MapGet("/word", async(WordDbContext db) => await db.WordInfos.ToListAsync());
app.MapGet("/word/{id}", async ( WordDbContext db, int id) => await db.WordInfos.FindAsync(id));

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
