using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace KronosUI.Controls
{
    /// <summary>
    /// Interaktionslogik für PictoMsgBox.xaml
    /// </summary>
    public partial class PictoMsgBox : Window
    {
        /// <summary>
        /// Creates a new pictographic message box
        /// </summary>
        /// <param name="title">The title of the message box</param>
        /// <param name="message">The message of the message box</param>
        private PictoMsgBox(string title, string message)
        {
            InitializeComponent();
            DataContext = this;
            Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            Header = title;
            Message = message;
        }

        public static bool? ShowMessage(string title, string message)
        {
            var dialog = new PictoMsgBox(title, message);

            return dialog.ShowDialog();
        }

        private void MouseDown_DragMove(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Deny_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        #region Properties

        /// <summary>
        /// The header of the message box
        /// </summary>
        public string Header { get; private set; }

        /// <summary>
        /// The message of the message box
        /// </summary>
        public string Message { get; private set; }

        #endregion
    }
}
