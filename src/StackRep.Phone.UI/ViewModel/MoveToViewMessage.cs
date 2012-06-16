namespace StackRep.Phone.UI.ViewModel
{
    public class MoveToViewMessage
    {
        private readonly Page _page;

        public MoveToViewMessage(Page page)
        {
            _page = page;
        }

        public Page Page { get { return _page; } }
    }
}