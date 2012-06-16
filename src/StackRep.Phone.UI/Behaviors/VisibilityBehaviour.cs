using System.Windows;
using System.Windows.Interactivity;

namespace StackRep.Phone.UI.Behaviors
{
    public class VisibilityBehaviour : Behavior<FrameworkElement>
    {
        public bool HasItems
        {
            get { return (bool)GetValue(HasItemsProperty); }
            set { SetValue(HasItemsProperty, value); }
        }

        public static readonly DependencyProperty HasItemsProperty = DependencyProperty.Register(
            "HasItems",
            typeof(bool),
            typeof(VisibilityBehaviour),
            new PropertyMetadata(false, HasItemsChanged));

        private static void HasItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = d as VisibilityBehaviour;
            if (behavior == null || behavior.AssociatedObject == null)
                return;

            if ((bool)e.NewValue)
            {
                behavior.AssociatedObject.Visibility = Visibility.Collapsed;
            }
            else
            {
                behavior.AssociatedObject.Visibility = Visibility.Visible;
            }
        }
    }
}
