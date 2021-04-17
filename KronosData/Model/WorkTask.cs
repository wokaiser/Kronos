using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosData.Model
{
    public class WorkTask : DB_Access
    {
        public WorkTask()
        {
            Title = string.Empty;
            AssignedAccount = new Account();
        }

        #region Properties

        public string Title { get; set; }

        public Account AssignedAccount { get; set; }

        #endregion
    }
}
