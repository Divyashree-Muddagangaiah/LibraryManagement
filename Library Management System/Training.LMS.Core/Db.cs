using System;
using System.Data;
using System.Data.SqlClient;

namespace Training.LMS.Core
{
    internal class Db
    {
        private SqlConnection connection;

        public Db(string connectionString)
        {
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
            }
            catch (SqlException)
            {
                connection.Dispose();
                throw;
            }
        }

        internal int ExecuteNonQuery(string sql)
        {
            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                return cmd.ExecuteNonQuery();
            }
        }

        public DataTable GetDT(String sql)
        {
            // Create a DataSet Object that we can fill and return
            DataTable dt = new DataTable();

                // Create a DataAdapter and use it to fill the DataSet
                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, connection))
                {
                    adapter.Fill(dt);
                }

                // Return the filled DataTable
                return dt;
        }

        internal T ExecuteScalar<T>(String sql)
        {
            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                try
                {
                    return (T)cmd.ExecuteScalar();
                }
                catch (InvalidCastException e)
                {
                    return default(T);
                }
            }
        }

    }
}