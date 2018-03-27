using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace CallBackApp.AppCode
{
    public class BasicClass
    {
        /// <summary>
        /// 获取需要回调的前100条记录
        /// </summary>
        /// <returns></returns>
        public DataTable GetCallbackRecords()
        {
            DataTable dt = null;
            string sqlStr = string.Empty;
            try
            {
                sqlStr = "select top 100 * from dbo.NetInterface with(nolock) where [Status]=0 and SendTimes<3 order by Id asc";
                dt = SqlHelper.ExecuteTable(SqlHelper.connString, sqlStr);
            }
            catch (Exception ex)
            {
                CommLog.WriteExceptionLog("获取需要回调的前100条记录时出现异常：" + ex.Message + ";  sql=" + sqlStr);
                dt = null;
            }
            return dt;
        }
        /// <summary>
        /// 根据租户id获取租户的回调所需的信息
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public DataTable GetCallbackParams(int tenantId)
        {
            DataTable dt = null;
            string sqlStr = string.Empty;
            try
            {
                sqlStr = string.Format("select top 1 TokenUrl,AesKey from dbo.NetTenant with(nolock) where IsDelete=0 and Id={0} Order by Id desc", tenantId);
                dt = SqlHelper.ExecuteTable(SqlHelper.connString, sqlStr);
            }
            catch (Exception ex)
            {
                CommLog.WriteExceptionLog("根据租户id获取租户的回调所需的信息时出现异常：" + ex.Message + "; sql=" + sqlStr);
                dt = null;
            }
            return dt;
        }
        /// <summary>
        /// 调用租户接口推送数据
        /// </summary>
        /// <param name="url">租户接口url</param>
        /// <param name="type">修改值</param>
        /// <param name="value">修改类型</param>
        /// <param name="msg">文字说明</param>
        /// <param name="appId">应用接入id</param>
        /// <param name="appSecret">密钥</param>
        /// <returns></returns>
        public string PostDataUrl(string url, int type, int value, string msg, string appSecret, string studentNo, string projectCode,int classId)
        {
            string result = string.Empty;
            string postString = string.Empty;


            try
            {
                string timeStamp = GetTimeStamp().ToString();//时间戳      
                string nonce = new Random().NextDouble().ToString();//随机数
                string signature = GetSignature(appSecret, timeStamp, nonce);//签名加密字符串


                System.Net.WebClient WebClientObj = new System.Net.WebClient();
                WebClientObj.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                NameValueCollection PostVars = new NameValueCollection();
                postString = "type=" + HttpUtility.UrlEncode(type.ToString(), UTF8Encoding.UTF8) + "&value=" + HttpUtility.UrlEncode(value.ToString(), UTF8Encoding.UTF8) + "&message=" + HttpUtility.UrlEncode(msg, UTF8Encoding.UTF8) + "&signature=" + HttpUtility.UrlEncode(signature, UTF8Encoding.UTF8) + "&timestamp=" + HttpUtility.UrlEncode(timeStamp, UTF8Encoding.UTF8) + "&nonce=" + HttpUtility.UrlEncode(nonce, UTF8Encoding.UTF8) + "&studentno=" + HttpUtility.UrlEncode(studentNo, UTF8Encoding.UTF8) + "&projectcode=" + HttpUtility.UrlEncode(projectCode, UTF8Encoding.UTF8) + "&classsid=" + HttpUtility.UrlEncode(classId.ToString(), UTF8Encoding.UTF8);
                byte[] postData = Encoding.UTF8.GetBytes(postString);
                byte[] responseData = WebClientObj.UploadData(url, "POST", postData);
                result = Encoding.UTF8.GetString(responseData);

                CommLog.WriteSystemLog("推送结果result=" + result + ";posturl=" + url + "?" + postString);

            }
            catch (Exception ex)
            {
                CommLog.WriteExceptionLog("调用租户接口推送数据时出现异常：" + ex.Message + ";posturl=" + url + "?" + postString);
                result = string.Empty;
            }
            return result;
        }
        public string PostDataUrlTest()
        {
            string result = string.Empty;
            string postString = string.Empty;
            string url = "http://edu.test.cn/api/Reform/ModifyStatus";
            int type = 1;
            int value = 1;
            string msg = "审核通过";
            string appSecret = "123456";
            string studentNo = "161229000027";
            string projectCode = "PS83312204";



            try
            {

                string timeStamp = GetTimeStamp().ToString();//时间戳      
                string nonce = new Random().NextDouble().ToString();//随机数
                string signature = GetSignature(appSecret, timeStamp, nonce);//签名加密字符串


                System.Net.WebClient WebClientObj = new System.Net.WebClient();
                WebClientObj.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                NameValueCollection PostVars = new NameValueCollection();
                postString = "type=" + HttpUtility.UrlEncode(type.ToString(), UTF8Encoding.UTF8) + "&value=" + HttpUtility.UrlEncode(value.ToString(), UTF8Encoding.UTF8) + "&message=" + HttpUtility.UrlEncode(msg, UTF8Encoding.UTF8) + "&signature=" + HttpUtility.UrlEncode(signature, UTF8Encoding.UTF8) + "&timestamp=" + HttpUtility.UrlEncode(timeStamp, UTF8Encoding.UTF8) + "&nonce=" + HttpUtility.UrlEncode(nonce, UTF8Encoding.UTF8) + "&studentno=" + HttpUtility.UrlEncode(studentNo, UTF8Encoding.UTF8) + "&projectcode=" + HttpUtility.UrlEncode(projectCode, UTF8Encoding.UTF8);
                byte[] postData = Encoding.UTF8.GetBytes(postString);
                byte[] responseData = WebClientObj.UploadData(url, "POST", postData);
                result = Encoding.UTF8.GetString(responseData);

                CommLog.WriteSystemLog("推送结果result=" + result + ";posturl=" + url + "?" + postString);

            }
            catch (Exception ex)
            {
                CommLog.WriteExceptionLog("调用租户接口推送数据时出现异常：" + ex.Message + ";posturl=" + url + "?" + postString);
                result = string.Empty;
            }
            return result;
        }

        /// <summary>
        /// 获取签名加密字符串
        /// </summary>
        /// <param name="appSecret"></param>
        /// <param name="timeStamp"></param>
        /// <param name="studentNo"></param>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        private string GetSignature(string appSecret, string timeStamp, string nonce)
        {
            string[] ArrTmp = { appSecret, timeStamp, nonce };

            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);

            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "MD5");
            return tmpStr.ToLower();
        }

        /// <summary>
        ///获取时间戳 
        /// </summary>
        /// <returns></returns>
        private long GetTimeStamp()
        {
            DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(DateTime.Now - UnixEpoch).TotalSeconds;
        }
        /// <summary>
        /// 推送成功或失败后，更新NetInterface记录状态
        /// </summary>
        /// <param name="type">1发送成功 0发送失败</param>
        /// <param name="id">记录id</param>
        /// <param name="sendTimes">发送次数</param>
        public void UpdateSendStatus(int type, long id, int sendTimes)
        {
            string sqlStr = string.Empty;
            try
            {
                if (type == 1)
                {
                    //发送成功
                    if (sendTimes == 0)
                    {
                        //首次发送
                        sqlStr = string.Format("update dbo.NetInterface set [Status]=1,SendTime='{0}',SendTimes=1 where Id={1}", DateTime.Now.ToString(), id);
                    }
                    else
                    {
                        //重复发送
                        sqlStr = string.Format("update dbo.NetInterface set [Status]=1,UpdateTime='{0}',SendTimes=SendTimes+1 where Id={1}", DateTime.Now.ToString(), id);
                    }

                }
                else
                {
                    //发送失败
                    if (sendTimes == 0)
                    {
                        //发送次数加1 修改发送时间
                        sqlStr = string.Format("update dbo.NetInterface set SendTime='{0}',SendTimes=1 where Id={1}", DateTime.Now.ToString(), id);
                    }
                    else
                    {
                        if (sendTimes < 2)
                        {
                            //发送次数加1 修改更新时间
                            sqlStr = string.Format("update dbo.NetInterface set UpdateTime='{0}',SendTimes=SendTimes+1 where Id={1}", DateTime.Now.ToString(), id);
                        }
                        else
                        {
                            //发送次数加1，修改更新时间，将状态置为2
                            sqlStr = string.Format("update dbo.NetInterface set [Status]=2,UpdateTime='{0}',SendTimes=SendTimes+1 where Id={1}", DateTime.Now.ToString(), id);
                        }
                    }
                }

                SqlHelper.ExecuteNonQuery(SqlHelper.connString, CommandType.Text, sqlStr);
            }
            catch (Exception ex)
            {
                CommLog.WriteExceptionLog("推送成功或失败后，更新NetInterface记录状态时出现异常：" + ex.Message + ";  sql=" + sqlStr);
            }



        }


        /// <summary>
        /// 将Json字符串转化为对象  
        /// </summary>
        /// <param name="strJson">
        /// Json字符串
        /// </param>
        /// <returns>
        /// 目标对象
        /// </returns>
        public T JsonStr2Obj<T>(string strJson)
        {
            var serialize = new JavaScriptSerializer();
            return serialize.Deserialize<T>(strJson);
        }
    }
}
