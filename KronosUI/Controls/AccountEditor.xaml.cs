﻿using System;
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

namespace KronosUI.Controls
{
    /// <summary>
    /// Interaktionslogik für AccountEditor.xaml
    /// </summary>
    public partial class AccountEditor : Window
    {
        public enum EditorStyle { Add = 0, Edit = 1};

        public AccountEditor(EditorStyle editorStyle, object selectedItem)
        {
            InitializeComponent();
            DataContext = new AccountEditorViewModel();
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}