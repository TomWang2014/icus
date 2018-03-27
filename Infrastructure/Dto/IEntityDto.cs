// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEntityDto.cs" company="WIKI Tec">
//   WIKI Tec copyright.
// </copyright>
// <author>������</author>
// <summary>
//   IEntityDto
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ICusCRM.Infrastructure.Dto
{
    /// <summary>
    /// A shortcut of <see cref="IEntityDto{TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// </summary>
    public interface IEntityDto : IEntityDto<int>
    {
    }
}