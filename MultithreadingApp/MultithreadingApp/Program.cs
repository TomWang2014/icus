using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultithreadingApp.AppCode;
using System.Threading;

namespace MultithreadingApp
{
    class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                //处理添加/修改学员信息操作表
                Thread thread0 = new Thread(new ThreadStart(LogicClass.Deal0));
                thread0.Start();
                //处理学员报名项目操作表
                Thread thread1 = new Thread(new ThreadStart(LogicClass.Deal1));
                thread1.Start();
                //处理学员支付操作表
                Thread thread2 = new Thread(new ThreadStart(LogicClass.Deal2));
                thread2.Start();

                //处理学员学习操作表
                Thread thread3 = new Thread(new ThreadStart(LogicClass.Deal3));
                thread3.Start();

                //处理学员考试操作表
                Thread thread4 = new Thread(new ThreadStart(LogicClass.Deal4));
                thread4.Start();
                
            }
            catch (Exception ex)
            {
                CommLog.WriteExceptionLog("程序出现异常：" + ex.Message, "Main");
            }
        }
    }
}
