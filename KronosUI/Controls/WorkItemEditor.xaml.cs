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
        private WorkItemEditor(WorkItem selectedItem)
        {
            InitializeComponent();

            DataContext = new WorkItemEditorViewModel(selectedItem);
            Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        }

        public static bool EditWorkItem(WorkItem workItem)
        {
            return (bool)new WorkItemEditor(workItem).ShowDialog();
        }

        public static bool AddWorkItem()
        {
            return (bool)new WorkItemEditor(WorkItem.Empty).ShowDialog();
        }

        private void DragMove_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
