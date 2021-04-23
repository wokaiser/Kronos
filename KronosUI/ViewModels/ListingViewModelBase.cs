using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosUI.ViewModels
{
    public abstract class ListingViewModelBase : ControlViewModelBase
    {
        private string pageTitle;

        #region Properties

        public string PageTitle
        {
            get { return pageTitle; }
            set { SetProperty(ref pageTitle, value); }
        }

        #endregion
    }
}
