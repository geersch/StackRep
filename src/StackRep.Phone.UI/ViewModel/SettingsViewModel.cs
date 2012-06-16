using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using StackRep.Phone.UI.Model;

namespace StackRep.Phone.UI.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private IEnumerable<int> _days;
        private readonly ISettings _settings;

        public SettingsViewModel(ISettings settings)
        {
            _settings = settings;
            _settings.Load();

            var availableDays = new List<int>();
            for(int i = 0; i < 20; i++)
            {
                availableDays.Add((i + 1) * 5);
            }

            AvailableDays = availableDays;

            SaveCommand = new RelayCommand<int>(days =>
            {
                _settings.ReputationHistory = days;
                _settings.Save();
                Messenger.Default.Send(new MoveToViewMessage(Page.MainPage));
            });
        }

        public IEnumerable<int> AvailableDays
        {
            get { return _days; }

            set
            {
                if (_days != value)
                {
                    _days = value;
                    RaisePropertyChanged("Data");
                }
            }
        }

        public int ReputationHistory
        {
            get { return _settings.ReputationHistory; }
        }

        public RelayCommand<int> SaveCommand { get; private set; }
    }
}