using Npgsql;

namespace ПМ
{
    class PostgresConnector
    {
        NpgsqlConnection connection = new NpgsqlConnection("Server=localhost; " +
            "Port=5432; " +
            "User Id=postgres; " +
            "Password=8702; " +
            "Database=BD_Phones");

        public void openConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public NpgsqlConnection getConnection()
        {
            return connection;
        }
    }
}