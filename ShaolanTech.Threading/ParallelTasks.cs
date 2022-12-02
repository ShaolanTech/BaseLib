using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShaolanTech.Threading
{
    public static class ParallelTasks
    {
        /// <summary>
        /// Loop an ienumerable object parallelly
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="callback">Call function in a thread</param>
        /// <param name="threadCount">max parallel count,default 0</param>
        public static void ParallelForeach<T>(this IEnumerable<T> source, Func<T, Task> callback, int threadCount = 0)
        {
            var t = threadCount;
            if (t == 0)
            {
                t = -1;
            }
            Parallel.ForEach(source, new ParallelOptions { MaxDegreeOfParallelism = t }, (s) =>
            {
                try
                {
                    var task = callback(s);
                    task.Wait();
                }
                catch (Exception ex)
                {


                }
            });

        }
        /// <summary>
        /// Loop an ienumerable object parallelly
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="callback">Call function in a thread</param>
        /// <param name="threadCount">max parallel count,default 0</param>
        public static void ParallelForeach<T>(this IEnumerable<T> source, Action<T> callback, int threadCount = 0)
        {
            var t = threadCount;
            if (t == 0)
            {
                t = -1;
            }
            Parallel.ForEach(source, new ParallelOptions { MaxDegreeOfParallelism = t }, (s) =>
            {
                try
                {
                    callback(s);
                }
                catch (Exception ex)
                {


                }
            });
        }


        /// <summary>
        /// Loop an ienumerable object parallelly
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="callback">Call function in a thread</param>
        /// <param name="threadCount">max parallel count,default 0</param>
        public static async Task ParallelForeachAsync<T>(this IEnumerable<T> source, Func<T, Task> callback, int threadCount = 0)
        {
            await Task.Run(() =>ParallelForeach(source,callback,threadCount));
        }
        /// <summary>
        /// Loop an ienumerable object parallelly
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="callback">Call function in a thread</param>
        /// <param name="threadCount">max parallel count,default 0</param>
        public static async Task ParallelForeachAsync<T>(this IEnumerable<T> source, Action<T> callback, int threadCount = 0)
        {
            await Task.Run(() => ParallelForeach(source, callback, threadCount));
        }
    }
}
