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
            WorkItems = new List<WorkItem>();
        }

        #region Properties

        public string Title { get; set; }

        public List<WorkItem> WorkItems { get; }

        #endregion
    }
}
