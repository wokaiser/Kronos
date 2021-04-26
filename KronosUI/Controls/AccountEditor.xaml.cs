using KronosUI.Views;
using Prism.Ioc;
using System.Windows;
using System.Windows.Input;
using static KronosUI.Controls.AccountEditorViewModel;

namespace KronosUI.Controls
{
    /// <summary>
    /// Interaktionslogik für AccountEditor.xaml
    /// </summary>
    public partial class AccountEditor : Window
    {

        public AccountEditor(EditorStyle editorStyle, object selectedItem)
        {
            InitializeComponent();

            DataContext = new AccountEditorViewModel(editorStyle, selectedItem);
            Owner = ContainerLocator.Container.Resolve<Shell>();
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
