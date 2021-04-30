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
        private TimeSpan beginOfDay;
        private TimeSpan endOfDay;

        public WorkDayEditorViewModel(WorkDay selectedItem)
        {
            InitializeCommands();
            InitializeEditor(selectedItem);
        }

        private void InitializeEditor(WorkDay selectedItem)
        {
            Title = string.Format("{0}, den {1} bearbeiten",
                CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(selectedItem.WorkTime.Begin.DayOfWeek),
                selectedItem.WorkTime.Begin.Date.ToShortDateString());

            BeginOfDay = new TimeSpan(09, 00, 0);
            EndOfDay = new TimeSpan(17, 0, 0);
        }

        #region Command implementations

        public void InitializeCommands()
        {
            SaveChangesCommand = new DelegateCommand<Window>(SaveChanges, CanSaveChanges);
            RevokeChangesCommand = new DelegateCommand<Window>(RevokeChanges);
            AddBreakCommand = new DelegateCommand(AddBreak);
            RemoveBreakCommand = new DelegateCommand(RemoveBreak, CanRemoveBreak);
            AddWorkItemCommand = new DelegateCommand(AddWorkItem);
            RemoveWorkItemCommand = new DelegateCommand(RemoveWorkItem, CanRemoveWorkItem);
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

        public void AddBreak()
        {

        }

        public void RemoveBreak()
        {

        }

        public bool CanRemoveBreak()
        {
            return false;
        }

        public void AddWorkItem()
        {

        }

        public void RemoveWorkItem()
        {

        }

        public bool CanRemoveWorkItem()
        {
            return false;
        }

        #endregion

        #region Properties

        public DelegateCommand<Window> SaveChangesCommand { get; private set; }

        public DelegateCommand<Window> RevokeChangesCommand { get; private set; }

        public DelegateCommand AddBreakCommand { get; private set; }

        public DelegateCommand RemoveBreakCommand { get; private set; }

        public DelegateCommand AddWorkItemCommand { get; private set; }

        public DelegateCommand RemoveWorkItemCommand { get; private set; }

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

        public TimeSpan BeginOfDay
        {
            get { return beginOfDay; }
            set
            {
                SetProperty(ref beginOfDay, value);
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }

        public TimeSpan EndOfDay
        {
            get { return endOfDay; }
            set
            {
                SetProperty(ref endOfDay, value);
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion
    }
}
