using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using StackExchange.Api;

namespace StackRep.Phone.UI.ViewModel
{
    public class AuthViewModel : ViewModelBase
    {
        public AuthViewModel()
        {
            SaveAuthorizationTokenCommand = new RelayCommand<Uri>(SaveAuthToken);
        }

        public RelayCommand<Uri> SaveAuthorizationTokenCommand { get; private set; }

        public string AuthUrl
        {
            get { return AccessToken.AuthorizationUri.ToString(); }
        }

        private void SaveAuthToken(Uri uri)
        {
            var token = AccessToken.ParseAccessToken(uri);
            if (token == null)
                return;

            new AccessToken {Token = token}.Save();

            Messenger.Default.Send(new MoveToViewMessage(Page.MainPage));
        }
    }
}