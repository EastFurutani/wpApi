using System.Data.SqlClient;
using static WpContext;

public static class ServiceCollectionsExtensions
{
    public static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped <GetConnection>(sp =>
         async () =>
         {
             string connectionString = sp.GetService <IConfiguration>()["ConnectionString"];
             var connection = new SqlConnection(connectionString);
             await connection.OpenAsync();
             return connection;
         });

        return builder;
    }
}