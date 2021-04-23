using KronosUI.ViewModels;
using Prism.Ioc;
using System.Windows.Controls;

namespace KronosUI.Views
{
    /// <summary>
    /// Interaktionslogik für WeekListingView.xaml
    /// </summary>
    public partial class WeekListingView : UserControl
    {
        public static readonly string ViewName = "WeekListingView";

        public WeekListingView()
        {
            InitializeComponent();
            DataContext = ContainerLocator.Container.Resolve<WeekListingViewModel>();
        }
    }
}
