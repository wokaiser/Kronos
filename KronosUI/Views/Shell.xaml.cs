using CommonServiceLocator;
using KronosUI.Model;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
