﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.DAL.Repositories
{
    public class SiteFeedbackRepository : GenericRepository<SiteFeedback>
    {
        public SiteFeedbackRepository(MarketDB context)
            : base(context)
        {
        }
    }
}
