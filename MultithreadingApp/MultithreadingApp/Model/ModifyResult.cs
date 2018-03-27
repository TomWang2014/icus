using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadingApp.Model
{
    public class ModifyResult
    {
        /// <summary>
        /// 接口返回结果码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 接口返回结果文字说明
        /// </summary>
        public string Message { get; set; }
    }
}
