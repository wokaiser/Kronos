using KronosData.Model;
using Prism.Commands;
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
            Title = selectedItem == null ? "Kontierung" : "Arbeitspaket";

            if (editorStyle == EditorStyle.Add)
            {
                Title += " hinzufügen";
            }
            else if (editorStyle == EditorStyle.Edit)
            {
                Title += " bearbeiten";
            }

            SaveChangesCommand = new DelegateCommand<Window>(SaveChanges, CanSaveChanges);
            AbortCommand = new DelegateCommand<Window>(Abort);
        }

        #region Command functions

        private void SaveChanges(Window window)
        {
            window.Close();
        }

        private bool CanSaveChanges(Window window)
        {
            return !string.IsNullOrWhiteSpace(Description) && !string.IsNullOrWhiteSpace(AccountNumber);
        }

        private void Abort(Window window)
        {
            window.Close();
        }

        #endregion

        #region Properties

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
