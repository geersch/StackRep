using Ninject;
using StackRep.Phone.UI.Model;
using StackRep.Phone.UI.ViewModel;

namespace StackRep.Phone.UI
{
    public class RunTimeModule : IIocContainer
    {
        private static readonly IKernel Kernel = new StandardKernel();

        static RunTimeModule()
        {
            Kernel.Bind<INavigationService>().To<NavigationService>().InSingletonScope();
            Kernel.Bind<IDeviceCache>().To<DeviceCache>().InSingletonScope();
            Kernel.Bind<ISettings>().To<Settings>().InSingletonScope();
        }

        public T Get<T>()
        {
            return Kernel.Get<T>();
        }
    }
}
