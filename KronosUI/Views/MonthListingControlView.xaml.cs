using KronosUI.ViewModels;
using Prism.Ioc;
using System.Windows.Controls;

namespace KronosUI.Views
{
    /// <summary>
    /// Interaktionslogik für MonthListingControlView.xaml
    /// </summary>
    public partial class MonthListingControlView : UserControl
    {
        public static readonly string ViewName = "MonthListingControlView";

        public MonthListingControlView()
        {
            InitializeComponent();
            DataContext = ContainerLocator.Container.Resolve<MonthListingViewModel>();
        }
    }
}
