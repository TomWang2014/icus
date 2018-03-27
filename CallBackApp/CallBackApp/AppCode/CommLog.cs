using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallBackApp.AppCode
{
    public class CommLog
    {
        private static object synchronous = new object();

        public static void WriteSystemLog(string log)
        {
            lock (synchronous)
            {
                string fileDayName = DateTime.Now.ToString("yyyyMMdd") + "_Log.txt";
                string filepath = AppDomain.CurrentDomain.BaseDirectory + "Logs/回调";
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
                catch (Exception e)
                {

                }
            }
        }

        public static void WriteExceptionLog(string log)
        {
            lock (synchronous)
            {
                string fileDayName = DateTime.Now.ToString("yyyyMMdd") + "_Exception.txt";
                string filepath = AppDomain.CurrentDomain.BaseDirectory + "Logs/回调";
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
