using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.DAL.Repositories
{
    public class OrderRepository : GenericRepository<Order>
    {
        public OrderRepository(MarketDB context)
            : base(context)
        {
        }
    }
}
