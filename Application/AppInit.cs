// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppInit.cs" company="zjzx">
//   ©2016 中教在线 版权所有
// </copyright>
// <summary>
//   Defines the HomeController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ICusCRM.Application
{
    using ICusCRM.Infrastructure.AutoMapper;
    using ICusCRM.Infrastructure.Unity.Aop;
    using ICusCRM.Infrastructure.Unity.Ioc;

    /// <summary>
    /// Application初始化器
    /// </summary>
    public class AppInit
    {
        /// <summary>
        /// 运行初始化操作
        /// </summary>
        public static void Run()
        {
            // 启动Ioc
            var ico = IocManager.Instance;

            // 启动Aop
            var interceptionInitializer = new InterceptionRegister();
            interceptionInitializer.Initialize();

            // 启动automapper
            var initializer = new AutoMapperInitializer();
            initializer.Initialize();
        }
    }
}
