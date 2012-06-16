using System;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using StackExchange.Api;
using StackRep.Phone.UI.ViewModel;

namespace StackRep.Phone.UI.View
{
    public partial class MainPage : PhoneApplicationPage
    {
        private bool _profileLoaded;
        private bool _reputationLoaded;
        private bool _badgesLoaded;

        public MainPage()
        {
            InitializeComponent();
        }

        protected MainViewModel ViewModel
        {
            get
            {
                return LayoutRoot.DataContext as MainViewModel;
            }
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            switch (Pivot.SelectedIndex)
            {
                case 0:
                    ViewModel.LoadProfileCommand.Execute(null);
                    break;

                case 1:
                    ViewModel.LoadReputationChangesCommand.Execute(null);
                    break;

                case 2:
                    ViewModel.LoadBadgesCommand.Execute(null);
                    break;
            }
        }

        private void ReputationList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var listBox = (ListBox)sender;
            if (listBox.SelectedIndex == -1)
                return;

            listBox.SelectedIndex = -1;

            if (e.AddedItems.Count > 0)
            {
                var reputationChange = e.AddedItems[0] as ReputationChange;
                if (reputationChange == null)
                    return;

                try
                {
                    var browserTask = new WebBrowserTask {Uri = new Uri(reputationChange.Link)};
                    browserTask.Show();
                }
                catch { }
            }
        }

        private void SettingsClick(object sender, EventArgs e)
        {
            ViewModel.SettingsCommand.Execute(null);
        }

        private void AboutClick(object sender, EventArgs e)
        {
            ViewModel.AboutCommand.Execute(null);
        }

        private void BadgeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = (ListBox) sender;
            if (listBox.SelectedIndex == -1)
                return;

            listBox.SelectedIndex = -1;

            if (e.AddedItems.Count > 0)
            {
                var badge = e.AddedItems[0] as Badge;
                if (badge == null)
                    return;

                try
                {
                    var browserTask = new WebBrowserTask {Uri = new Uri(badge.Link)};
                    browserTask.Show();
                }
                catch { }
            }
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((Pivot)sender).SelectedIndex)
            {
                case 0:
                    if (_profileLoaded)
                        return;
                    _profileLoaded = true;
                    ViewModel.LoadProfileCommand.Execute(null);
                    break;
                case 1:
                    if (_reputationLoaded)
                        return;
                    _reputationLoaded = true;
                    ViewModel.LoadReputationChangesCommand.Execute(null);
                    break;
                case 2:
                    if (_badgesLoaded)
                        return;
                    _badgesLoaded = true;
                    ViewModel.LoadBadgesCommand.Execute(null);
                    break;
            }
        }
    }
}