using CallBackApp.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CallBackApp.AppCode
{
    public static class LogicClass
    {
        /// <summary>
        /// 开启定时器
        /// </summary>
        public static void EnableTimer()
        {
            var timer = new Timer();
            timer.Elapsed += CallBackInterface;//执行方法
            timer.Interval = (6 * 1000) * Convert.ToInt32(ConfigurationManager.AppSettings["SelectInterval"]);//间隔时间
            timer.Enabled = true;
        }
        /// <summary>
        /// 定时器执行方法
        /// </summary>   
        private static void CallBackInterface(object sender, ElapsedEventArgs e)
        {
            CommLog.WriteSystemLog("开始本次执行...");
            BasicClass basic = new BasicClass();
            //查询数据库中前100条需要回调的记录
            DataTable dt_records = basic.GetCallbackRecords();
            if (dt_records != null && dt_records.Rows.Count > 0)
            {

                string url = string.Empty;//接口地址
                int tenantId = 0;//租户id                
                string appSecret = string.Empty;//密钥
                string studentNo = string.Empty;//学号
                string projectCode = string.Empty;//项目编号
                int type = -1;//修改类型:  0修改审核状态  1修改学员状态（冻结/恢复）
                int value = -1;//修改值：type为0时即修改审核状态 0审核中 1审核通过 2审核未通过   type为1时即修改学员状态  0恢复 1冻结 
                string msg = string.Empty;//修改状态说明文字 
                int sendTimes = 0;//推送次数
                long id = 0;
                int classId = 0;//班级id
                DataTable dt_tenant = null;
                string sendResult = string.Empty;
                for (int i = 0; i < dt_records.Rows.Count; i++)
                {
                    tenantId = Convert.ToInt32(dt_records.Rows[i]["TenantId"]);
                    id = Convert.ToInt64(dt_records.Rows[i]["Id"]);
                    if (tenantId > 0)
                    {
                        dt_tenant = basic.GetCallbackParams(tenantId);
                        if (dt_tenant != null && dt_tenant.Rows.Count > 0)
                        {
                            url = dt_tenant.Rows[0]["TokenUrl"].ToString();
                            appSecret = dt_tenant.Rows[0]["AesKey"].ToString();
                        }
                    }
                    else
                    {
                        continue;
                    }
                    type = Convert.ToInt32(dt_records.Rows[i]["Type"]);
                    value = Convert.ToInt32(dt_records.Rows[i]["Value"]);
                    msg = dt_records.Rows[i]["ExplainMsg"].ToString();
                    sendTimes = Convert.ToInt32(dt_records.Rows[i]["SendTimes"]);
                    studentNo = dt_records.Rows[i]["StudentNo"].ToString();
                    projectCode = dt_records.Rows[i]["ProjectCode"].ToString();
                    classId =dt_records.Rows[i].IsNull("ClassId")?0: Convert.ToInt32(dt_records.Rows[i]["ClassId"]);
                    if (!string.IsNullOrEmpty(url) && type > -1 && value > -1 && !string.IsNullOrEmpty(appSecret))
                    {
                        sendResult = basic.PostDataUrl(url, type, value, msg, appSecret, studentNo, projectCode,classId);
                        if (!string.IsNullOrEmpty(sendResult))
                        {
                            var postResult = basic.JsonStr2Obj<PostResult>(sendResult);
                            if (postResult != null)
                            {
                                //推送成功
                                basic.UpdateSendStatus(1, id, sendTimes);
                            }
                            else
                            {
                                //推送失败
                                basic.UpdateSendStatus(0, id, sendTimes);
                            }
                        }
                        else
                        {
                            //推送失败
                            basic.UpdateSendStatus(0, id, sendTimes);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            CommLog.WriteSystemLog("结束本次执行...");
            CommLog.WriteSystemLog("--------------------------------------------------");

        }
    }
}
