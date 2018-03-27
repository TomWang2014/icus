// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHasModifyTime.cs" company="">
//   
// </copyright>
// <author>�����</author>
// <summary>
//   ��׼���ӿڣ�Ҫ�����޸�ʱ��(CreationTime)
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ICusCRM.Infrastructure.Entity.Auditing
{
    using System;

    /// <summary>
    /// ��׼���ӿڣ�Ҫ�����޸�ʱ��(CreationTime)
    /// An entity can implement this interface if <see cref="IHasModifyTime"/> of this entity must be stored.
    /// <see cref="IHasModifyTime"/> can be automatically set when saving <see cref="Entity"/> to database.
    /// </summary>
    public interface IHasModifyTime
    {
        /// <summary>
        /// ����ʱ��
        /// Creation time of this entity.
        /// </summary>
        DateTime ModifyTime { get; set; }
    }
}