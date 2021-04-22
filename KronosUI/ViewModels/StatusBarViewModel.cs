using KronosUI.Events;
using Prism.Events;
using Prism.Mvvm;
using System.Threading;

namespace KronosUI.ViewModels
{
    public class StatusBarViewModel : BindableBase
    {
        private string statusText;

        public StatusBarViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<UpdateStatusBarTextEvent>().Subscribe(StatusBarTextUpdated);
        }

        void StatusBarTextUpdated(string text)
        {

            var delayedStatusThread = new Thread(() => ShowDelayedStatusText(text, 5000));
            delayedStatusThread.Start();
        }

        private void ShowDelayedStatusText(string text, int delay)
        {
            StatusText = text;
            Thread.Sleep(delay);
            StatusText = string.Empty;
        }

        #region Properties

        public string StatusText
        {
            get { return statusText; }
            set { SetProperty(ref statusText, value); }
        }

        #endregion
    }
}
