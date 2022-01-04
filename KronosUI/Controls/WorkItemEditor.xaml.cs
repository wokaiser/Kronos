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
        private WorkItemEditor(WorkDay currentDay, WorkItem selectedItem)
        {
            InitializeComponent();

            DataContext = new WorkItemEditorViewModel(currentDay, selectedItem);
            Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        }

        public static bool EditWorkItem(WorkDay workDay, WorkItem workItem)
        {
            var editor = new WorkItemEditor(workDay, workItem);

            return (bool)editor.ShowDialog();
        }

        public static bool AddWorkItem(WorkDay currentDay)
        {
            var editor = new WorkItemEditor(currentDay, WorkItem.Empty);

            return (bool)editor.ShowDialog();
        }

        private void DragMove_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
