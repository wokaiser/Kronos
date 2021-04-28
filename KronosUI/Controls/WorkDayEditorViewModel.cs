using KronosData.Model;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosUI.Controls
{
    public class WorkDayEditorViewModel : BindableBase
    {
        private string title;

        public WorkDayEditorViewModel(WorkDay selectedItem)
        {
            Title = "Test123";
        }

        #region Properties

        public string Title
        {
            get { return title; }
            set
            {
                SetProperty(ref title, value);
            }
        }

        #endregion
    }
}
