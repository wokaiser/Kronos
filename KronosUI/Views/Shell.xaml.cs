using KronosUI.Model;
using Prism.Ioc;
using Prism.Regions;
using System.Windows;
using System.Windows.Input;

namespace KronosUI.Views
{
    /// <summary>
    /// Interaktionslogik für Shell.xaml
    /// </summary>
    public partial class Shell : Window
    {
        public Shell()
        {
            InitializeComponent();

            RegisterRegions();
        }

        private void RegisterRegions()
        {
            var regionManager = ContainerLocator.Container.Resolve<IRegionManager>();

            regionManager.RegisterViewWithRegion(RegionNames.NavigationRegion, typeof(NavigationView));

            regionManager.RegisterViewWithRegion(RegionNames.StatusBarRegion, typeof(StatusBarView));

            regionManager.RegisterViewWithRegion(RegionNames.ControlRegion, typeof(WeekListingControlView));
            regionManager.RegisterViewWithRegion(RegionNames.ControlRegion, typeof(MonthListingControlView));
            regionManager.RegisterViewWithRegion(RegionNames.ControlRegion, typeof(YearListingControlView));
            regionManager.RegisterViewWithRegion(RegionNames.ControlRegion, typeof(ConfigurationControlView));

            regionManager.RegisterViewWithRegion(RegionNames.DataRegion, typeof(WeekListingView));
            regionManager.RegisterViewWithRegion(RegionNames.DataRegion, typeof(MonthListingView));
            regionManager.RegisterViewWithRegion(RegionNames.DataRegion, typeof(YearListingView));
            regionManager.RegisterViewWithRegion(RegionNames.DataRegion, typeof(ConfigurationView));
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
