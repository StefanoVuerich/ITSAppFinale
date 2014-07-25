using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Data
{
    public class NewsletterRepository
    {
        private string connectionString;

        public NewsletterRepository()
            : this("virtualMachineCS")
        {
        }

        public NewsletterRepository(string connectionStringName)
        {
            var cs = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (cs == null)
                throw new ApplicationException(string.Format("ConnectionString '{0}' not found", connectionStringName));
            connectionString = cs.ConnectionString;
        }

        public bool Post(string email)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO [dbo].[Newsletter]
                                ([Email])
                                VALUES
                                (@Email);";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@Email", email));

                    int affectedRow = command.ExecuteNonQuery();

                    connection.Close();

                    return true;
                }
            }
        }
    }
}