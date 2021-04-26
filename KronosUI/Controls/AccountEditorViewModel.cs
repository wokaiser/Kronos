using KronosData.Logic;
using KronosData.Model;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KronosUI.Controls
{
    public class AccountEditorViewModel : BindableBase
    {
        public enum EditorStyle { Add = 0, Edit = 1 };

        private Visibility isNumberVisible;
        private string title;
        private string description;
        private string accountNumber;

        private EditorStyle editorStyle;
        private object selectedItem;

        public AccountEditorViewModel(EditorStyle editorStyle, object selectedItem)
        {
            this.editorStyle = editorStyle;
            this.selectedItem = selectedItem;
            description = string.Empty;
            accountNumber = string.Empty;

            Initialize();
        }

        private void Initialize()
        {
            SaveChangesCommand = new DelegateCommand<Window>(SaveChanges, CanSaveChanges);
            AbortCommand = new DelegateCommand<Window>(Abort);

            Title = selectedItem == null ? "Kontierung" : "Arbeitspaket";
            IsNumberVisible = selectedItem == null || selectedItem is Account ? Visibility.Visible : Visibility.Collapsed;

            if (editorStyle == EditorStyle.Add)
            {
                Title += " hinzufügen";
            }
            else if (editorStyle == EditorStyle.Edit)
            {
                Title += " bearbeiten";

                if (selectedItem is Account)
                {
                    AccountNumber = (selectedItem as Account).Number;
                    Description = (selectedItem as Account).Title;
                }

                if (selectedItem is WorkTask)
                {
                    Description = (selectedItem as WorkTask).Title;
                }
            }
        }

        private void AddItem()
        {
            if (selectedItem == null)
            {
                ContainerLocator.Container.Resolve<DataManager>().Accounts.Add(new Account(AccountNumber) { Title = Description });
            }

            if (selectedItem is Account)
            {
                (selectedItem as Account).AssignedTasks.Add(new WorkTask(Description, selectedItem as Account));
            }

            if (selectedItem is WorkTask)
            {
                var corrAcc = ContainerLocator.Container.Resolve<DataManager>().FindCorrespondingAccount(selectedItem as WorkTask);

                corrAcc.AssignedTasks.Add(new WorkTask(Description, corrAcc));
            }
        }

        private void EditItem()
        {
            if (selectedItem == null)
            {
                return;
            }

            if (selectedItem is Account)
            {
                ContainerLocator.Container.Resolve<DataManager>().Accounts.First(d => d.Number.Equals((selectedItem as Account).Number)).Update(AccountNumber, Description);
            }

            if (selectedItem is WorkTask)
            {
                ContainerLocator.Container.Resolve<DataManager>().FindCorrespondingAccount(selectedItem as WorkTask).AssignedTasks.First(d => d.Title.Equals((selectedItem as WorkTask).Title)).Title = Description;
            }
        }

        #region Command functions

        private void SaveChanges(Window window)
        {
            if (editorStyle == EditorStyle.Add)
            {
                AddItem();
            }

            if (editorStyle == EditorStyle.Edit)
            {
                EditItem();
            }

            window.DialogResult = true;
            window.Close();
        }

        private bool CanSaveChanges(Window window)
        {
            if (selectedItem is null)
            {
                return !string.IsNullOrWhiteSpace(Description) && !string.IsNullOrWhiteSpace(AccountNumber);
            }

            return !string.IsNullOrWhiteSpace(Description);
        }

        private void Abort(Window window)
        {
            window.DialogResult = false;
            window.Close();
        }

        #endregion

        #region Properties

        public Visibility IsNumberVisible
        {
            get { return isNumberVisible; }
            set
            {
                SetProperty(ref isNumberVisible, value);
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                SetProperty(ref title, value);
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                SetProperty(ref description, value);
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }

        public string AccountNumber
        {
            get { return accountNumber; }
            set
            {
                SetProperty(ref accountNumber, value);
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand<Window> SaveChangesCommand { get; private set; }

        public DelegateCommand<Window> AbortCommand { get; private set; }

        #endregion
    }
}
