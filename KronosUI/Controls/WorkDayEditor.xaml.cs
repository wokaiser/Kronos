using KronosData.Model;
using System.Linq;
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
            Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
