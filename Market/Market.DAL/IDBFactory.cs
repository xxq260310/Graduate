using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.DAL
{
    /// <summary>
    /// This interface defines how to create a database context.
    /// </summary>
    public interface IDBFactory
    {
        /// <summary>
        /// Create a database context instance.
        /// </summary>
        /// <returns>database context instance.</returns>
        MarketDB Create();
    }
}
