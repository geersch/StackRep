using System.Windows;
using System.Windows.Interactivity;
using GalaSoft.MvvmLight.Messaging;

namespace StackRep.Phone.UI.Behaviors
{
    public class DialogMessageBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            Messenger.Default.Register<DialogMessage>(this, Token, Show);
        }

        public string Token { get; set; }

        public string Message { get; set; }

        public string Caption { get; set; }

        public MessageBoxButton Buttons { get; set; }

        public void Show(DialogMessage message)
        {
            var result = MessageBox.Show(Message, Caption, Buttons);
            message.Callback(result);
        }
    }
}
