// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHasCreationTime.cs" company="">
//   
// </copyright>
// <author>�����</author>
// <summary>
//   ��׼���ӿڣ�Ҫ���д���ʱ��(CreationTime)
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ICusCRM.Infrastructure.Entity.Auditing
{
    using System;

    /// <summary>
    /// ��׼���ӿڣ�Ҫ���д���ʱ��(CreationTime)
    /// An entity can implement this interface if <see cref="CreateTime"/> of this entity must be stored.
    /// <see cref="CreateTime"/> can be automatically set when saving <see cref="Entity"/> to database.
    /// </summary>
    public interface IHasCreationTime
    {
        /// <summary>
        /// ����ʱ��
        /// Creation time of this entity.
        /// </summary>
        DateTime CreateTime { get; set; }
    }
}