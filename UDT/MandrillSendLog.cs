using System;
using FISCA.UDT;

namespace TeachingEvaluation.UDT
{
    /// <summary>
    /// Mandrill Log
    /// </summary>
    [FISCA.UDT.TableName("ischool.emba.mandrill.send.log")]
    public class MandrillSendLog : ActiveRecord
    {
        /// <summary>
        /// 學生系統編號
        /// </summary>
        [Field(Field = "ref_student_id", Indexed = true, Caption = "學生系統編號")]
        public int? StudentID { get; set; }

        /// <summary>
        /// 學號
        /// </summary>
        [Field(Field = "student_number", Indexed = false, Caption = "學號")]
        public string StudentNumber { get; set; }

        /// <summary>
        /// 學生姓名
        /// </summary>
        [Field(Field = "student_name", Indexed = false, Caption = "學生姓名")]
        public string StudentName { get; set; }

        /// <summary>
        /// 課程系統編號
        /// </summary>
        [Field(Field = "ref_subject_id", Indexed = true, Caption = "課程系統編號")]
        public int SubjectID { get; set; }

        /// <summary>
        /// 課程名稱
        /// </summary>
        [Field(Field = "subject_name", Indexed = false, Caption = "課程名稱")]
        public string SubjectName { get; set; }

        /// <summary>
        /// 開課系統編號
        /// </summary>
        [Field(Field = "ref_course_id", Indexed = true, Caption = "開課系統編號")]
        public int CourseID { get; set; }

        /// <summary>
        /// 開課名稱
        /// </summary>
        [Field(Field = "course_name", Indexed = false, Caption = "開課名稱")]
        public string CourseName { get; set; }

        /// <summary>
        /// 授課教師系統編號
        /// </summary>
        [Field(Field = "ref_teacher_id", Indexed = true, Caption = "授課教師系統編號")]
        public int TeacherID { get; set; }

        /// <summary>
        /// 教師姓名
        /// </summary>
        [Field(Field = "teacher_name", Indexed = false, Caption = "教師姓名")]
        public string TeacherName { get; set; }

        /// <summary>
        /// 學年度
        /// </summary>
        [Field(Field = "school_year", Indexed = false, Caption = "學年度")]
        public string SchoolYear { get; set; }

        /// <summary>
        /// 學期
        /// </summary>
        [Field(Field = "semester", Indexed = false, Caption = "學期")]
        public string Semester { get; set; }

        /// <summary>
        /// 評鑑開始時間
        /// </summary>
        [Field(Field = "survey_begin_time", Indexed = false, Caption = "評鑑開始時間")]
        public string SurveyBeginTime { get; set; }

        /// <summary>
        /// 評鑑結束時間
        /// </summary>
        [Field(Field = "survey_end_time", Indexed = false, Caption = "評鑑結束時間")]
        public string SurveyEndTime { get; set; }

        /// <summary>
        /// 發信人系統帳號
        /// </summary>
        [Field(Field = "current_user_account", Indexed = false, Caption = "發信人系統帳號")]
        public string CurrentUserAccount { get; set; }

        /// <summary>
        /// 寄件人電子郵件
        /// </summary>
        [Field(Field = "sender_email_address", Indexed = false, Caption = "寄件人電子郵件")]
        public string SenderEmailAddress { get; set; }

        /// <summary>
        /// 寄件人名稱
        /// </summary>
        [Field(Field = "sender_name", Indexed = false, Caption = "寄件人名稱")]
        public string SenderName { get; set; }

        /// <summary>
        /// 收件人電子郵件
        /// </summary>
        [Field(Field = "recipient_email_address", Indexed = false, Caption = "收件人電子郵件")]
        public string RecipientEmailAddress { get; set; }

        /// <summary>
        /// 電子郵件副本
        /// </summary>
        [Field(Field = "cc_email_address", Indexed = false, Caption = "電子郵件副本")]
        public string CCEmailAddress { get; set; }

        /// <summary>
        /// 是否為副本
        /// </summary>
        [Field(Field = "is_cc", Indexed = false, Caption = "是否為副本")]
        public bool IsCC { get; set; }												

        /// <summary>
        /// 信件類別：發送 Email 提醒通知、發送 Email 再次提醒通知
        /// </summary>
        [Field(Field = "email_category", Indexed = false, Caption = "信件類別")]
        public string EmailCategory { get; set; }

        /// <summary>
        /// 寄信時間
        /// </summary>
        [Field(Field = "time_stamp", Indexed = false, Caption = "寄信時間")]
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// 作業代碼
        /// </summary>
        [Field(Field = "guid", Indexed = false, Caption = "作業代碼")]
        public string GUID { get; set; }

        /// <summary>
        /// 使用設備：desktop、web、mobile
        /// </summary>
        [Field(Field = "device", Indexed = false, Caption = "使用設備")]
        public string Device { get; set; }

        /// <summary>
        /// 失敗狀態
        /// </summary>
        [Field(Field = "error_status", Indexed = false, Caption = "失敗狀態")]
        public string ErrorStatus { get; set; }

        /// <summary>
        /// 失敗代碼
        /// </summary>
        [Field(Field = "error_code", Indexed = false, Caption = "失敗代碼")]
        public int? ErrorCode { get; set; }

        /// <summary>
        /// 失敗名稱
        /// </summary>
        [Field(Field = "error_name", Indexed = false, Caption = "失敗名稱")]
        public string ErrorName { get; set; }

        /// <summary>
        /// 失敗訊息
        /// </summary>
        [Field(Field = "error_message", Indexed = false, Caption = "失敗訊息")]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 失敗完整訊息
        /// </summary>
        [Field(Field = "source", Indexed = false, Caption = "失敗完整訊息")]
        public string Source { get; set; }

        /// <summary>
        /// 成功狀態
        /// </summary>
        [Field(Field = "result_status", Indexed = false, Caption = "成功狀態")]
        public string ResultStatus { get; set; }

        /// <summary>
        /// 成功代碼
        /// </summary>
        [Field(Field = "result_id", Indexed = false, Caption = "成功代碼")]
        public string ResultID { get; set; }

        /// <summary>
        /// 成功寄出之電子郵件
        /// </summary>
        [Field(Field = "result_email", Indexed = false, Caption = "成功寄出之電子郵件")]
        public string ResultEmail { get; set; }

        /// <summary>
        /// 退信原因
        /// </summary>
        [Field(Field = "reject_reason", Indexed = false, Caption = "退信原因")]
        public string RejectReason { get; set; }

        /// <summary>
        /// 淺層複製物件
        /// </summary>
        /// <returns></returns>
        public MandrillSendLog Clone()
        {
            return this.MemberwiseClone() as MandrillSendLog;
        }
    }
}
