using System;
using System.ComponentModel;

namespace KronosData.Model
{
    public class Settings : INotifyPropertyChanged
    {
        private TimeSpan defaultDailyWorkTime;
        private TimeSpan defaultBeginOfWork;
        private TimeSpan defaultEndOfWork;
        private TimeSpan defaultBreakTime;
        private TimeSpan carryOverTime;
        private string mappingToken;
        private string mappingUrl;

        public Settings()
        {
            DefaultDailyWorkTime = new TimeSpan(0);
            DefaultBeginOfWork = new TimeSpan(0);
            DefaultEndOfWork = new TimeSpan(0);
            DefaultBreakTime = new TimeSpan(0);
            CarryOverTime = new TimeSpan(0);
            MappingToken = string.Empty;
            MappingUrl = string.Empty;
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

        public TimeSpan DefaultDailyWorkTime 
        {
            get { return defaultDailyWorkTime; }
            set
            {
                defaultDailyWorkTime = value;
                OnPropertyChanged(nameof(DefaultDailyWorkTime));
            }
        }

        public TimeSpan DefaultBeginOfWork
        {
            get { return defaultBeginOfWork; }
            set
            {
                defaultBeginOfWork = value;
                OnPropertyChanged(nameof(DefaultBeginOfWork));
            }
        }

        public TimeSpan DefaultEndOfWork
        {
            get { return defaultEndOfWork; }
            set
            {
                defaultEndOfWork = value;
                OnPropertyChanged(nameof(DefaultEndOfWork));
            }
        }

        public TimeSpan DefaultBreakTime
        {
            get { return defaultBreakTime; }
            set
            {
                defaultBreakTime = value;
                OnPropertyChanged(nameof(DefaultBreakTime));
            }
        }

        public TimeSpan CarryOverTime
        {
            get { return carryOverTime; }
            set
            {
                carryOverTime = value;
                OnPropertyChanged(nameof(CarryOverTime));
            }
        }

        public string MappingToken
        {
            get { return mappingToken; }
            set
            {
                mappingToken = value;
                OnPropertyChanged(nameof(MappingToken));
            }
        }

        public string MappingUrl
        {
            get { return mappingUrl; }
            set
            {
                mappingUrl = value;
                OnPropertyChanged(nameof(MappingUrl));
            }
        }

        #endregion
    }
}
