using System;
using FISCA.UDT;

namespace TeachingEvaluation.UDT
{
    /// <summary>
    /// 教學評鑑做答
    /// </summary>
    [FISCA.UDT.TableName("ischool.emba.teaching_evaluation.reply")]
    public class Reply : ActiveRecord
    {
        /// <summary>
        /// 學生系統編號
        /// </summary>
        [Field(Field = "ref_student_id", Indexed = true, Caption = "學生系統編號")]
        public int StudentID { get; set; }

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
        /// 評鑑做答狀態：0-->暫存、1-->送出
        /// </summary>
        [Field(Field = "status", Indexed = true, Caption = "評鑑做答狀態")]
        public int Status { get; set; }

        /// <summary>
        /// 評鑑做答，格式如下：
        /// <Answers>
        ///     <Question QuestionID="">
        ///         <Answer CaseID="" Score="3">尚可</Answer>
        ///         <Answer CaseID="123" Score="5">很滿意</Answer>
        ///     </Question>
        /// </Answers>
        /// </summary>
        [Field(Field = "answer", Indexed = false, Caption = "評鑑做答")]
        public string Answer { get; set; }
        
        ///// <summary>
        ///// Log，格式如下：
        ///// <Log RespondentsID="" RespondentsType="STUDENT/TEACHER" FillTime="2012/12/22 15:34:31" HostName="EePC" HostAddress="140.13.21.32" />
        ///// </summary>
        //[Field(Field = "log", Indexed = false, Caption = "log")]
        //public string Log { get; set; }

        /// <summary>
        /// 淺層複製物件
        /// </summary>
        /// <returns></returns>
        public Reply Clone()
        {
            return this.MemberwiseClone() as Reply;
        }
    }
}
