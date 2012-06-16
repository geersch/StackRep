using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using StackExchange.Api;
using StackRep.Phone.UI.Model;

namespace StackRep.Phone.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private const string ApplicationKey = "S6XDTEGdBeKj7S02POvGnA((";
        private User _user;
        private IEnumerable<ReputationChange> _reputationChanges;
        private IEnumerable<Badge> _badges;
        private bool _isLoading;
        private bool _authorizing;
        private readonly IDeviceCache _cache;
        private bool _hasReputation = true;
        private bool _hasBadges = true;

        public MainViewModel(IDeviceCache deviceCache)
        {
            _cache = deviceCache;

            User = _cache.Load<User>("User");
            ReputationChanges = _cache.Load<IEnumerable<ReputationChange>>("Reputation");
            Badges = _cache.Load<IEnumerable<Badge>>("Badges");

            LoadProfileCommand = new RelayCommand(LoadProfile);
            LoadReputationChangesCommand = new RelayCommand(LoadReputationChanges);
            LoadBadgesCommand = new RelayCommand(LoadBadges);
            AboutCommand = new RelayCommand(() => Messenger.Default.Send(new MoveToViewMessage(Page.About)));
            SettingsCommand = new RelayCommand(() => Messenger.Default.Send(new MoveToViewMessage(Page.Settings)));
        }

        public User User
        {
            get { return _user; }

            set
            {
                if (_user != value)
                {
                    _user = value;
                    RaisePropertyChanged("User");
                }
            }
        }

        public IEnumerable<ReputationChange> ReputationChanges
        {
            get { return _reputationChanges; }

            set
            {
                if (_reputationChanges != value)
                {
                    _reputationChanges = value;
                    RaisePropertyChanged("ReputationChanges");
                }
            }
        }

        public IEnumerable<Badge> Badges
        {
            get { return _badges; }

            set
            {
                if (_badges != value)
                {
                    _badges = value;
                    RaisePropertyChanged("Badges");
                }
            }
        }

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }

            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    RaisePropertyChanged("IsLoading");
                }
            }
        }

        public bool HasReputation
        {
            get
            {
                return _hasReputation;
            }

            set
            {
                if (_hasReputation != value)
                {
                    _hasReputation = value;
                    RaisePropertyChanged("HasReputation");
                }
            }
        }

        public bool HasBadges
        {
            get
            {
                return _hasBadges;
            }

            set
            {
                if (_hasBadges != value)
                {
                    _hasBadges = value;
                    RaisePropertyChanged("HasBadges");
                }
            }
        }

        public RelayCommand LoadProfileCommand { get; private set; }

        public RelayCommand LoadReputationChangesCommand { get; private set; }

        public RelayCommand LoadBadgesCommand { get; private set; }

        public RelayCommand AboutCommand { get; private set; }

        public RelayCommand SettingsCommand { get; private set; }

        private void LoadProfile()
        {
            Action<StackExchangeApi> action = api => 
                api.GetUser(user => DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    User = user;
                    IsLoading = false;

                    _cache.Save("User", user);
                }));

            LoadData(action);
        }

        private void LoadReputationChanges()
        {
            HasReputation = ReputationChanges != null && ReputationChanges.Any();

            var settings = new Settings();
            settings.Load();

            Action<StackExchangeApi> action = api =>
                api.GetReputationChanges(settings.ReputationHistory, reputationChanges => DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    ReputationChanges = reputationChanges;
                    IsLoading = false;
                    HasReputation = ReputationChanges != null && ReputationChanges.Any();

                    _cache.Save("Reputation", reputationChanges);
                }));

            LoadData(action);
        }

        private void LoadBadges()
        {
            HasBadges = Badges != null && Badges.Any();

            Action<StackExchangeApi> action = api =>
                api.GetBadges(badges => DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    Badges = badges;
                    IsLoading = false;
                    HasBadges = Badges != null && Badges.Any();

                    _cache.Save("Badges", badges);
                }));

            LoadData(action);
        }

        private void LoadData(Action<StackExchangeApi> action)
        {
            try
            {
                var accessToken = new AccessToken();
                accessToken.Load();

                var isAuthorized = accessToken.IsValid;
                if (!isAuthorized)
                {
                    Authorize();
                    return;
               }

                var api = new StackExchangeApi(ApplicationKey, accessToken.Token);
                api.Error += (s, e) => HandleError(e);

                IsLoading = true;

                action(api);
            }
            catch
            {
                IsLoading = false;

                if (!NetworkInterface.GetIsNetworkAvailable())
                {
                    var message = new DialogMessage(null, result => { });
                    Messenger.Default.Send(message, "NoInternetConnection");
                    return;
                }
               
                throw;
            }
        }

        private void HandleError(StackExchangeErrorEventArgs error)
        {
            Action action = () =>
            {
                IsLoading = false;

                switch (error.Id)
                {
                    case StackExchangeError.AccessTokenRequired:
                    case StackExchangeError.InvalidAccessToken:
                    case StackExchangeError.AccessTokenCompromised:
                        Authorize();
                        break;
                    case StackExchangeError.TemporarilyUnavailable:
                        var message = new DialogMessage(null, result => {});
                        Messenger.Default.Send(message, "NoInternetConnection");
                        break;
                    default:
                        throw new Exception(error.Message);
                }
            };

            DispatcherHelper.CheckBeginInvokeOnUI(action);
        }

        private void Authorize()
        {
            if (_authorizing)
                return;

            _authorizing = true;
            var message = new DialogMessage(null, result =>
            {
                if (result == MessageBoxResult.OK)
                {
                    Messenger.Default.Send(new MoveToViewMessage(Page.AuthPage));
                }
                _authorizing = false;
            });

            Messenger.Default.Send(message, "Authorize");
        }
    }
}