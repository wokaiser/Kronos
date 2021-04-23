using KronosUI.ViewModels;
using Prism.Ioc;
using System.Windows.Controls;

namespace KronosUI.Views
{
    /// <summary>
    /// Interaktionslogik für WeekListingControlView.xaml
    /// </summary>
    public partial class WeekListingControlView : UserControl
    {
        public static readonly string ViewName = "WeekListingControlView";

        public WeekListingControlView()
        {
            InitializeComponent();
            DataContext = ContainerLocator.Container.Resolve<WeekListingViewModel>();
        }
    }
}
