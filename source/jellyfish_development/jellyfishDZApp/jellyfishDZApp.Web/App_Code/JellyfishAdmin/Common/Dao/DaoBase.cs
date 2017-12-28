using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace JellyfishAdmin.Common.Dao
{
    /// <summary>
    /// DaoBase Class
    /// </summary>
    public class DaoBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DaoBase"/> class.
        /// </summary>
        public DaoBase()
        {

        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <returns>SqlConnection Object</returns>
        public SqlConnection GetConnection()
        {
            String connString = ConfigurationManager.ConnectionStrings["LocalJFish"].ConnectionString;

            return new SqlConnection(connString); 
        }
    }
}
