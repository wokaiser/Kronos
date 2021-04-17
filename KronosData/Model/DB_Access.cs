using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosData.Model
{
    public abstract class DB_Access
    {
        /// <summary>
        /// Creats a new DB_Access object
        /// </summary>
        public DB_Access()
        {
            ID = string.Empty;
        }

        #region Properties

        public string ID { get; set; }

        #endregion
    }
}
