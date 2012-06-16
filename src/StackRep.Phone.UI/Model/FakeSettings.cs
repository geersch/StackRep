namespace StackRep.Phone.UI.Model
{
    public class FakeSettings : ISettings
    {
        public void Load()
        {
        }

        public void Save()
        {
        }

        public int ReputationHistory
        {
            get { return 30; }
            set
            {
            }
        }
    }
}