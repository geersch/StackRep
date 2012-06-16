using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using StackRep.Phone.UI.Model;

namespace StackRep.Phone.UI.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        private static MainViewModel _main;
        private static AboutViewModel _about;
        private static AuthViewModel _authorization;
        private static SettingsViewModel _settings;

        public static readonly Uri MainPageUri = new Uri("/View/MainPage.xaml", UriKind.Relative);

        public static readonly Uri AuthPageUri = new Uri("/View/AuthPage.xaml", UriKind.Relative);

        public static readonly Uri AboutPageUri = new Uri("/View/About.xaml", UriKind.Relative);

        public static readonly Uri SettingsPageUri = new Uri("/View/Settings.xaml", UriKind.Relative);

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            IIocContainer container;
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time services and viewmodels
                container = new DesignTimeModule();
            }
            else
            {
                // Create run time services and view models
                container = new RunTimeModule();
            }

            var navigationService = container.Get<INavigationService>();

            Messenger.Default.Register<MoveToViewMessage>(this, message =>
            {
                switch (message.Page)
                {
                    case Page.AuthPage:
                        navigationService.NavigateTo(AuthPageUri);
                        break;

                    case Page.MainPage:
                        navigationService.NavigateTo(MainPageUri);
                        break;

                    case Page.About:
                        navigationService.NavigateTo(AboutPageUri);
                        break;

                    case Page.Settings:
                        navigationService.NavigateTo(SettingsPageUri);
                        break;
                }
            });

            _main = new MainViewModel(container.Get<IDeviceCache>());
            _about = new AboutViewModel();
            _authorization = new AuthViewModel();
            _settings = new SettingsViewModel(container.Get<ISettings>());
        }

        /// <summary>
        /// Gets the Main property which defines the main viewmodel.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return _main;
            }
        }

        /// <summary>
        /// Gets the About property which defines the about viewmodel.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public AboutViewModel About
        {
            get
            {
                return _about;
            }
        }

        /// <summary>
        /// Gets the Authorization property which defines the authorization viewmodel.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public AuthViewModel Authorization
        {
            get
            {
                return _authorization;
            }
        }

        /// <summary>
        /// Gets the Settings property which defines the settings viewmodel.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public SettingsViewModel Settings
        {
            get
            {
                return _settings;
            }
        }
    }
}