namespace Market.Common.Logger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NLog;

    public class LogHelper
    {
        /// <summary>
        /// NLog instance
        /// </summary>
        private static readonly Logger LoggerInstance = LogManager.GetLogger("Market");

        /// <summary>
        /// Gets a log instance.
        /// </summary>
        public static Logger Logger
        {
            get
            {
                return LoggerInstance;
            }
        }
    }
}
