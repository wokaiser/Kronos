using KronosUI.ViewModels;
using Prism.Ioc;
using System.Windows.Controls;

namespace KronosUI.Views
{
    /// <summary>
    /// Interaktionslogik für MonthListingView.xaml
    /// </summary>
    public partial class MonthListingView : UserControl
    {
        public static readonly string ViewName = "MonthListingView";

        public MonthListingView()
        {
            InitializeComponent();
            DataContext = ContainerLocator.Container.Resolve<MonthListingViewModel>();
        }
    }
}
