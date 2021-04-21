using KronosUI.Model;
using Prism.Regions;
using System.Windows;

namespace KronosUI.Views
{
    /// <summary>
    /// Interaktionslogik für Shell.xaml
    /// </summary>
    public partial class Shell : Window
    {
        public Shell(IRegionManager regionManager)
        {
            InitializeComponent();

            RegisterRegions(regionManager);
        }

        private void RegisterRegions(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion(RegionNames.StatusBarRegion, typeof(StatusBarView));
            regionManager.RegisterViewWithRegion(RegionNames.ControlRegion, typeof(ControlView));
            regionManager.RegisterViewWithRegion(RegionNames.DataRegion, typeof(ConfigurationView));
            regionManager.RegisterViewWithRegion(RegionNames.DataRegion, typeof(WeekListingView));
            regionManager.RegisterViewWithRegion(RegionNames.DataRegion, typeof(MonthListingView));
            regionManager.RegisterViewWithRegion(RegionNames.DataRegion, typeof(YearListingView));
        }
    }
}
