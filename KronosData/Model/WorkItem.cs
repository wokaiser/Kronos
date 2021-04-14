using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosData.Model
{
    public class WorkItem
    {
        public WorkItem(DateTime begin, DateTime end)
        {
            Begin = begin;
            End = end;
        }

        public TimeSpan Duration()
        {
            return End - Begin;
        }

        #region Properties

        public DateTime Begin { get; }

        public DateTime End { get; }

        #endregion
    }
}
