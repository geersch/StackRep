using System.Windows;
using System.Windows.Interactivity;
using Microsoft.Phone.Shell;

namespace StackRep.Phone.UI.Behaviors
{
    public class LoadingBehavior : Behavior<ProgressIndicator>
    {
        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
          DependencyProperty.Register(
              "IsLoading",
              typeof(bool),
              typeof(LoadingBehavior),
              new PropertyMetadata(false, (d, e) =>
              {
                  var isLoading = (bool)e.NewValue;
                  var control = ((LoadingBehavior)d).AssociatedObject;
                  control.IsVisible = isLoading;
              }));
    }
}
