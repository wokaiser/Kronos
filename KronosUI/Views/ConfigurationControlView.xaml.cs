﻿using KronosUI.ViewModels;
using Prism.Ioc;
using System.Windows.Controls;

namespace KronosUI.Views
{
    /// <summary>
    /// Interaktionslogik für ControlView.xaml
    /// </summary>
    public partial class ConfigurationControlView : UserControl
    {
        public static readonly string ViewName = "ConfigurationControlView";

        public ConfigurationControlView()
        {
            InitializeComponent();
            DataContext = ContainerLocator.Container.Resolve<ConfigurationViewModel>();
        }
    }
}
