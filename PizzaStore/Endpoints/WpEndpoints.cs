using Dapper.Contrib.Extensions;
using static WpContext;
using System.Data;
//using System.Data.SqlClient;

public static class TarefasEndpoints
{
    public static void MapWPDapperEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => "Mini Wp API");

        app.MapGet("/word", async (GetConnection connectionGetter) =>
        {
            using var con = await connectionGetter();
            return con.GetAll<Wp>().ToList();
            /*using(IDbCommand command = con.CreateCommand()){
                command.CommandText = "select * from WPDapper";
                using(IDataReader reader = command.ExecuteReader()){
                    reader.GetData(0);
                }
            } */
        });

        app.MapGet("/word/{id}", async (GetConnection connectionGetter, int id) =>
        {
            using var con = await connectionGetter();
            return con.Get<Wp>(id);
        });

        app.MapDelete("/word/{id}", async (GetConnection connectionGetter, int id) =>
        {
            using var con = await connectionGetter();
            con.Delete(new Wp(id, "", ""));
            return Results.NoContent();
        });

        app.MapPost("/word", async (GetConnection connectionGetter, Wp wp) =>
        {
            using var con = await connectionGetter();
            var id = con.Insert(wp);
            return Results.Created($"/word/{id}", wp);
        });

        app.MapPut("/word/{id}", async (GetConnection connectionGetter, Wp wp) =>
        {
            using var con = await connectionGetter();
            var id = con.Update(wp);
            return Results.Ok();
        });

    }
}