using Microsoft.Phone.Controls;
using StackRep.Phone.UI.ViewModel;

namespace StackRep.Phone.UI.View
{
    public partial class Settings : PhoneApplicationPage
    {
        public Settings()
        {
            InitializeComponent();
        }

        protected SettingsViewModel ViewModel
        {
            get
            {
                return LayoutRoot.DataContext as SettingsViewModel;
            }
        }

        private void Save_Click(object sender, System.EventArgs e)
        {
            ViewModel.SaveCommand.Execute(ReputationHistory.SelectedItem);
        }
    }
}