using KronosUI.Events;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System.Threading;

namespace KronosUI.ViewModels
{
    public class StatusBarViewModel : BindableBase
    {
        private string statusText;

        public StatusBarViewModel()
        {
            ContainerLocator.Container.Resolve<IEventAggregator>().GetEvent<UpdateStatusBarTextEvent>().Subscribe(StatusBarTextUpdated);
        }

        void StatusBarTextUpdated(string text)
        {
            //TODO: Reset timer to avoid cancelling of previous running thread
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
