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
        private WorkItemEditor(ref WorkItem selectedItem)
        {
            InitializeComponent();

            DataContext = new WorkItemEditorViewModel(ref selectedItem);
            Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        }

        public static bool EditWorkItem(ref WorkItem workItem)
        {
            return (bool)new WorkItemEditor(ref workItem).ShowDialog();
        }

        public static bool AddWorkItem(out WorkItem workItem)
        {
            workItem = WorkItem.Empty;

            return (bool)new WorkItemEditor(ref workItem).ShowDialog();
        }

        private void DragMove_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
