using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadingApp.Model
{
    public class SubmitRecordResult
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public List<RecordResult> Success { get; set; }
        public List<RecordResult> Failed { get; set; }
    }
}
