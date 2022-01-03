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
            var editor = new WorkItemEditor(workItem);

            return (bool)editor.ShowDialog();
        }

        public static bool AddWorkItem(WorkDay workDay)
        {
            return EditWorkItem(null);
        }

        private void DragMove_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
