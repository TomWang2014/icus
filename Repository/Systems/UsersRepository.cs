namespace ICusCRM.Repository.Systems
{
    using System.Collections.Generic;
    using System.Linq;

    using ICusCRM.Domain;
    using ICusCRM.Domain.Systems;
    using ICusCRM.Infrastructure.Data;

    /// <summary>
    /// 用户管理仓储
    /// </summary>
    public class UsersRepository : RepositoryBase<Users>, IUsersRepository
    {
        /// <summary>
        /// 够着函数
        /// </summary>
        /// <param name="context">数据库上下文</param>
        public UsersRepository(AppContext context)
            : base(context)
        {
        }
    }
}
