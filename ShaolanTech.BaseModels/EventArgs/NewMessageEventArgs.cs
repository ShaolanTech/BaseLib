using System;
using System.Collections.Generic;
using System.Text;

namespace ShaolanTech 
{
    public class NewMessageEventArgs<T> : EventArgs
    {
        public NewMessageEventArgs()
        { }
        public NewMessageEventArgs(string message)
        {
            this.Message = message;
        }
        public NewMessageEventArgs(string message, T data)
        {
            this.Message = message;
            this.Data = data;
        }
        public string Message { get; set; }
        public T Data { get; set; }
    }
    public class NewMessageEventArgs : EventArgs
    {
        public NewMessageEventArgs()
        { }
        public NewMessageEventArgs(string message)
        {
            this.Message = message;
        }
        public NewMessageEventArgs(string message, object data)
        {
            this.Message = message;
            this.Data = data;
        }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
