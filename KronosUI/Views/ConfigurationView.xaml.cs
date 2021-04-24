using KronosUI.ViewModels;
using Prism.Ioc;
using System.Windows.Controls;

namespace KronosUI.Views
{
    /// <summary>
    /// Interaktionslogik für ConfigurationView.xaml
    /// </summary>
    public partial class ConfigurationView : UserControl
    {
        public static readonly string ViewName = "ConfigurationView";

        public ConfigurationView()
        {
            InitializeComponent();
            DataContext = ContainerLocator.Container.Resolve<ConfigurationViewModel>();
        }
    }
}
