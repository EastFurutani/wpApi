using System.Data;

public class WpContext{
    public delegate Task<IDbConnection> GetConnection();
}