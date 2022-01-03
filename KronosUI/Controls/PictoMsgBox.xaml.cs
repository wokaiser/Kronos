using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KronosUI.Controls
{
    /// <summary>
    /// Interaktionslogik für PictoMsgBox.xaml
    /// </summary>
    public partial class PictoMsgBox : Window
    {
        private PictoMsgBox(string title, string message, PictoMsgBoxButton bType)
        {
            InitializeComponent();
            DataContext = this;
            Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            Header = title;
            Message = message;
            SetButtonType(bType);
        }

        private void SetButtonType(PictoMsgBoxButton bType)
        {
            switch (bType)
            {
                case PictoMsgBoxButton.OK:
                    B_Deny.Visibility = Visibility.Collapsed;
                    B_Accept.SetValue(Grid.ColumnSpanProperty, 2);
                    break;

                case PictoMsgBoxButton.YesNo:
                    break;

                default:
                    return;
            }
        }

        #region Static functions

        /// <summary>
        /// Displays a message box that has a message
        /// </summary>
        /// <param name="message">The message to be displayes</param>
        /// <returns>Returns true</returns>
        public static bool? ShowMessage(string message)
        {
            return new PictoMsgBox(string.Empty, message, PictoMsgBoxButton.OK).ShowDialog();
        }

        /// <summary>
        /// Displays a message box that has a message and a title
        /// </summary>
        /// <param name="title">The title to be displayed</param>
        /// <param name="message">The message to be displayes</param>
        /// <returns>Returns true</returns>
        public static bool? ShowMessage(string title, string message)
        {
            return new PictoMsgBox(title, message, PictoMsgBoxButton.OK).ShowDialog();
        }

        /// <summary>
        /// Displays a message box that has a message, a title and the specified buttons
        /// </summary>
        /// <param name="title">The title to be displayed</param>
        /// <param name="message">The message to be displayes</param>
        /// <param name="button">The type of buttons to be displayed</param>
        /// <returns>Return value depends on what kind of buttons was pressed</returns>
        public static bool? ShowMessage(string title, string message, PictoMsgBoxButton button)
        {
            return new PictoMsgBox(title, message, button).ShowDialog();
        }

        #endregion

        #region Event handler

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

        #endregion

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

    /// <summary>
    /// Specifies the type of buttons to be displayed in an PictoMsgBox
    /// </summary>
    public enum PictoMsgBoxButton
    {
        /// <summary>
        /// Displayes only one confirm button
        /// </summary>
        OK = 0,

        /// <summary>
        /// Displays an accept and deny button
        /// </summary>
        YesNo
    }
}
