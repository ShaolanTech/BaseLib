using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShaolanTech
{
    public class SimpleModel : INotifyPropertyChanged, IDisposable
    {

        [JsonIgnore]
        protected Dictionary<string, object> properties = new Dictionary<string, object>();
        public Dictionary<string, object> GetProperties()
        {
            return this.properties;
        }
        public void SetProperties(Dictionary<string, object> dic)
        {
            foreach (var item in dic)
            {
                var key = item.Key.ToLower();
                if (this.properties.ContainsKey(key) == false)
                {
                    this.properties.Add(key, item.Value);
                }
            }
        }
        private readonly object propertyLock = new object();


        public event PropertyChangedEventHandler PropertyChanged;

        public object GetProperty(string propertyName)
        {
            object result = null;

            lock (propertyLock)
            {
                if (this.properties == null || this.properties.ContainsKey(propertyName.ToLower()) == false)
                {

                }
                else
                {
                    result = this.properties[propertyName.ToLower()];
                    if (result == DBNull.Value)
                    {
                        result = null;
                    }

                }
            }
            return result;
        }
        public void SetProperty(string propertyName, object value)
        {
            if (this.properties == null)
            {
                return;
            }

            lock (propertyLock)
            {
                if (this.properties.ContainsKey(propertyName.ToLower()))
                {
                    this.properties[propertyName.ToLower()] = value;
                }
                else
                {

                    this.properties.Add(propertyName.ToLower(), value);

                }

            }
            this.OnPropertyChanged(propertyName.ToLower());
        }
        public T GetProperty<T>(string propertyName)
        {
            object result = this.GetProperty(propertyName);


            if (result != null && result is T)
            {
                return (T)result;
            }
            else
            {
                return default(T);
            }

        }
        [System.Text.Json.Serialization.JsonIgnore]
        [JsonIgnore]
        public object this[string propertyName]
        {
            get
            {
                return this.GetProperty(propertyName);
            }

        }

        protected void ClearProperties()
        {
            lock (propertyLock)
            {
                if (this.properties != null)
                {
                    this.properties.Clear();
                }

            }

        }

        protected void OnPropertyChanged(string properyName)
        {
            if (this.PropertyChanged != null)
            {
                this.SafeInvoke(new Action(() =>
                {

                    this.PropertyChanged(this, new PropertyChangedEventArgs(properyName));

                }));
            }
        }

        /// <summary>
        /// 获取属性的Double可空类型
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public double? GetDouble(string propertyName)
        {
            var value = this.GetProperty(propertyName);
            if (value == null)
            {
                return null;
            }
            else
            {
                if (value is float || value is decimal || value is double)
                {
                    return double.Parse(value.ToString());
                }
                return null;
            }
        }
        /// <summary>
        /// 获取属性的Long可空类型
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public long? GetLong(string propertyName)
        {
            var value = this.GetProperty(propertyName);
            if (value == null)
            {
                return null;
            }
            else
            {
                if (value is long)
                {
                    return (long)value;
                }
                else
                {
                    if (value is byte || value is sbyte
                  || value is short || value is ushort
                  || value is int || value is uint)
                    {
                        return long.Parse(value.ToString());
                    }
                    return null;
                }

            }

        }
        /// <summary>
        /// 获取属性的Int可空类型
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public int? GetInt(string propertyName)
        {
            var value = this.GetProperty(propertyName);
            if (value == null)
            {
                return null;
            }
            else
            {
                if (value is int)
                {
                    return (int)value;
                }
                else
                {
                    if (value is byte || value is sbyte
                  || value is short || value is ushort)
                    {
                        return int.Parse(value.ToString());
                    }
                    return null;
                }

            }

        }
        /// <summary>
        /// 获取属性的string值
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public string GetString(string propertyName)
        {
            var value = this.GetProperty(propertyName);
            if (value == null)
            {
                return null;
            }
            if (value is string == false)
            {
                return value.ToString();
            }
            return (string)value;
        }
        /// <summary>
        /// 获取属性的DateTime可空类型
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public DateTime? GetDateTime(string propertyName)
        {
            var value = this.GetProperty(propertyName);
            if (value == null || !(value is DateTime))
            {
                return null;
            }
            return (DateTime)value;
        }
        public WeakReference ToWeakReference()
        {
            return new WeakReference(this);
        }
        public void Dispose()
        {
            this.ClearProperties();
            this.properties = null;
        }
    }
    /// <summary>
    /// 简单实体基础类
    /// </summary>
    public class ModelBase : SimpleModel
    {
        private IEnumerable<MethodInfo> methods;
        public static event EventHandler<NewMessageEventArgs> OnCommand;
        public static void TriggerStaticCommand(object sender, string commandName, object data)
        {
            if (OnCommand != null)
            {
                OnCommand(sender, new NewMessageEventArgs() { Message = commandName, Data = data });
            }
        }
        /// <summary>
        /// 当OnButtonCommand执行前触发
        /// </summary>
        public event EventHandler<NewMessageEventArgs> OnButtonEvent;
        public virtual ButtonCommand ButtonCommand
        {
            get { return this.GetProperty<ButtonCommand>("ButtonCommand"); }
            set
            {
                this.SetProperty("ButtonCommand", value);
            }
        }
        public ModelBase()
        {
            this.ButtonCommand = new ButtonCommand();
            this.ButtonCommand.Executing += ButtonCommand_Executing;
            this.methods = this.GetType().GetRuntimeMethods();

        }

        private void ButtonCommand_Executing(object sender, EventArgs e)
        {
            if (this.OnButtonEvent != null)
            {
                this.OnButtonEvent(this, new NewMessageEventArgs() { Message = this.ButtonCommand.CommandName });
            }
            if (OnCommand != null)
            {
                OnCommand(this, new NewMessageEventArgs() { Message = this.ButtonCommand.CommandName });
            }
            var method = this.methods.FirstOrDefault(m => m.Name == this.ButtonCommand.CommandName && m.GetParameters().Length == 0);
            if (method != null)
            {
                method.Invoke(this, null);
            }
            else
            {
                this.OnButtonCommand(this.ButtonCommand.CommandName);
            }
        }
        protected virtual void OnButtonCommand(string commandName)
        {

        }
    }
}
