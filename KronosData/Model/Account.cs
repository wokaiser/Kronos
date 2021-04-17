using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosData.Model
{
    public class Account : DB_Access
    {
        public Account()
        {
            Number = string.Empty;
            Title = string.Empty;
        }

        #region Properties

        public string Number { get; set; }

        public string Title { get; set; }

        #endregion
    }
}
