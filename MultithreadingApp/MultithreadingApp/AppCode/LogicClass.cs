using MultithreadingApp.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultithreadingApp.AppCode
{
    /// <summary>
    /// 逻辑类
    /// </summary>
    public static class LogicClass
    {

        /// <summary>
        /// 处理添加/修改学员信息操作表
        /// </summary>
        internal static void Deal0()
        {
            BasicClass basic = new BasicClass();
            while (true)
            {

                //从添加/修改学员操作表中获取未处理的记录
                DataTable dt_student = basic.GetStudentInfo();
                if (dt_student != null && dt_student.Rows.Count > 0)
                {
                    //循环遍历
                    for (int i = 0; i < dt_student.Rows.Count; i++)
                    {
                        int id = Convert.ToInt32(dt_student.Rows[i]["Id"]);
                        string studentNo = dt_student.Rows[i].IsNull("StudentNo") ? string.Empty : dt_student.Rows[i]["StudentNo"].ToString();
                        string realName = dt_student.Rows[i].IsNull("RealName") ? string.Empty : dt_student.Rows[i]["RealName"].ToString();
                        int sex = dt_student.Rows[i].IsNull("Sex") ? 0 : Convert.ToInt32(dt_student.Rows[i]["Sex"]);
                        string phone = dt_student.Rows[i].IsNull("Phone") ? string.Empty : dt_student.Rows[i]["Phone"].ToString();
                        string email = dt_student.Rows[i].IsNull("Email") ? string.Empty : dt_student.Rows[i]["Email"].ToString();
                        string nation = dt_student.Rows[i].IsNull("Nation") ? string.Empty : dt_student.Rows[i]["Nation"].ToString();
                        int certificateType = dt_student.Rows[i].IsNull("CertificateType") ? -1 : Convert.ToInt32(dt_student.Rows[i]["CertificateType"]);
                        string certificateCode = dt_student.Rows[i].IsNull("CertificateCode") ? string.Empty : dt_student.Rows[i]["CertificateCode"].ToString();
                        string url = dt_student.Rows[i].IsNull("Url") ? string.Empty : dt_student.Rows[i]["Url"].ToString();
                        int type = dt_student.Rows[i].IsNull("Type") ? -1 : Convert.ToInt32(dt_student.Rows[i]["Type"]);
                        int sendTimes = dt_student.Rows[i].IsNull("SendTimes") ? 0 : Convert.ToInt32(dt_student.Rows[i]["SendTimes"]);
                        string projectCode = dt_student.Rows[i].IsNull("ProjectCode") ? string.Empty : dt_student.Rows[i]["ProjectCode"].ToString();
                        int userId = dt_student.Rows[i].IsNull("UserId") ? 0 : Convert.ToInt32(dt_student.Rows[i]["UserId"]);

                        //准备接口所需要的参数
                        string cards = string.Empty;
                        if (certificateType >= 0 && !string.IsNullOrEmpty(certificateCode))
                        {
                            cards = "{\"CertificateType\":\"" + certificateType.ToString() + "\",\"CertificateNum\":\"" + certificateCode + "\"}";
                        }

                        string studentJsonStr = string.Empty;
                        if (type == 0)
                        {
                            //添加
                            studentJsonStr = "{\"RealName\":\"" + realName + "\",\"Sex\":\"" + sex.ToString() + "\",\"Phone\":\"" + phone + "\",\"Email\":\"" + email + "\",\"Nation\":\"" + nation + "\",\"Certificates\":[" + cards + "]}";
                        }
                        else if (type == 1)
                        {
                            //修改
                            studentJsonStr = "{\"StudentNo\":\"" + studentNo + "\",\"RealName\":\"" + realName + "\",\"Sex\":\"" + sex.ToString() + "\",\"Phone\":\"" + phone + "\",\"Email\":\"" + email + "\",\"Nation\":\"" + nation + "\",\"Certificates\":[" + cards + "]}";
                        }
                        string aeskey = string.Empty;
                        string aesiv = string.Empty;
                        string aesString = basic.GetAesKeyByUserId(userId);
                        if (aesString.Contains(","))
                        {
                            aeskey = aesString.Split(',')[0];
                            aesiv = aesString.Split(',')[1];
                        }

                        string encryption = string.Empty;
                        if (!string.IsNullOrEmpty(aeskey) && !string.IsNullOrEmpty(aesiv))
                        {
                            encryption = basic.Encrypt(studentJsonStr, aeskey, aesiv);
                        }
                        string result = basic.PostData(url, projectCode, encryption);
                        if (!string.IsNullOrEmpty(result))
                        {
                            if (type == 0)
                            {
                                var addResult = basic.JsonStr2Obj<AddResult>(result);
                                if (addResult != null)
                                {
                                    basic.UpdateStudentStatus(id, addResult.ReturnValue.Trim(), addResult.Code, addResult.Message);
                                    if (addResult.Code.Trim() == "0")
                                    {
                                        //添加学员成功                                       
                                        //更新学员表studentno
                                        basic.UpdateStudentNo(userId, addResult.ReturnValue.Trim());
                                        //更新一下该学员支付表中的学号
                                        basic.UpdatePayStudentNo(userId, addResult.ReturnValue.Trim(), projectCode);
                                        //向学员报名操作表中插入一条未处理的数据
                                        basic.InsertIntoReformSignUp(projectCode, addResult.ReturnValue.Trim(), ConfigurationManager.AppSettings["ReformInterface"].ToString() + "/SignUp");
                                    }


                                }
                                else
                                {
                                    //添加失败
                                    //如果累计提交次数达到3次，将操作表中的状态修改为处理失败；如果未达到3次将处理次数加1
                                    basic.UpdateStudentSendTimes(id, string.Empty, string.Empty, sendTimes);

                                }
                            }
                            else if (type == 1)
                            {
                                var modifyResult = basic.JsonStr2Obj<ModifyResult>(result);
                                if (modifyResult != null)
                                {
                                    //修改成功
                                    basic.UpdateStudentStatus(id, studentNo, modifyResult.Code, modifyResult.Message);
                                }
                                else
                                {
                                    //修改失败
                                    basic.UpdateStudentSendTimes(id, string.Empty, string.Empty, sendTimes);
                                }
                            }

                        }
                        else
                        {
                            basic.UpdateStudentSendTimes(id, string.Empty, string.Empty, sendTimes);
                        }
                    }
                }

                //休眠十分钟
                Thread.Sleep(1000 * 60 * Convert.ToInt32(ConfigurationManager.AppSettings["deal0_sleep"]));
            }

        }

        /// <summary>
        /// 处理学员报名项目操作表
        /// </summary>
        internal static void Deal1()
        {
            BasicClass basic = new BasicClass();
            while (true)
            {
                //从报名项目操作表中取出还没有处理的记录
                DataTable dt_SignUp = basic.GetSignUpInfo();
                if (dt_SignUp != null && dt_SignUp.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_SignUp.Rows.Count; i++)
                    {

                        int id = Convert.ToInt32(dt_SignUp.Rows[i]["Id"]);
                        string studentNo = dt_SignUp.Rows[i].IsNull("StudentNo") ? string.Empty : dt_SignUp.Rows[i]["StudentNo"].ToString();
                        string projectCode = dt_SignUp.Rows[i].IsNull("ProjectCode") ? string.Empty : dt_SignUp.Rows[i]["ProjectCode"].ToString();
                        string url = dt_SignUp.Rows[i].IsNull("Url") ? string.Empty : dt_SignUp.Rows[i]["Url"].ToString();
                        int sendTimes = dt_SignUp.Rows[i].IsNull("SendTimes") ? 0 : Convert.ToInt32(dt_SignUp.Rows[i]["SendTimes"]);
                        int classId = dt_SignUp.Rows[i].IsNull("ClassId") ? 0 : Convert.ToInt32(dt_SignUp.Rows[i]["ClassId"]);
                        //准备接口所需参数
                        long timestamp = basic.GetTimeStamp(DateTime.Now);
                        string signUpJsonStr = "{\"TimeStamp\":\"" + timestamp.ToString() + "\",\"StudentNo\":\"" + studentNo + "\",\"ClassId\":\"" + classId.ToString() + "\"}";

                        string aeskey = string.Empty;
                        string aesiv = string.Empty;
                        string aesString = basic.GetAesKeyByStudentNo(studentNo);
                        if (aesString.Contains(","))
                        {
                            aeskey = aesString.Split(',')[0];
                            aesiv = aesString.Split(',')[1];
                        }
                        string encryption = string.Empty;
                        if (!string.IsNullOrEmpty(aeskey) && !string.IsNullOrEmpty(aesiv))
                        {
                            encryption = basic.Encrypt(signUpJsonStr, aeskey, aesiv);
                        }
                        //调用接口
                        string result = basic.PostData(url, projectCode, encryption);
                        if (!string.IsNullOrEmpty(result))
                        {
                            var signUpResult = basic.JsonStr2Obj<SignUpResult>(result);
                            if (signUpResult != null)
                            {
                                //报名成功
                                //更新操作表记录的状态
                                basic.UpdateSignUpStatus(id, signUpResult.Code, signUpResult.Message, signUpResult.ReturnValue.Trim());
                                //如果审核通过则更新报名表Res_CourseEnter记录的审核状态
                                if (signUpResult.ReturnValue.Trim() == "1")
                                {
                                    basic.UpdateCourseEnterStatus(projectCode, studentNo);
                                }

                            }
                            else
                            {
                                //报名失败
                                //更新操作表记录的sendTimes
                                basic.UpdateSignUpSendTimes(id, sendTimes, string.Empty, string.Empty);
                            }
                        }
                        else
                        {
                            basic.UpdateSignUpSendTimes(id, sendTimes, string.Empty, string.Empty);
                        }
                    }
                }


                //休眠十分钟
                Thread.Sleep(1000 * 60 * Convert.ToInt32(ConfigurationManager.AppSettings["deal1_sleep"]));
            }

        }

        /// <summary>
        /// 处理学员支付操作表
        /// </summary>
        internal static void Deal2()
        {
            BasicClass basic = new BasicClass();
            while (true)
            {
                //获取支付操作表中项目编号 ProjectCode
                DataTable dt_project = basic.GetPayProjectCode();
                if (dt_project != null && dt_project.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_project.Rows.Count; i++)
                    {
                        string projectCode = dt_project.Rows[i].IsNull("ProjectCode") ? string.Empty : dt_project.Rows[i]["ProjectCode"].ToString();
                        if (!string.IsNullOrEmpty(projectCode))
                        {
                            //获取某个项目下的支付记录
                            DataTable dt_PayRecords = basic.GetPayRecords(projectCode);
                            if (dt_PayRecords != null && dt_PayRecords.Rows.Count > 0)
                            {
                                //拼接支付记录Json
                                long timeStamp = basic.GetTimeStamp(DateTime.Now);
                                string payJson = string.Empty;
                                StringBuilder sb = new StringBuilder();
                                for (int j = 0; j < dt_PayRecords.Rows.Count; j++)
                                {
                                    string studentNo = dt_PayRecords.Rows[j].IsNull("StudentNo") ? string.Empty : dt_PayRecords.Rows[j]["StudentNo"].ToString();
                                    string payAmount = dt_PayRecords.Rows[j].IsNull("PayAmount") ? string.Empty : dt_PayRecords.Rows[j]["PayAmount"].ToString();
                                    string payTime = dt_PayRecords.Rows[j].IsNull("PayTime") ? string.Empty : basic.GetTimeStamp(Convert.ToDateTime(dt_PayRecords.Rows[j]["PayTime"])).ToString();
                                    string payDesc = dt_PayRecords.Rows[j].IsNull("PayDesc") ? string.Empty : dt_PayRecords.Rows[j]["PayDesc"].ToString();
                                    int classId= dt_PayRecords.Rows[j].IsNull("ClassId") ? 0 : Convert.ToInt32(dt_PayRecords.Rows[j]["ClassId"]);
                                    if (!string.IsNullOrEmpty(studentNo) && !string.IsNullOrEmpty(payAmount) && !string.IsNullOrEmpty(payTime) && !string.IsNullOrEmpty(payDesc))
                                    {
                                        sb.Append("{\"StudentNo\":\"" + studentNo + "\",\"PayAmount\":\"" + payAmount + "\",\"PayTime\":\"" + payTime + "\",\"PayDesc\":\"" + payDesc + "\",\"ClassId\":\"" + classId.ToString() + "\"},");
                                    }
                                }
                                if (sb.Length > 0)
                                {
                                    payJson = sb.Remove(sb.Length - 1, 1).ToString();
                                }
                                string jsonStr = "{\"TimeStamp\":\"" + timeStamp.ToString() + "\",\"Records\":[" + payJson + "]}";

                                string aeskey = string.Empty;
                                string aesiv = string.Empty;
                                string aesString = basic.GetAesKeyByProjectCode(projectCode);
                                if (aesString.Contains(","))
                                {
                                    aeskey = aesString.Split(',')[0];
                                    aesiv = aesString.Split(',')[1];
                                }
                                string encryption = string.Empty;
                                if (!string.IsNullOrEmpty(aeskey) && !string.IsNullOrEmpty(aesiv))
                                {
                                    encryption = basic.Encrypt(jsonStr, aeskey, aesiv);
                                }

                                //调用接口
                                string result = basic.PostData(ConfigurationManager.AppSettings["ReformInterface"].ToString() + "/PaySubmit", projectCode, encryption);
                                if (!string.IsNullOrEmpty(result))
                                {
                                    var submitRecordResult = basic.JsonStr2Obj<SubmitRecordResult>(result);
                                    if (submitRecordResult != null)
                                    {
                                        if (submitRecordResult.Success.Count > 0)
                                        {
                                            //提交成功的记录
                                            var sucList = submitRecordResult.Success;
                                            int sucNumber = 0;
                                            string sucMessage = string.Empty;
                                            foreach (var item in sucList)
                                            {
                                                sucNumber = item.number;
                                                sucMessage = item.msg;
                                                if (sucNumber >= 0)
                                                {
                                                    basic.UpdatePayRecordStatus(Convert.ToInt32(dt_PayRecords.Rows[sucNumber]["Id"]), sucMessage, Convert.ToInt32(dt_PayRecords.Rows[sucNumber].IsNull("SendTimes") ? 0 : dt_PayRecords.Rows[sucNumber]["SendTimes"]));
                                                }

                                            }
                                        }
                                        if (submitRecordResult.Failed.Count > 0)
                                        {
                                            //提交失败的记录
                                            var failList = submitRecordResult.Failed;
                                            int failNumber = 0;
                                            string failMessage = string.Empty;
                                            foreach (var item in failList)
                                            {
                                                failNumber = item.number;
                                                failMessage = item.msg;
                                                if (failNumber >= 0)
                                                {
                                                    basic.UpdatePayRecordStatus(Convert.ToInt32(dt_PayRecords.Rows[failNumber]["Id"]), failMessage, Convert.ToInt32(dt_PayRecords.Rows[failNumber].IsNull("SendTimes") ? 0 : dt_PayRecords.Rows[failNumber]["SendTimes"]));
                                                }
                                            }
                                        }
                                    }
                                }


                            }

                        }
                    }
                }
                //休眠一小时
                Thread.Sleep(1000 * 60 * Convert.ToInt32(ConfigurationManager.AppSettings["deal2_sleep"]));

            }
        }

        /// <summary>
        ///处理学员学习操作表
        /// </summary>
        internal static void Deal3()
        {
            BasicClass basic = new BasicClass();
            while (true)
            {
                //获取学习操作表中的项目编号
                DataTable dt_project = basic.GetLearnProjectCode();
                if (dt_project != null && dt_project.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_project.Rows.Count; i++)
                    {
                        string projectCode = dt_project.Rows[i].IsNull("ProjectCode") ? string.Empty : dt_project.Rows[i]["ProjectCode"].ToString();
                        if (!string.IsNullOrEmpty(projectCode))
                        {
                            //获取该项目下的学习记录
                            DataTable dt_learn = basic.GetLearnRecords(projectCode);
                            if (dt_learn != null && dt_learn.Rows.Count > 0)
                            {
                                //拼接学习记录Json
                                long timeStamp = basic.GetTimeStamp(DateTime.Now);
                                string learnJson = string.Empty;
                                StringBuilder sb = new StringBuilder();
                                for (int j = 0; j < dt_learn.Rows.Count; j++)
                                {
                                    string studentNo = dt_learn.Rows[j].IsNull("StudentNo") ? string.Empty : dt_learn.Rows[j]["StudentNo"].ToString();
                                    string startTime = dt_learn.Rows[j].IsNull("StartTime") ? string.Empty : basic.GetTimeStamp(Convert.ToDateTime(dt_learn.Rows[j]["StartTime"])).ToString();
                                    string endTime = dt_learn.Rows[j].IsNull("EndTime") ? string.Empty : basic.GetTimeStamp(Convert.ToDateTime(dt_learn.Rows[j]["EndTime"])).ToString();
                                    string learnTime = dt_learn.Rows[j].IsNull("LearnTime") ? string.Empty : dt_learn.Rows[j]["LearnTime"].ToString();
                                    int classId = dt_learn.Rows[j].IsNull("ClassId") ? 0 : Convert.ToInt32(dt_learn.Rows[j]["ClassId"]);
                                    if (!string.IsNullOrEmpty(studentNo) && !string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime) && !string.IsNullOrEmpty(learnTime))
                                    {
                                        sb.Append("{\"StudentNo\":\"" + studentNo + "\",\"StartTime\":\"" + startTime + "\",\"EndTime\":\"" + endTime + "\",\"LearnTime\":\"" + learnTime + "\",\"ClassId\":\"" + classId.ToString() + "\"},");
                                    }
                                }
                                if (sb.Length > 0)
                                {
                                    learnJson = sb.Remove(sb.Length - 1, 1).ToString();
                                }
                                string jsonStr = "{\"TimeStamp\":\"" + timeStamp.ToString() + "\",\"Records\":[" + learnJson + "]}";
                                string aeskey = string.Empty;
                                string aesiv = string.Empty;
                                string aesString = basic.GetAesKeyByProjectCode(projectCode);
                                if (aesString.Contains(","))
                                {
                                    aeskey = aesString.Split(',')[0];
                                    aesiv = aesString.Split(',')[1];
                                }
                                string encryption = string.Empty;
                                if (!string.IsNullOrEmpty(aeskey) && !string.IsNullOrEmpty(aesiv))
                                {
                                    encryption = basic.Encrypt(jsonStr, aeskey, aesiv);
                                }
                                //调用接口
                                string result = basic.PostData(ConfigurationManager.AppSettings["ReformInterface"].ToString() + "/StudySubmit", projectCode, encryption);
                                if (!string.IsNullOrEmpty(result))
                                {

                                    var submitRecordResult = basic.JsonStr2Obj<SubmitRecordResult>(result);
                                    if (submitRecordResult != null)
                                    {
                                        if (submitRecordResult.Success.Count > 0)
                                        {
                                            //提交成功的记录
                                            var sucList = submitRecordResult.Success;
                                            int sucNumber = 0;
                                            string sucMessage = string.Empty;
                                            foreach (var item in sucList)
                                            {
                                                sucNumber = item.number;
                                                sucMessage = item.msg;
                                                if (sucNumber >= 0)
                                                {
                                                    basic.UpdateLearnRecordStatus(Convert.ToInt32(dt_learn.Rows[sucNumber]["Id"]), sucMessage, Convert.ToInt32(dt_learn.Rows[sucNumber].IsNull("SendTimes") ? 0 : dt_learn.Rows[sucNumber]["SendTimes"]));
                                                }

                                            }
                                        }
                                        if (submitRecordResult.Failed.Count >= 0)
                                        {
                                            //提交失败的记录
                                            var failList = submitRecordResult.Failed;
                                            int failNumber = 0;
                                            string failMessage = string.Empty;
                                            foreach (var item in failList)
                                            {
                                                failNumber = item.number;
                                                failMessage = item.msg;
                                                if (failNumber > 0)
                                                {
                                                    basic.UpdateLearnRecordStatus(Convert.ToInt32(dt_learn.Rows[failNumber]["Id"]), failMessage, Convert.ToInt32(dt_learn.Rows[failNumber].IsNull("SendTimes") ? 0 : dt_learn.Rows[failNumber]["SendTimes"]));
                                                }
                                            }
                                        }
                                    }
                                }


                            }
                        }
                    }
                }


                //休眠一小时
                Thread.Sleep(1000 * 60 * Convert.ToInt32(ConfigurationManager.AppSettings["deal3_sleep"]));

            }
        }

        /// <summary>
        /// 处理学员考试操作表
        /// </summary>
        internal static void Deal4()
        {
            BasicClass basic = new BasicClass();
            while (true)
            {
                //获取考试操作表中项目编号 ProjectCode
                DataTable dt_project = basic.GetExamProjectCode();
                if (dt_project != null && dt_project.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_project.Rows.Count; i++)
                    {
                        string projectCode = dt_project.Rows[i].IsNull("ProjectCode") ? string.Empty : dt_project.Rows[i]["ProjectCode"].ToString();
                        if (!string.IsNullOrEmpty(projectCode))
                        {
                            DataTable dt_exam = basic.GetExamRecords(projectCode);
                            if (dt_exam != null && dt_exam.Rows.Count > 0)
                            {
                                //拼接考试记录Json
                                long timeStamp = basic.GetTimeStamp(DateTime.Now);
                                string examJson = string.Empty;
                                StringBuilder sb = new StringBuilder();
                                for (int j = 0; j < dt_exam.Rows.Count; j++)
                                {
                                    string studentNo = dt_exam.Rows[j].IsNull("StudentNo") ? string.Empty : dt_exam.Rows[j]["StudentNo"].ToString();
                                    string score = dt_exam.Rows[j].IsNull("Score") ? string.Empty : dt_exam.Rows[j]["Score"].ToString();
                                    string examTime = dt_exam.Rows[j].IsNull("ExamTime") ? string.Empty : basic.GetTimeStamp(Convert.ToDateTime(dt_exam.Rows[j]["ExamTime"])).ToString();
                                    string examName = dt_exam.Rows[j].IsNull("ExamName") ? string.Empty : dt_exam.Rows[j]["ExamName"].ToString();
                                    int classId = dt_exam.Rows[j].IsNull("ClassId") ? 0 : Convert.ToInt32(dt_exam.Rows[j]["ClassId"]);
                                    if (!string.IsNullOrEmpty(studentNo) && !string.IsNullOrEmpty(score) && !string.IsNullOrEmpty(examTime) && !string.IsNullOrEmpty(examName))
                                    {
                                        sb.Append("{\"StudentNo\":\"" + studentNo + "\",\"Score\":\"" + score + "\",\"ExamTime\":\"" + examTime + "\",\"ExamName\":\"" + examName + "\",\"ClassId\":\"" + classId.ToString() + "\"},");
                                    }
                                }
                                if (sb.Length > 0)
                                {
                                    examJson = sb.Remove(sb.Length - 1, 1).ToString();
                                }
                                string jsonStr = "{\"TimeStamp\":\"" + timeStamp.ToString() + "\",\"Records\":[" + examJson + "]}";
                                string aeskey = string.Empty;
                                string aesiv = string.Empty;
                                string aesString = basic.GetAesKeyByProjectCode(projectCode);
                                if (aesString.Contains(","))
                                {
                                    aeskey = aesString.Split(',')[0];
                                    aesiv = aesString.Split(',')[1];
                                }
                                string encryption = string.Empty;
                                if (!string.IsNullOrEmpty(aeskey) && !string.IsNullOrEmpty(aesiv))
                                {
                                    encryption = basic.Encrypt(jsonStr, aeskey, aesiv);
                                }
                                //调用接口
                                string result = basic.PostData(ConfigurationManager.AppSettings["ReformInterface"].ToString() + "/ExamSubmit", projectCode, encryption);
                                if (!string.IsNullOrEmpty(result))
                                {

                                    var submitRecordResult = basic.JsonStr2Obj<SubmitRecordResult>(result);
                                    if (submitRecordResult != null)
                                    {
                                        if (submitRecordResult.Success.Count > 0)
                                        {
                                            //提交成功的记录
                                            var sucList = submitRecordResult.Success;
                                            int sucNumber = 0;
                                            string sucMessage = string.Empty;
                                            foreach (var item in sucList)
                                            {
                                                sucNumber = item.number;
                                                sucMessage = item.msg;
                                                if (sucNumber >= 0)
                                                {
                                                    basic.UpdateExamRecordStatus(Convert.ToInt32(dt_exam.Rows[sucNumber]["Id"]), sucMessage, Convert.ToInt32(dt_exam.Rows[sucNumber].IsNull("SendTimes") ? 0 : dt_exam.Rows[sucNumber]["SendTimes"]));
                                                }

                                            }
                                        }
                                        if (submitRecordResult.Failed.Count > 0)
                                        {
                                            //提交失败的记录
                                            var failList = submitRecordResult.Failed;
                                            int failNumber = 0;
                                            string failMessage = string.Empty;
                                            foreach (var item in failList)
                                            {
                                                failNumber = item.number;
                                                failMessage = item.msg;
                                                if (failNumber >= 0)
                                                {
                                                    basic.UpdateExamRecordStatus(Convert.ToInt32(dt_exam.Rows[failNumber]["Id"]), failMessage, Convert.ToInt32(dt_exam.Rows[failNumber].IsNull("SendTimes") ? 0 : dt_exam.Rows[failNumber]["SendTimes"]));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }

                //休眠一小时          
                Thread.Sleep(1000 * 60 * Convert.ToInt32(ConfigurationManager.AppSettings["deal4_sleep"]));
            }
        }
    }
}
