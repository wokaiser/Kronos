using KronosData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosData.Logic
{
    public class DataManager
    {
        public static List<WorkTask> GetTasksOfDay(User user, DateTime desiredDay)
        {
            var retVal = new List<WorkTask>();

            // TODO: Check if LINQ is faster
            foreach (var account in user.AssignedAccounts)
            {
                foreach (var task in account.AssignedTasks)
                {
                    foreach (var item in task.WorkItems)
                    {
                        if (item.Begin.Day.Equals(desiredDay.Day))
                        {

                        }
                    }
                }
            }

            return retVal;
        }
    }
}
