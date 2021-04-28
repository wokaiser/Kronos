using KronosData.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KronosUI.Controls
{
    public class WorkDayEditorViewModel : BindableBase
    {
        private string title;
        private bool pendingChanges = false;

        public WorkDayEditorViewModel(WorkDay selectedItem)
        {
            Title = string.Format("{0}, den {1} bearbeiten",
                CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(selectedItem.WorkTime.Begin.DayOfWeek),
                selectedItem.WorkTime.Begin.Date.ToShortDateString());

            InitializeCommands();
        }

        #region Command implementations

        public void InitializeCommands()
        {
            SaveChangesCommand = new DelegateCommand<Window>(SaveChanges, CanSaveChanges);
            RevokeChangesCommand = new DelegateCommand<Window>(RevokeChanges);
        }

        public void SaveChanges(Window window)
        {
            //TODO: Do save changes

            PendingChanges = false;
            window.DialogResult = true;
            window.Close();
        }

        public bool CanSaveChanges(Window window)
        {
            return PendingChanges;
        }

        public void RevokeChanges(Window window)
        {
            PendingChanges = false;
            window.DialogResult = false;
            window.Close();
        }

        #endregion

        #region Properties

        public DelegateCommand<Window> SaveChangesCommand { get; private set; }

        public DelegateCommand<Window> RevokeChangesCommand { get; private set; }

        public string Title
        {
            get { return title; }
            set
            {
                SetProperty(ref title, value);
            }
        }

        public bool PendingChanges
        {
            get { return pendingChanges; }
            set
            {
                SetProperty(ref pendingChanges, value);
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion
    }
}
