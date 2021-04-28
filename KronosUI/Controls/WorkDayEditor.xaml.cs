using KronosData.Model;
using KronosUI.Views;
using Prism.Ioc;
using System.Windows;
using System.Windows.Input;

namespace KronosUI.Controls
{
    /// <summary>
    /// Interaktionslogik für WorkDayEditor.xaml
    /// </summary>
    public partial class WorkDayEditor : Window
    {
        public WorkDayEditor(WorkDay selectedItem)
        {
            InitializeComponent();

            DataContext = new WorkDayEditorViewModel(selectedItem);
            Owner = ContainerLocator.Container.Resolve<Shell>();
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
