// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionExtensions.cs" company="">
//   
// </copyright>
// <author>�����</author>
// <summary>
//   ���������չ����
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ICusCRM.Infrastructure.Collections.Extensions
{
    using System.Collections.Generic;

    /// <summary>
    /// Extension methods for Collections.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Checks whatever given collection object is null or has no item.
        /// </summary>
        /// <typeparam name="T">
        /// ����Ԫ������
        /// </typeparam>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            return collection == null || collection.Count <= 0;
        }
    }
}