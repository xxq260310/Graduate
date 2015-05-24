using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.DAL
{
    /// <summary>
    /// Holds the DB factory configuration.
    /// </summary>
    public class DbFactoryConfiguration
    {
        /// <summary>
        ///  Holds the factory for database context generating.
        /// </summary>
        private static IDBFactory databaseFactory;

        /// <summary>
        /// Gets or sets factory for database context generating.
        /// </summary>
        public static IDBFactory DbFactory
        {
            get { return DbFactoryConfiguration.databaseFactory = DbFactoryConfiguration.databaseFactory ?? new DefaultDbFactory(); }
            set { DbFactoryConfiguration.databaseFactory = value; }
        }
    }
}
