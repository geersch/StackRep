using System;
using System.Windows.Navigation;

namespace StackRep.Phone.UI.ViewModel
{
    public interface INavigationService
    {
        event NavigatingCancelEventHandler Navigating;

        void NavigateTo(Uri pageUri);

        void GoBack();
    }
}