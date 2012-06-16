using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Tasks;

namespace StackRep.Phone.UI.ViewModel
{
    public class AboutViewModel : ViewModelBase
    {
        public AboutViewModel()
        {
            EmailCommand = new RelayCommand<string>(emailAddress =>
            {
                try
                {
                    var task = new EmailComposeTask
                                   {
                                       To = emailAddress,
                                       Subject = "StackRep"
                                   };
                    task.Show();
                }
                catch { }
            });

            WebsiteCommand = new RelayCommand<string>(url =>
            {
                try
                {
                    var task = new WebBrowserTask
                                   {
                                       Uri = new Uri(url)
                                   };
                    task.Show();
                }
                catch { }
            });
        }

        public RelayCommand<string> EmailCommand { get; private set; }

        public RelayCommand<string> WebsiteCommand { get; private set; }
    }
}