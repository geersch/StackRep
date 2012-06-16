using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using StackRep.Phone.UI.ViewModel;

namespace StackRep.Phone.UI.View
{
    public partial class AuthPage : PhoneApplicationPage
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        protected AuthViewModel ViewModel
        {
            get
            {
                return LayoutRoot.DataContext as AuthViewModel;
            }
        }

        private void webBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            ViewModel.SaveAuthorizationTokenCommand.Execute(e.Uri);
        }
    }
}