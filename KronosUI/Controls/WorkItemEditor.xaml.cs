using KronosData.Model;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace KronosUI.Controls
{
    /// <summary>
    /// Interaktionslogik für WorkItemEditor.xaml
    /// </summary>
    public partial class WorkItemEditor : Window
    {
        public WorkItemEditor(WorkItem selectedItem)
        {
            InitializeComponent();

            DataContext = new WorkItemEditorViewModel(selectedItem);
            Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        }

        private void DragMove_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
