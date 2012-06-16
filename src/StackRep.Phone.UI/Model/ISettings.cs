namespace StackRep.Phone.UI.Model
{
    public interface ISettings
    {
        void Load();

        void Save();

        int ReputationHistory { get; set; }
    }
}