// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Log.cs" company="zjzx">
//   2016-11-8 18:01:08
// </copyright>
// <author>李天赐</author>
// <date>2016/7/6 17:17:43</date>
// <summary>
//   主要功能有：
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ICusCRM.Infrastructure
{
    using System;
    using System.IO;

    /// <summary>
    /// Log
    /// </summary>
    public class Log
    {
        /// <summary>
        /// The lockerlogwrite.
        /// </summary>
        private static readonly object Lockerlogwrite = new object();

        /// <summary>
        /// WriteLine
        /// </summary>
        /// <param name="message">
        /// message 
        /// </param>
        public static void WriteLine(string message)
        {
            lock (Lockerlogwrite)
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + "log/系统异常";
                var fullpath = path + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                File.AppendAllText(fullpath, message + "\r\n      -----" + DateTime.Now + "\r\n");
            }
        }

        /// <summary>
        /// WriteLine
        /// </summary>
        /// <param name="exception">
        /// exception 
        /// </param>
        public static void WriteLine(Exception exception)
        {
            lock (Lockerlogwrite)
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + "log/系统异常";
                var fullpath = path + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var message = string.Format(
                        "OnException {0}:\r\n{1}",
                        exception.Message,
                        exception.StackTrace);

                File.AppendAllText(fullpath, message + "\r\n      -----" + DateTime.Now + "\r\n");

                if (exception.InnerException != null)
                {
                    WriteLine(exception.InnerException);
                }
            }
        }

        /// <summary>
        /// 记录异常
        /// </summary>
        /// <param name="directoryName">
        /// 文件夹名称 
        /// </param>
        /// <param name="message">
        /// 日志内容 
        /// </param>
        public static void WriteLine(string directoryName, string message)
        {
            lock (Lockerlogwrite)
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + "log/" + directoryName;
                var fullpath = path + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                File.AppendAllText(fullpath, message + "\r\n      -----" + DateTime.Now + "\r\n");
            }
        }
    }
}
