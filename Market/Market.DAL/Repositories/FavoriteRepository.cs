using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.DAL.Repositories
{
    public class FavoriteRepository : GenericRepository<Favorite>
    {
        public FavoriteRepository(MarketDB context)
            : base(context)
        {
        }
    }
}
