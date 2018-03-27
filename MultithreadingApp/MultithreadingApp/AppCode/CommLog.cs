using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadingApp.AppCode
{
    /// <summary>
    /// 日志类
    /// </summary>
    public class CommLog
    {
        private static object synchronous = new object();
        /// <summary>
        /// 系统日志
        /// </summary>
        /// <param name="log"></param>
        /// <param name="pathName"></param>
        public static void WriteSystemLog(string log, string pathName)
        {
            lock (synchronous)
            {
                string fileDayName = DateTime.Now.ToString("yyyyMMdd") + "_Log.txt";
                string filepath = AppDomain.CurrentDomain.BaseDirectory + "Logs/处理操作表日志/" + pathName;
                try
                {
                    if (!Directory.Exists(string.Format("{0}", filepath)))
                    {
                        Directory.CreateDirectory(string.Format("{0}", filepath));
                    }

                    FileStream fs = new FileStream(string.Format("{0}\\{1}", filepath, fileDayName), FileMode.Append);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(string.Format("{0} {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), log));
                    Console.WriteLine(string.Format("{0} {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), log));
                    sw.Close();
                    fs.Close();
                    fs.Dispose();
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="log"></param>
        /// <param name="pathName"></param>
        public static void WriteExceptionLog(string log, string pathName)
        {
            lock (synchronous)
            {
                string fileDayName = DateTime.Now.ToString("yyyyMMdd") + "_Exception.txt";
                string filepath = AppDomain.CurrentDomain.BaseDirectory + "Logs/处理操作表日志/" + pathName;
                try
                {
                    if (!Directory.Exists(string.Format("{0}", filepath)))
                    {
                        Directory.CreateDirectory(string.Format("{0}", filepath));
                    }

                    FileStream fs = new FileStream(string.Format("{0}\\{1}", filepath, fileDayName), FileMode.Append);

                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(string.Format("{0}", log));
                    Console.WriteLine(string.Format("{0}", log));
                    sw.WriteLine("---------------------------------------------------------");
                    sw.Close();
                    fs.Close();
                    fs.Dispose();
                }
                catch
                {

                }
            }
        }
    }
}
