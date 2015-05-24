using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.DAL.Repositories
{
    public class ShoppingTrolleyRepository : GenericRepository<ShoppingTrolley>
    {
        public ShoppingTrolleyRepository(MarketDB context)
            : base(context)
        {
        }
    }
}
