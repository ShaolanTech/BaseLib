using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;

namespace ShaolanTech
{
    public static class SystemModelExtensions
    {
        private static SynchronizationContext SynchronizationContext { get; set; }
        public static void BindSynchronizationContext(this object obj, SynchronizationContext context)
        {
            if (context != null)
            {
                SynchronizationContext = context;
            }

        }


        /// <summary>
        ///  在子线程中执行主线程中的安全操作
        /// </summary>
        /// <param name="obj">被扩展对象</param>
        /// <param name="action">要执行的回调函数</param>
        public static void SafeInvoke(this object obj, Action action)
        {
            SafeInvoke(action);
        }
        public static void SafeInvoke<T>(this object obj, Action<T> action, object data)
        {
            SafeInvoke(action, data);
        }
        class SafeInvokeCallback
        {
            public object Callback { get; set; }
            public object State { get; set; }
        }
        public static void SafeInvoke<T>(Action<T> callback, object state = null)
        {

            if (SynchronizationContext != null)
            {
                SynchronizationContext.Post((e) =>
                {
                    var data = (SafeInvokeCallback)e;
                    ((Action<T>)data.Callback).Invoke((T)data.State);
                }, new SafeInvokeCallback() { Callback = callback, State = state });
            }
            else
            {
                callback((T)state);
            }
        }

        public static void SafeInvoke(Action callback)
        {
            if (SynchronizationContext != null)
            {
                try
                {
                    SynchronizationContext.Send((e) =>
                    {
                        var data = (SafeInvokeCallback)e;
                        ((Action)data.Callback).Invoke();
                    }, new SafeInvokeCallback() { Callback = callback });
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                callback();
            }
        }
        public static void SafeAdd<T>(this ObservableCollection<T> source, T data)
        {
            if (source == null)
            {
                source = new ObservableCollection<T>();
            }
            SafeInvoke(new Action(() =>
            {
                source.Add(data);
            }));

        }
        public static void SafeInsertTop<T>(this ObservableCollection<T> source, T data)
        {
            if (source == null)
            {
                source = new ObservableCollection<T>();
            }
            SafeInvoke(new Action(() =>
            {
                source.Insert(0, data);
            }));

        }
        public static void SafeAddRange<T>(this ObservableCollection<T> source, IEnumerable<T> data)
        {
            if (source == null)
            {
                source = new ObservableCollection<T>();
            }
            SafeInvoke(new Action(() =>
            {
                foreach (var item in data)
                {
                    source.Add(item);
                }

            }));
        }
        public static void SafeRemove<T>(this ObservableCollection<T> source, T data)
        {
            if (source == null)
            {
                source = new ObservableCollection<T>();
            }
            SafeInvoke(new Action(() =>
            {
                source.Remove(data);
            }));

        }
        public static void SafeClear<T>(this ObservableCollection<T> source)
        {
            if (source == null)
            {
                source = new ObservableCollection<T>();
            }
            SafeInvoke(new Action(() =>
            {
                source.Clear();
            }));

        }

    }
}
