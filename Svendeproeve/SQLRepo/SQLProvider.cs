using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;

namespace Svendeproeve.SQLRepo
{
    public class SQLProvider : ISQLProvider
    {
        private readonly IConfiguration configuration;
        MySqlConnection con;
        public SQLProvider(IConfiguration _configuration)
        {
            configuration = _configuration;
            con = new MySqlConnection(configuration.GetConnectionString("Svendeproeve").ToString());
            con.Open();

            if (_configuration == null)
                throw new ArgumentNullException(nameof(IConfiguration));
        }

        public dynamic Create(string query)
        {
            var command = new MySqlCommand(query, con);
            var execution = command.ExecuteScalarAsync();
            return execution.Result;
        }
        public dynamic Read(string query)
        {
            var command = new MySqlCommand(query, con);
            var reader = command.ExecuteReaderAsync();
            return reader.Result;
        }
        public dynamic Update(string query)
        {
            var command = new MySqlCommand(query, con);
            var execution = command.ExecuteScalarAsync();
            return execution.Result;
        }
        public dynamic Delete(string query)
        {
            var command = new MySqlCommand(query, con);
            var execution = command.ExecuteScalarAsync();
            return execution.Result;
        }
    }
}
