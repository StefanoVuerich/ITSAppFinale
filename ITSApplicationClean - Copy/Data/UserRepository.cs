using System;
using System.Configuration;

namespace Data
{
    public class UserRepository
    {
        private string connectionString;

        public UserRepository()
            : this("virtualMachineCS")
        {
        }

        public UserRepository(string connectionStringName)
        {
            var cs = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (cs == null)
                throw new ApplicationException(string.Format("ConnectionString '{0}' not found", connectionStringName));
            connectionString = cs.ConnectionString;
        }
    }
}