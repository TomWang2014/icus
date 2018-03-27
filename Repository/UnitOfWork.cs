﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitOfWork.cs" company="WIKI Tec">
//   WIKI Tec copyright.
// </copyright>
// <author>李天赐</author>
// <date>2015/3/17 17:16:30</date>
// <summary>
//   主要功能有：
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ICusCRM.Repository
{
    using System;

    using ICusCRM.Infrastructure.Data;

    /// <summary>
    /// 数据库提交辅助类
    /// </summary>
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        private readonly AppContext dataContext;

        /// <summary>
        /// 在构造函数中设置上下文对象
        /// </summary>
        /// <param name="entityDataContext">EntityDataContext</param>
        public UnitOfWork(AppContext entityDataContext)
        {
            this.dataContext = entityDataContext;
        }

        /// <summary>
        /// 数据上下文(只读)
        /// </summary>
        protected AppContext DataContext
        {
            get
            {
                return this.dataContext;
            }
        }

        /// <summary>
        /// 向提交数据更改
        /// </summary>
        public void Commit()
        {
            this.DataContext.SaveChanges();
        }

        /// <summary>
        /// 释放当前上下文对象
        /// </summary>
        public void Dispose()
        {
            if (null != this.dataContext)
            {
                this.dataContext.Dispose();
            }
        }
    }

}
