using KronosData.Model;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosUI.Events
{
    public class WorkItemChangedEvent : PubSubEvent<WorkItem>
    {
    }
}
