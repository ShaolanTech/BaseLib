using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShaolanTech.Threading
{
    /// <summary>
    /// 批量操作管理器
    /// </summary>
    public class BatchOperation<T> : IDisposable
    {
        public bool EnableLock { get; set; } = true;
        /// <summary>
        /// 每次批量处理完毕后，是否自动回收内存，默认为True
        /// </summary>
        public bool AutoClear { get; set; } = true;
        /// <summary>
        /// 是否自动回收内存
        /// </summary>
        public bool AutoCollect { get; set; } = true;
        /// <summary>
        /// 当批量处理队列已经达上限时，进行处理的回调函数
        /// </summary>
        public Action<List<T>> BatchOperationCallback { get; set; }
        /// <summary>
        /// 当批量处理队列已经达上限时，进行处理的回调函数
        /// </summary>
        public Func<List<T>, Task> BatchOperationCallbackAsync { get; set; }
        /// <summary>
        /// 当队列元素数量达到指定数值时，将进行批量处理
        /// </summary>
        public int BatchCount { get; set; } = 1000;
        private List<T> list = new List<T>();
        System.Threading.ManualResetEvent signal = new System.Threading.ManualResetEvent(true);
        /// <summary>
        /// 将一个元素加入队列
        /// </summary>
        /// <param name="item"></param>
        public void Append(T item)
        {
            signal.WaitOne();
            if (this.EnableLock)
            {
                lock (this.list)
                {
                    this.list.Add(item);
                    if (this.list.Count == this.BatchCount)
                    {
                        this.Flush();
                    }

                }
            }
            else
            {
                this.list.Add(item);
                if (this.list.Count == this.BatchCount)
                {
                    this.Flush();
                }
            }

        }
        /// <summary>
        /// 将一组元素加入队列
        /// </summary>
        /// <param name="items"></param>
        public void Append(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                this.Append(item);
            }
        }
        public void Flush()
        {
            this.signal.Reset();

            if (this.BatchOperationCallback == null && this.BatchOperationCallbackAsync == null)
            {
                throw new Exception("需要将函数指针BatchOperationCallback指向一个回调函数");
            }
            if (this.list.Count != 0)
            {

                if (this.BatchOperationCallback != null)
                {
                    this.BatchOperationCallback(this.list);
                }
                else
                {
                    if (this.BatchOperationCallbackAsync != null)
                    {
                        var task = this.BatchOperationCallbackAsync(this.list);
                        task.Wait();
                    }
                }

            }
            if (this.AutoClear)
            {
                if (this.AutoCollect)
                {
                    foreach (var item in this.list)
                    {
                        if (item is IDisposable)
                        {
                            ((IDisposable)item).Dispose();
                        }
                    }
                }


                //

            }
            this.list.Clear();
            if (this.AutoCollect)
            {
                GC.Collect();
            }
            this.signal.Set();
        }
        public void Dispose()
        {
            lock (this.list)
            {
                this.Flush();
            }

        }
    }

    /// <summary>
    /// 批量操作管理器
    /// </summary>
    public class BatchOperationBySize<T> : IDisposable
    {
        public bool EnableLock { get; set; } = true;
        /// <summary>
        /// 每次批量处理完毕后，是否自动回收内存，默认为True
        /// </summary>
        public bool AutoClear { get; set; } = true;
        /// <summary>
        /// 是否自动回收内存
        /// </summary>
        public bool AutoCollect { get; set; } = true;
        /// <summary>
        /// 当批量处理队列已经达上限时，进行处理的回调函数
        /// </summary>
        public Action<List<T>> BatchOperationCallback { get; set; }
        /// <summary>
        /// 当批量处理队列已经达上限时，进行处理的回调函数
        /// </summary>
        public Func<List<T>, Task> BatchOperationCallbackAsync { get; set; }
        /// <summary>
        /// 当队列元素数量达到指定数值时，将进行批量处理
        /// </summary>
        public long BatchCount { get; set; } = 1024*1024*4;
        private List<T> list = new List<T>();
        System.Threading.ManualResetEvent signal = new System.Threading.ManualResetEvent(true);
        /// <summary>
        /// 将一个元素加入队列
        /// </summary>
        /// <param name="item"></param>
        public void Append(T item)
        {
            signal.WaitOne();
            if (this.EnableLock)
            {
                lock (this.list)
                {
                    this.list.Add(item);
                    if (this.list.Count == this.BatchCount)
                    {
                        this.Flush();
                    }

                }
            }
            else
            {
                this.list.Add(item);
                if (this.list.Count == this.BatchCount)
                {
                    this.Flush();
                }
            }

        }
        /// <summary>
        /// 将一组元素加入队列
        /// </summary>
        /// <param name="items"></param>
        public void Append(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                this.Append(item);
            }
        }
        public void Flush()
        {
            this.signal.Reset();

            if (this.BatchOperationCallback == null && this.BatchOperationCallbackAsync == null)
            {
                throw new Exception("需要将函数指针BatchOperationCallback指向一个回调函数");
            }
            if (this.list.Count != 0)
            {

                if (this.BatchOperationCallback != null)
                {
                    this.BatchOperationCallback(this.list);
                }
                else
                {
                    if (this.BatchOperationCallbackAsync != null)
                    {
                        var task = this.BatchOperationCallbackAsync(this.list);
                        task.Wait();
                    }
                }

            }
            if (this.AutoClear)
            {
                if (this.AutoCollect)
                {
                    foreach (var item in this.list)
                    {
                        if (item is IDisposable)
                        {
                            ((IDisposable)item).Dispose();
                        }
                    }
                }


                //

            }
            this.list.Clear();
            if (this.AutoCollect)
            {
                GC.Collect();
            }
            this.signal.Set();
        }
        public void Dispose()
        {
            lock (this.list)
            {
                this.Flush();
            }

        }
    }
}
