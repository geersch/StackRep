using System;

namespace StackExchange.Api
{
    public class StackExchangeErrorEventArgs : EventArgs
    {
        private readonly int _id;
        private readonly string _name;
        private readonly string _message;

        public StackExchangeErrorEventArgs(Error error)
        {
            _id = error.Id;
            _name = error.Name;
            _message = error.Message;
        }

        public StackExchangeError Id
        {
            get { return (StackExchangeError)_id; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string Message
        {
            get { return _message; }
        }
    }
}