using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectModel;
using System.Configuration;
using System.Data.SqlClient;

namespace Data
{
    public class UserRepository
    {
        string connectionString;
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
