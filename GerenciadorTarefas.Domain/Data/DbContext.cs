using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace GerenciadorTarefas.Data
{
    public class DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _stringConnection;

        public DbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _stringConnection = _configuration.GetConnectionString("DefaultConnection");
        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_stringConnection);
        }
    }
}
