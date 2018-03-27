// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEntityDtoOfPrimaryKey.cs" company="WIKI Tec">
//   WIKI Tec copyright.
// </copyright>
// <author>������</author>
// <summary>
//   IEntityDtoOfPrimaryKey
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ICusCRM.Infrastructure.Dto
{
    /// <summary>
    /// Defines common properties for entity based DTOs.
    /// </summary>
    /// <typeparam name="TPrimaryKey">��������</typeparam>
    public interface IEntityDto<TPrimaryKey> : IDto
    {
        /// <summary>
        /// Id of the entity.
        /// </summary>
        TPrimaryKey Id { get; set; }
    }
}