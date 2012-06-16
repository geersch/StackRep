namespace StackRep.Phone.UI.Model
{
    public interface IDeviceCache
    {
        void Save(string key, object value);

        T Load<T>(string key);
    }
}