// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeFormatDto.cs" company="">
//   
// </copyright>
// <author>������</author>
// <summary>
//   NameValueDto
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ICusCRM.Infrastructure.Entity
{
    using System;

    /// <summary>
    /// Can be used to send/receive Name/Value (or Key/Value) pairs.
    /// </summary>
    [Serializable]
    public class DateTimeFormatDto
    {
        /// <summary>
        /// Creates a new <see cref="NameValueDto"/>.
        /// </summary>
        public DateTimeFormatDto()
        {
        }

        /// <summary>
        /// �����ʽ yyyy/MM/dd
        /// </summary>
        public string I { get; set; }

        /// <summary>
        /// �����ʽ HH:mm
        /// </summary>
        public string M { get; set; }

        /// <summary>
        /// �����ʽ yyy-MM-dd HH:mm
        /// </summary>
        public string S { get; set; }
    }
}