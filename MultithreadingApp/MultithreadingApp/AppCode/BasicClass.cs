using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace MultithreadingApp.AppCode
{
    /// <summary>
    /// 操作数据库类
    /// </summary>
    public class BasicClass
    {
        #region 公用方法
        /// <summary>
        /// aes加密
        /// </summary>
        /// <param name="toEncrypt">需要加密的字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">偏移量</param>
        /// <returns>加密后的字符串</returns>
        public string Encrypt(string toEncrypt, string key, string iv)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] ivArray = UTF8Encoding.UTF8.GetBytes(iv);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.Zeros;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }


        /// <summary>
        /// 调用接口推送数据
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="projectCode">项目编号</param>
        /// <param name="encryption">接口参数加密串</param>
        /// <returns></returns>
        public string PostData(string url, string projectCode, string encryption)
        {
            string result = string.Empty;
            string postString = string.Empty;
            try
            {
                System.Net.WebClient WebClientObj = new System.Net.WebClient();
                WebClientObj.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                postString = "projectCode=" + HttpUtility.UrlEncode(projectCode, UTF8Encoding.UTF8) + "&encryption=" + HttpUtility.UrlEncode(encryption, UTF8Encoding.UTF8);
                byte[] postData = Encoding.UTF8.GetBytes(postString);
                byte[] responseData = WebClientObj.UploadData(url, "POST", postData);
                result = Encoding.UTF8.GetString(responseData);
            }
            catch (Exception ex)
            {
                result = string.Empty;
                CommLog.WriteExceptionLog("调用接口数据时出现异常：" + ex.Message + "; Url=" + url + ";projectCode=" + projectCode + ";encryption=" + encryption, "Main");
            }
            return result;
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

        /// <summary>
        /// 生成时间戳
        /// </summary>
        /// <returns></returns>
        internal long GetTimeStamp(DateTime time)
        {
            DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(time - UnixEpoch).TotalSeconds;
        }
        #endregion

        #region 添加/修改学员操作表相关
        /// <summary>
        /// 获取添加/修改学员操作表中还未处理的记录
        /// </summary>
        /// <returns></returns>
        internal DataTable GetStudentInfo()
        {
            DataTable dt = null;
            try
            {
                string sqlStr = "SELECT TOP 100 * FROM Reform_Students WHERE Status=0 ORDER BY Id DESC ";
                dt = SqlHelper.ExecuteTable(SqlHelper.connString, sqlStr);
            }
            catch (Exception ex)
            {
                dt = null;
                CommLog.WriteExceptionLog("获取添加/修改学员操作表中还未处理的记录时出现异常：" + ex.Message, "AddModify");
            }
            return dt;
        }




        /// <summary>
        /// 更新学员表中的学号
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="studentNo">学号</param>
        internal void UpdateStudentNo(int id, string studentNo)
        {
            if (id > 0 && !string.IsNullOrEmpty(studentNo))
            {
                string sqlStr = string.Format(@" UPDATE Sys_Users SET StudentNo='{0}' WHERE UserId={1} ", studentNo, id);
                try
                {
                    SqlHelper.ExecuteNonQuery(SqlHelper.connString, CommandType.Text, sqlStr);
                }
                catch (Exception ex)
                {
                    CommLog.WriteExceptionLog("添加学员成功后更新学员表学号时出现异常：" + ex.Message + ";sql=" + sqlStr, "AddModify");
                }
            }
        }

        /// <summary>
        /// 更新支付记录中学号
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="studentNo">学号</param>
        /// <param name="projectCode">项目编号</param>
        internal void UpdatePayStudentNo(int userId, string studentNo, string projectCode)
        {
            if (userId > 0 && !string.IsNullOrEmpty(studentNo) && !string.IsNullOrEmpty(projectCode))
            {
                string sqlStr = string.Format(@" UPDATE Reform_PayRecords SET StudentNo='{0}' WHERE ProjectCode='{1}' AND UserId={2} AND StudentNo IS NULL", studentNo, projectCode, userId);
                try
                {
                    SqlHelper.ExecuteNonQuery(SqlHelper.connString, CommandType.Text, sqlStr);
                }
                catch (Exception ex)
                {
                    CommLog.WriteExceptionLog("添加学员成功后更新支付操作表中学号时出现异常：" + ex.Message + ";sql=" + sqlStr, "AddModify");
                }
            }
        }

        /// <summary>
        /// 添加学员成功后向报名操作表中插入一条未处理的记录
        /// </summary>
        /// <param name="projectCode">项目编号</param>
        /// <param name="studentNo">学号</param>
        /// <param name="url">接口地址</param>
        internal void InsertIntoReformSignUp(string projectCode, string studentNo, string url)
        {
            if (!string.IsNullOrEmpty(projectCode) && !string.IsNullOrEmpty(studentNo) && !string.IsNullOrEmpty(url))
            {
                string sqlStr = string.Format(@"INSERT INTO Reform_SignUpProject(StudentNo, ProjectCode, Url, Status, SendTimes, CreateTime, SignUpStatus) VALUES('{0}','{1}','{2}',{3},{4},'{5}',{6})", studentNo, projectCode, url, 0, 0, DateTime.Now.ToString(), 0);
                try
                {
                    SqlHelper.ExecuteNonQuery(SqlHelper.connString, CommandType.Text, sqlStr);
                }
                catch (Exception ex)
                {
                    CommLog.WriteExceptionLog("添加学员成功后向报名操作表中插入一条未处理的记录时出现异常：" + ex.Message + ";sql=" + sqlStr, "AddModify");
                }
            }
        }

        /// <summary>
        /// 添加/修改学员信息成功后更新操作表的状态
        /// </summary>
        /// <param name="id">操作表id</param>
        /// <param name="studentNo">学号</param>
        internal void UpdateStudentStatus(int id, string studentNo, string code, string message)
        {
            if (id > 0 && !string.IsNullOrEmpty(studentNo))
            {
                string sqlStr = string.Format(@"UPDATE Reform_Students SET StudentNo='{0}',Status=1,SendTimes=SendTimes+1,ResultCode='{1}',ResultMsg='{2}',UpdateTime='{3}' WHERE Id={4}", studentNo, code, message, DateTime.Now.ToString(), id);
                try
                {
                    SqlHelper.ExecuteNonQuery(SqlHelper.connString, CommandType.Text, sqlStr);
                }
                catch (Exception ex)
                {
                    CommLog.WriteExceptionLog("添加或者修改学员信息成功后更新操作表的状态时出现异常：" + ex.Message + ";sql=" + sqlStr, "AddModify");
                }
            }
        }

        /// <summary>
        /// 添加修改学员失败时，修改提交次数
        /// </summary>
        /// <param name="id">操作表id</param>
        /// <param name="code">接口返回结果码</param>
        /// <param name="message">接口返回结果文字说明</param>
        /// <param name="sendTimes">发送次数</param>
        internal void UpdateStudentSendTimes(int id, string code, string message, int sendTimes)
        {
            if (id > 0)
            {
                string sqlStr = string.Empty;
                if (sendTimes < 2)
                {
                    sqlStr = string.Format(@"UPDATE Reform_Students SET SendTimes=SendTimes+1,ResultCode='{0}',ResultMsg='{1}',UpdateTime='{2}' WHERE Id={3}", code, message, DateTime.Now.ToString(), id);
                }
                else
                {
                    sqlStr = string.Format(@"UPDATE Reform_Students SET Status=2,SendTimes=SendTimes+1,ResultCode='{0}',ResultMsg='{1}',UpdateTime='{2}' WHERE Id={3}", code, message, DateTime.Now.ToString(), id);
                }
                try
                {
                    SqlHelper.ExecuteNonQuery(SqlHelper.connString, CommandType.Text, sqlStr);
                }
                catch (Exception ex)
                {
                    CommLog.WriteExceptionLog("添加修改学员失败时，修改提交次数时出现异常：" + ex.Message + ";sql=" + sqlStr, "AddModify");
                }
            }
        }

        #endregion

        #region 学员报名操作表相关

        /// <summary>
        /// 获取学员报名操作表中还未处理的记录
        /// </summary>
        /// <returns></returns>
        internal DataTable GetSignUpInfo()
        {
            DataTable dt = null;
            string sqlStr = "SELECT TOP 100 * FROM Reform_SignUpProject WHERE Status=0 ORDER BY Id DESC ";
            try
            {
                dt = SqlHelper.ExecuteTable(SqlHelper.connString, sqlStr);
            }
            catch (Exception ex)
            {
                dt = null;
                CommLog.WriteExceptionLog(" 获取学员报名操作表中还未处理的记录时出现异常：" + ex.Message + ";Sql=" + sqlStr, "SignUp");
            }
            return dt;
        }


        /// <summary>
        /// 学员报名成功修改操作表状态
        /// </summary>
        /// <param name="id">操作表id</param>
        /// <param name="code">接口返回结果码</param>
        /// <param name="message">接口返回结果说明文档</param>
        /// <param name="signUpStatus">审核状态：0审核中 1审核通过 2审核失败</param>
        internal void UpdateSignUpStatus(int id, string code, string message, string signUpStatus)
        {
            if (id > 0 && !string.IsNullOrEmpty(signUpStatus))
            {
                string sqlStr = string.Format(@"UPDATE Reform_SignUpProject SET Status=1,ResultCode='{0}',ResultMsg='{1}',SendTimes=SendTimes+1,UpdateTime='{2}',SignUpStatus={3} WHERE Id={4}", code, message, DateTime.Now.ToString(), signUpStatus, id);
                try
                {
                    SqlHelper.ExecuteNonQuery(SqlHelper.connString, CommandType.Text, sqlStr);
                }
                catch (Exception ex)
                {
                    CommLog.WriteExceptionLog(" 学员报名成功修改操作表状态时出现异常：" + ex.Message + ";Sql=" + sqlStr, "SignUp");
                }
            }
        }

        /// <summary>
        /// 报名审核通过修改ResCourseEnter表的审核状态
        /// </summary>
        /// <param name="projectCode"></param>
        /// <param name="studentNo"></param>
        internal void UpdateCourseEnterStatus(string projectCode, string studentNo)
        {
            if (!string.IsNullOrEmpty(projectCode) && !string.IsNullOrEmpty(studentNo))
            {
                int courseId = GetCourseIdByProjectCode(projectCode);
                int userId = GetUserIdByStudentNo(studentNo);
                if (userId > 0 && courseId > 0)
                {
                    string sqlStr = string.Format(@"UPDATE Res_CourseEnter SET SignUpStatus=1 WHERE CourseId={0} AND UserId={1}", courseId, userId);
                    try
                    {
                        SqlHelper.ExecuteNonQuery(SqlHelper.connString, CommandType.Text, sqlStr);
                    }
                    catch (Exception ex)
                    {
                        CommLog.WriteExceptionLog(" 报名审核通过修改ResCourseEnter表的审核状态时出现异常：" + ex.Message + ";Sql=" + sqlStr, "SignUp");
                    }
                }
            }
        }

        /// <summary>
        /// 根据学号获取学员id
        /// </summary>
        /// <param name="studentNo">学号</param>
        /// <returns></returns>
        private int GetUserIdByStudentNo(string studentNo)
        {
            int userId = 0;
            string sqlStr = string.Format(@"SELECT TOP 1 UserId FROM Sys_Users WHERE StudentNo='{0}' ORDER BY UserId DESC ", studentNo);
            try
            {
                userId = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.connString, CommandType.Text, sqlStr));
            }
            catch (Exception ex)
            {
                CommLog.WriteExceptionLog(" 根据学号获取学员id时出现异常：" + ex.Message + ";Sql=" + sqlStr, "SignUp");
            }
            return userId;
        }

        /// <summary>
        /// 根据项目编号获取项目id
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        private int GetCourseIdByProjectCode(string projectCode)
        {
            int courseId = 0;
            string sqlStr = string.Format(@"SELECT TOP 1 CourseId FROM Res_Course WHERE ProjectCode='{0}' ORDER BY CourseId DESC", projectCode);
            try
            {
                courseId = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.connString, CommandType.Text, sqlStr));
            }
            catch (Exception ex)
            {
                CommLog.WriteExceptionLog(" 根据项目编号获取项目id时出现异常：" + ex.Message + ";Sql=" + sqlStr, "SignUp");
            }
            return courseId;
        }


        /// <summary>
        /// 报名项目失败后修改sendtimes
        /// </summary>
        /// <param name="id">操作表id</param>
        /// <param name="sendTimes">接口调用累计次数</param>
        /// <param name="code">接口返回结果码</param>
        /// <param name="message">接口返回结果文字说明</param>
        internal void UpdateSignUpSendTimes(int id, int sendTimes, string code, string message)
        {
            if (id > 0)
            {
                string sqlStr = string.Empty;
                if (sendTimes < 2)
                {
                    sqlStr = string.Format(@"UPDATE Reform_SignUpProject SET SendTimes=SendTimes+1,ResultCode='{0}',ResultMsg='{1}',UpdateTime='{2}' WHERE Id={3}", code, message, DateTime.Now.ToString(), id);
                }
                else
                {
                    sqlStr = string.Format(@"UPDATE Reform_SignUpProject SET Status=2,SendTimes=SendTimes+1,ResultCode='{0}',ResultMsg='{1}',UpdateTime='{2}' WHERE Id={3}", code, message, DateTime.Now.ToString(), id);
                }
                try
                {
                    SqlHelper.ExecuteNonQuery(SqlHelper.connString, CommandType.Text, sqlStr);
                }
                catch (Exception ex)
                {
                    CommLog.WriteExceptionLog("学员报名项目失败时，修改提交次数时出现异常：" + ex.Message + ";sql=" + sqlStr, "SignUp");
                }
            }
        }
        #endregion

        #region 支付记录操作表相关

        /// <summary>
        /// 获取支付操作表中所有的项目编号
        /// </summary>
        /// <returns></returns>
        internal DataTable GetPayProjectCode()
        {
            DataTable dt = null;
            string sqlStr = " SELECT ProjectCode FROM Reform_PayRecords GROUP BY ProjectCode ";
            try
            {
                dt = SqlHelper.ExecuteTable(SqlHelper.connString, sqlStr);
            }
            catch (Exception ex)
            {
                CommLog.WriteExceptionLog("获取支付操作表中所有的项目编号时出现异常：" + ex.Message + ";sql=" + sqlStr, "Pay");
            }
            return dt;
        }


        /// <summary>
        /// 获取该项目下还未处理的支付记录
        /// </summary>
        /// <param name="projectCode">项目编号</param>
        /// <returns></returns>
        internal DataTable GetPayRecords(string projectCode)
        {
            DataTable dt = null;
            string sqlStr = string.Format(@"SELECT TOP 100 Id, ProjectCode, StudentNo, PayAmount, PayTime, PayDesc,SendTimes,ClassId FROM Reform_PayRecords WHERE ProjectCode='{0}' AND Status=0", projectCode);
            try
            {
                dt = SqlHelper.ExecuteTable(SqlHelper.connString, sqlStr);
            }
            catch (Exception ex)
            {
                CommLog.WriteExceptionLog("获取该项目下还未处理的支付记录时出现异常：" + ex.Message + ";sql=" + sqlStr, "Pay");
            }
            return dt;
        }

        /// <summary>
        /// 提交支付记录成功修改状态
        /// </summary>
        /// <param name="id">支付记录操作表id</param>
        /// <param name="sucMessage">支付成功文字说明</param>
        internal void UpdatePayRecordStatus(int id, string sucMessage, int sendTimes)
        {
            string sqlStr = string.Empty;
            if (sendTimes == 0)
            {
                sqlStr = string.Format(@"UPDATE Reform_PayRecords SET ResultMsg='{0}',Status=1,SendTimes=1,SendTime='{1}' WHERE Id={2}", sucMessage, DateTime.Now.ToString(), id);
            }
            else
            {
                sqlStr = string.Format(@"UPDATE Reform_PayRecords SET ResultMsg='{0}',Status=1,SendTimes=SendTimes+1,UpdateTime='{1}' WHERE Id={2}", sucMessage, DateTime.Now.ToString(), id);
            }
            try
            {
                SqlHelper.ExecuteNonQuery(SqlHelper.connString, CommandType.Text, sqlStr);
            }
            catch (Exception ex)
            {
                CommLog.WriteExceptionLog("提交支付记录成功修改状态时出现异常：" + ex.Message + ";sql=" + sqlStr, "Pay");
            }
        }


        /// <summary>
        /// 提交支付记录失败后修改支付状态
        /// </summary>
        /// <param name="id">支付记录操作表id</param>
        /// <param name="failMessage">支付记录失败文字说明</param>
        /// <param name="sendTimes">支付记录累计提交次数</param>
        internal void UpdatePaySendTimes(int id, string failMessage, int sendTimes)
        {
            string sqlStr = string.Empty;
            if (sendTimes < 2)
            {
                if (sendTimes == 0)
                {
                    sqlStr = string.Format(@"UPDATE Reform_PayRecords SET ResultMsg='{0}',SendTimes=1,SendTime='{1}' WHERE Id={2}", failMessage, DateTime.Now.ToString(), id);
                }
                else
                {
                    sqlStr = string.Format(@"UPDATE Reform_PayRecords SET ResultMsg='{0}',SendTimes=SendTimes+1,UpdateTime='{1}' WHERE Id={2}", failMessage, DateTime.Now.ToString(), id);
                }
            }
            else
            {
                sqlStr = string.Format(@"UPDATE Reform_PayRecords SET ResultMsg='{0}',Status=2,SendTimes=SendTimes+1,UpdateTime='{1}' WHERE Id={2}", failMessage, DateTime.Now.ToString(), id);
            }
            try
            {
                SqlHelper.ExecuteNonQuery(SqlHelper.connString, CommandType.Text, sqlStr);
            }
            catch (Exception ex)
            {
                CommLog.WriteExceptionLog("提交支付记录失败后修改支付状态时出现异常：" + ex.Message + ";sql=" + sqlStr, "Pay");
            }
        }
        #endregion

        #region 学习记录操作表相关

        /// <summary>
        /// 获取学习记录操作表中项目编号
        /// </summary>
        /// <returns></returns>
        internal DataTable GetLearnProjectCode()
        {
            DataTable dt = null;
            string sqlStr = "SELECT ProjectCode FROM Reform_LearnRecords GROUP BY ProjectCode ";
            try
            {
                dt = SqlHelper.ExecuteTable(SqlHelper.connString, sqlStr);
            }
            catch (Exception ex)
            {
                CommLog.WriteExceptionLog("获取学习记录操作表中项目编号时出现异常：" + ex.Message + ";sql=" + sqlStr, "Learn");
            }
            return dt;
        }

        /// <summary>
        /// 获取学习记录操作表中未处理的记录
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        internal DataTable GetLearnRecords(string projectCode)
        {
            DataTable dt = null;
            string sqlStr = string.Format(@"SELECT TOP 100 Id, ProjectCode, StudentNo, StartTime, EndTime, LearnTime, SendTimes,ClassId FROM Reform_LearnRecords WHERE ProjectCode='{0}' AND Status=0  ", projectCode);
            try
            {
                dt = SqlHelper.ExecuteTable(SqlHelper.connString, sqlStr);
            }
            catch (Exception ex)
            {
                CommLog.WriteExceptionLog("获取学习记录操作表中未处理的记录时出现异常：" + ex.Message + ";sql=" + sqlStr, "Learn");
            }
            return dt;
        }

        /// <summary>
        /// 提交学习记录成功修改状态
        /// </summary>
        /// <param name="id">学习记录操作表id</param>
        /// <param name="sucMessage">接口返回文字说明</param>
        /// <param name="sendTimes">记录累积处理次数</param>
        internal void UpdateLearnRecordStatus(int id, string sucMessage, int sendTimes)
        {
            string sqlStr = string.Empty;
            if (sendTimes == 0)
            {
                sqlStr = string.Format(@"UPDATE Reform_LearnRecords SET ResultMsg='{0}',Status=1,SendTimes=1,SendTime='{1}' WHERE Id={2}", sucMessage, DateTime.Now.ToString(), id);
            }
            else
            {
                sqlStr = string.Format(@"UPDATE Reform_LearnRecords SET ResultMsg='{0}',Status=1,SendTimes=SendTimes+1,UpdateTime='{1}' WHERE Id={2}", sucMessage, DateTime.Now.ToString(), id);
            }
            try
            {
                SqlHelper.ExecuteNonQuery(SqlHelper.connString, CommandType.Text, sqlStr);
            }
            catch (Exception ex)
            {
                CommLog.WriteExceptionLog("提交学习记录成功修改状态时出现异常：" + ex.Message + ";sql=" + sqlStr, "Learn");
            }
        }

        /// <summary>
        /// 提交学习记录失败后修改操作表状态
        /// </summary>
        /// <param name="id">学习记录操作表id</param>
        /// <param name="failMessage">提交失败文字说明</param>
        /// <param name="sendTimes">记录累积处理次数</param>
        internal void UpdateLearnSendTimes(int id, string failMessage, int sendTimes)
        {
            string sqlStr = string.Empty;
            if (sendTimes < 2)
            {
                if (sendTimes == 0)
                {
                    sqlStr = string.Format(@"UPDATE Reform_LearnRecords SET ResultMsg='{0}',SendTimes=1,SendTime='{1}' WHERE Id={2}", failMessage, DateTime.Now.ToString(), id);
                }
                else
                {
                    sqlStr = string.Format(@"UPDATE Reform_LearnRecords SET ResultMsg='{0}',SendTimes=SendTimes+1,UpdateTime='{1}' WHERE Id={2}", failMessage, DateTime.Now.ToString(), id);
                }
            }
            else
            {
                sqlStr = string.Format(@"UPDATE Reform_LearnRecords SET ResultMsg='{0}',Status=2,SendTimes=SendTimes+1,UpdateTime='{1}' WHERE Id={2}", failMessage, DateTime.Now.ToString(), id);
            }
            try
            {
                SqlHelper.ExecuteNonQuery(SqlHelper.connString, CommandType.Text, sqlStr);
            }
            catch (Exception ex)
            {
                CommLog.WriteExceptionLog("提交学习记录失败后修改操作表状态时出现异常：" + ex.Message + ";sql=" + sqlStr, "Learn");
            }
        }
        #endregion

        #region 考试记录操作表相关
        /// <summary>
        /// 获取考试记录操作表中所有的项目编号
        /// </summary>
        /// <returns></returns>
        internal DataTable GetExamProjectCode()
        {
            DataTable dt = null;
            string sqlStr = "SELECT ProjectCode FROM Reform_ExamRecords GROUP BY ProjectCode ";
            try
            {
                dt = SqlHelper.ExecuteTable(SqlHelper.connString, sqlStr);
            }
            catch (Exception ex)
            {
                CommLog.WriteExceptionLog("获取考试记录操作表中所有的项目编号时出现异常：" + ex.Message + ";sql=" + sqlStr, "Exam");
            }
            return dt;
        }

        /// <summary>
        /// 获取考试记录操作表中未处理的记录
        /// </summary>
        /// <param name="projectCode">项目编号</param>
        /// <returns></returns>
        internal DataTable GetExamRecords(string projectCode)
        {
            DataTable dt = null;
            string sqlStr = string.Format(@"SELECT TOP 100 Id, ProjectCode, StudentNo, Score, ExamTime, ExamName, Status, SendTimes,ClassId FROM Reform_ExamRecords WHERE ProjectCode='{0}' AND Status=0", projectCode);
            try
            {
                dt = SqlHelper.ExecuteTable(SqlHelper.connString, sqlStr);
            }
            catch (Exception ex)
            {
                CommLog.WriteExceptionLog("获取考试记录操作表中未处理的记录时出现异常：" + ex.Message + ";sql=" + sqlStr, "Exam");
            }
            return dt;
        }

        /// <summary>
        /// 提交考试记录成功后更新操作表状态
        /// </summary>
        /// <param name="id">考试操作表id</param>
        /// <param name="sucMessage">提交成功文字说明</param>
        /// <param name="sendTimes">考试记录累积处理次数</param>
        internal void UpdateExamRecordStatus(int id, string sucMessage, int sendTimes)
        {
            string sqlStr = string.Empty;
            if (sendTimes == 0)
            {
                sqlStr = string.Format(@"UPDATE Reform_ExamRecords SET ResultMsg='{0}',Status=1,SendTimes=1,SendTime='{1}' WHERE Id={2}", sucMessage, DateTime.Now.ToString(), id);
            }
            else
            {
                sqlStr = string.Format(@"UPDATE Reform_ExamRecords SET ResultMsg='{0}',Status=1,SendTimes=SendTimes+1,UpdateTime='{1}' WHERE Id={2}", sucMessage, DateTime.Now.ToString(), id);
            }
            try
            {
                SqlHelper.ExecuteNonQuery(SqlHelper.connString, CommandType.Text, sqlStr);
            }
            catch (Exception ex)
            {
                CommLog.WriteExceptionLog("提交考试记录成功后更新操作表状态时出现异常：" + ex.Message + ";sql=" + sqlStr, "Exam");
            }
        }

        /// <summary>
        ///提交考试记录失败更新操作表状态 
        /// </summary>
        /// <param name="id">考试操作表id</param>
        /// <param name="failMessage">提交失败文字说明</param>
        /// <param name="sendTimes">考试记录累积处理次数</param>
        internal void UpdateExamSendTimes(int id, string failMessage, int sendTimes)
        {
            string sqlStr = string.Empty;
            if (sendTimes < 2)
            {
                if (sendTimes == 0)
                {
                    sqlStr = string.Format(@"UPDATE Reform_ExamRecords SET ResultMsg='{0}',SendTimes=1,SendTime='{1}' WHERE Id={2}", failMessage, DateTime.Now.ToString(), id);
                }
                else
                {
                    sqlStr = string.Format(@"UPDATE Reform_ExamRecords SET ResultMsg='{0}',SendTimes=SendTimes+1,UpdateTime='{1}' WHERE Id={2}", failMessage, DateTime.Now.ToString(), id);
                }
            }
            else
            {
                sqlStr = string.Format(@"UPDATE Reform_ExamRecords SET ResultMsg='{0}',Status=2,SendTimes=SendTimes+1,UpdateTime='{1}' WHERE Id={2}", failMessage, DateTime.Now.ToString(), id);
            }
            try
            {
                SqlHelper.ExecuteNonQuery(SqlHelper.connString, CommandType.Text, sqlStr);
            }
            catch (Exception ex)
            {
                CommLog.WriteExceptionLog("提交考试记录失败更新操作表状态时出现异常：" + ex.Message + ";sql=" + sqlStr, "Exam");
            }
        }
        #endregion


        internal string GetAesKeyByUserId(int userId)
        {

            string result = string.Empty;
            if (userId > 0)
            {
                try
                {
                    string sqlStr = string.Format(@"select top 1 AESKey,AESIv from Sys_Tenant where TenantId in(select top 1 TenantId from Sys_Users where UserId={0} AND IsDelete=0 ORDER BY UserId DESC) ", userId);
                    DataTable dt = SqlHelper.ExecuteTable(SqlHelper.connString, sqlStr);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        result = dt.Rows[0]["AESKey"].ToString() + "," + dt.Rows[0]["AESIv"].ToString();
                    }
                }
                catch (Exception)
                {
                    result = string.Empty;
                }
            }
            return result;
        }


        internal string GetAesKeyByStudentNo(string studentNo)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(studentNo))
            {
                try
                {
                    string sqlStr = string.Format(@"select top 1 AESKey,AESIv from Sys_Tenant where TenantId in(select top 1 TenantId from Sys_Users where StudentNo='{0}' AND IsDelete=0 ORDER BY UserId DESC)", studentNo);
                    DataTable dt = SqlHelper.ExecuteTable(SqlHelper.connString, sqlStr);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        result = dt.Rows[0]["AESKey"].ToString() + "," + dt.Rows[0]["AESIv"].ToString();
                    }

                }
                catch (Exception)
                {
                    result = string.Empty;
                }
            }
            return result;
        }

        internal string GetAesKeyByProjectCode(string projectCode)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(projectCode))
            {
                try
                {
                    string sqlStr = string.Format(@"select top 1 AESKey,AESIv from Sys_Tenant where TenantId in(select top 1 TenantId from Res_Course where ProjectCode='{0}' AND IsDelete=0 )", projectCode);
                    DataTable dt = SqlHelper.ExecuteTable(SqlHelper.connString, sqlStr);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        result = dt.Rows[0]["AESKey"].ToString() + "," + dt.Rows[0]["AESIv"].ToString();
                    }

                }
                catch (Exception)
                {
                    result = string.Empty;
                }
            }
            return result;
        }
    }
}
