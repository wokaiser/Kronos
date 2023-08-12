using KronosData.Model;
using System;
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
        private WorkItemEditor(ref WorkItem selectedItem, TimeSpan remaining)
        {
            InitializeComponent();

            DataContext = new WorkItemEditorViewModel(ref selectedItem, remaining);
            Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        }

        public static bool EditWorkItem(ref WorkItem workItem, TimeSpan remaining)
        {
            return (bool)new WorkItemEditor(ref workItem, remaining).ShowDialog();
        }

        public static bool AddWorkItem(out WorkItem workItem, TimeSpan remaining)
        {
            workItem = WorkItem.Empty;

            return (bool)new WorkItemEditor(ref workItem, remaining).ShowDialog();
        }

        private void DragMove_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
