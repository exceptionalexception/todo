using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DataAccess
{
    public class TodoDb(IConfiguration config)
    {
        private readonly IConfiguration _config = config;
        
        public IDbConnection Connection => new SqlConnection(
            _config.GetConnectionString("DbConnection")
        );
    }
}
