using KronosUI.ViewModels;
using Prism.Ioc;
using System.Windows.Controls;

namespace KronosUI.Views
{
    /// <summary>
    /// Interaktionslogik für YearListingControlView.xaml
    /// </summary>
    public partial class YearListingControlView : UserControl
    {
        public static readonly string ViewName = "YearListingControlView";

        public YearListingControlView()
        {
            InitializeComponent();
            DataContext = ContainerLocator.Container.Resolve<YearListingViewModel>();
        }
    }
}
