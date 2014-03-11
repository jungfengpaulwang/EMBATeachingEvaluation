using System;
using FISCA.UDT;

namespace TeachingEvaluation.UDT
{
    /// <summary>
    /// 教學評鑑問卷樣版
    /// </summary>
    [FISCA.UDT.TableName("ischool.emba.teaching_evaluation.assigned_survey")]
    public class AssignedSurvey : ActiveRecord
    {
        /// <summary>
        /// 問卷樣版系統編號
        /// </summary>
        [Field(Field = "ref_survey_id", Indexed = true, Caption = "問卷樣版系統編號")]
        public int SurveyID { get; set; }

        /// <summary>
        /// 課程系統編號
        /// </summary>
        [Field(Field = "ref_course_id", Indexed = true, Caption = "課程系統編號")]
        public int CourseID { get; set; }

        /// <summary>
        /// 教師系統編號
        /// </summary>
        [Field(Field = "ref_teacher_id", Indexed = true, Caption = "教師系統編號")]
        public int TeacherID { get; set; }

        /// <summary>
        /// 問卷填寫開始時間
        /// </summary>
        [Field(Field = "opening_time", Indexed = false, Caption = "問卷填寫開始時間")]
        public DateTime OpeningTime { get; set; }

        /// <summary>
        /// 問卷填寫結束時間
        /// </summary>
        [Field(Field = "end_time", Indexed = false, Caption = "問卷填寫結束時間")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 評鑑說明
        /// </summary>
        [Field(Field = "description", Indexed = false, Caption = "評鑑說明")]
        public string Description { get; set; }

        /// <summary>
        /// Email 提醒通知時間
        /// </summary>
        [Field(Field = "email_time", Indexed = false, Caption = "Email 提醒通知時間")]
        public DateTime? EmailTime { get; set; }

        /// <summary>
        /// Email 再次提醒通知時間
        /// </summary>
        [Field(Field = "second_email_time", Indexed = false, Caption = "Email 再次提醒通知時間")]
        public DateTime? SecondEmailTime { get; set; }

        /// <summary>
        /// 淺層複製物件
        /// </summary>
        /// <returns></returns>
        public AssignedSurvey Clone()
        {
            return this.MemberwiseClone() as AssignedSurvey;
        }
    }
}
