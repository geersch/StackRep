using Ninject;
using StackRep.Phone.UI.Model;
using StackRep.Phone.UI.ViewModel;

namespace StackRep.Phone.UI
{
    public class DesignTimeModule : IIocContainer
    {
        private static readonly IKernel Kernel = new StandardKernel();

        static DesignTimeModule()
        {
            Kernel.Bind<INavigationService>().To<NavigationService>();
            Kernel.Bind<IDeviceCache>().To<FakeDeviceCache>();
            Kernel.Bind<ISettings>().To<FakeSettings>();
        }

        public T Get<T>()
        {
            return Kernel.Get<T>();
        }
    }
}