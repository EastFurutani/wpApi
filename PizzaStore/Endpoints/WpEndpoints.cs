using Dapper.Contrib.Extensions;
using Dapper;
using static WpContext;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

public static class WpEndpoints
{
    public static void MapWPDapperEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => "Mini Wp API");

        app.MapGet("/word", async (GetConnection connectionGetter) =>
        {
            using var con = await connectionGetter();
            var sql = "SELECT * FROM WPDapper";
            return con.Query<Wp>(sql);
            //return con.GetAll<Wp>().ToList();
        });

        app.MapGet("/word/{id}", async (GetConnection connectionGetter, int id) =>
        {
            using var con = await connectionGetter();
            var sql = "SELECT * FROM WPDapper WHERE id = @Id";
            var parameters = new {Id = id};
            return con.QueryFirstOrDefault<Wp>(sql, parameters);
            //return con.Get<Wp>(id);
        });

        app.MapDelete("/word/{id}", async (GetConnection connectionGetter, int id) =>
        {
            using var con = await connectionGetter();
            var sql = "DELETE FROM WPDapper WHERE id = @Id";
            var parameters = new {Id = id};
            return con.Query<Wp>(sql, parameters);
            /* con.Delete(new Wp(id, "", ""));
            return Results.NoContent(); */
        });

        app.MapPost("/word", async (GetConnection connectionGetter, Wp wp) =>
        {
            using var con = await connectionGetter();
            var sql = "INSERT INTO WPDapper VALUES (@WordName, @WordType)";
            var parameters = new {WordName = wp.wordName, WordType = wp.wordType};
            return con.Query<Wp>(sql, parameters);
            /* var id = con.Insert(wp);
            return Results.Created($"/word/{id}", wp); */
        });

        app.MapPut("/word/{id}", async (GetConnection connectionGetter, Wp wp, int id) =>
        {
            using var con = await connectionGetter();
            var sql = "UPDATE WPDapper SET wordName = @WordName, wordType = @WordType WHERE id = @Id";
            var parameters = new {Id = id, WordName = wp.wordName, WordType = wp.wordType};
            return con.Query<Wp>(sql, parameters);
            /* var id = con.Update(wp);
            return Results.Ok(); */
        });

    }
}