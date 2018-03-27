namespace ICusCRM.Repository.Systems
{
    using ICusCRM.Domain;
    using ICusCRM.Domain.Systems;
    using ICusCRM.Infrastructure.Data;

    /// <summary>
    /// 角色管理仓储
    /// </summary>
    public class RolesRepository : RepositoryBase<Roles>, IRolesRepository
    {
        /// <summary>
        /// 够着函数
        /// </summary>
        /// <param name="context">数据库上下文</param>
        public RolesRepository(AppContext context)
            : base(context)
        {
        }
    }
}
