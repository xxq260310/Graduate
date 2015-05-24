using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.DAL
{
    /// <summary>
    /// Create a DB context instance.
    /// </summary>
    public class DefaultDbFactory :IDBFactory
    {
        /// <summary>
        /// Create the DB context instance.
        /// </summary>
        /// <returns>DB context instance.</returns>
        public MarketDB Create()
        {
            return new MarketDB();
        }
    }
}
