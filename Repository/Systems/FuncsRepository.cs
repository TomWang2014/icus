namespace ICusCRM.Repository.Systems
{
    using ICusCRM.Domain;
    using ICusCRM.Domain.Systems;
    using ICusCRM.Infrastructure.Data;

    /// <summary>
    /// ResDataCategoryRepository
    /// </summary>
    public class FuncsRepository : RepositoryBase<Funcs>, IFuncsRepository
    {
        /// <summary>
        /// 够着函数
        /// </summary>
        /// <param name="context">数据库上下文</param>
        public FuncsRepository(AppContext context)
            : base(context)
        {
        }
    }
}
