using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace KronosUI.Controls
{
    /// <summary>
    /// Interaktionslogik für TimePicker.xaml
    /// </summary>
    public partial class TimePicker : UserControl, INotifyPropertyChanged
    {
        public TimePicker()
        {
            ValidHours = new List<string>(); 
            ValidMinutes = new List<string>();

            for (int i = 0; i < 60; i++)
            {
                if (i < 24)
                {
                    ValidHours.Add(i.ToString("00"));
                }
                ValidMinutes.Add(i.ToString("00"));
            }

            InitializeComponent();
        }

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = obj as TimePicker;
            var newTime = (TimeSpan)e.NewValue;

            control.Hours = newTime.Hours.ToString("00");
            control.Minutes = newTime.Minutes.ToString("00");
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Properties

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(TimeSpan), typeof(TimePicker),
            new FrameworkPropertyMetadata(DateTime.Now.TimeOfDay, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

        public TimeSpan Value
        {
            get { return (TimeSpan)GetValue(ValueProperty); }
            set
            {
                SetValue(ValueProperty, value);
                OnPropertyChanged(nameof(Hours));
                OnPropertyChanged(nameof(Minutes));
            }
        }

        public string Hours 
        {
            get { return Value.Hours.ToString("00"); }
            set
            {
                SetValue(ValueProperty, new TimeSpan(int.Parse(value), Value.Minutes, Value.Seconds));
                OnPropertyChanged(nameof(Hours));
            }
        }

        public string Minutes
        {
            get { return Value.Minutes.ToString("00"); }
            set
            {
                SetValue(ValueProperty, new TimeSpan(Value.Hours, int.Parse(value), Value.Seconds));
                OnPropertyChanged(nameof(Minutes));
            }
        }

        public List<string> ValidHours { get; private set; }

        public List<string> ValidMinutes { get; private set; }

        #endregion
    }
}
