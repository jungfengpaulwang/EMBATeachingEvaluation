using System;
using FISCA.UDT;

namespace TeachingEvaluation.UDT
{
    /// <summary>
    /// 教師評鑑統計
    /// </summary>
    [FISCA.UDT.TableName("ischool.emba.teaching_evaluation.teacher_statistics")]
    public class TeacherStatistics : ActiveRecord
    {
        /// <summary>
        /// 學年度
        /// </summary>
        [Field(Field = "school_year", Indexed = true, Caption = "學年度")]
        public int SchoolYear { get; set; }

        /// <summary>
        /// 學期
        /// </summary>
        [Field(Field = "semester", Indexed = true, Caption = "學期")]
        public int Semester { get; set; }

        /// <summary>
        /// 開課系統編號
        /// </summary>
        [Field(Field = "ref_course_id", Indexed = true, Caption = "開課系統編號")]
        public int CourseID { get; set; }

        /// <summary>
        /// 教師系統編號
        /// </summary>
        [Field(Field = "ref_teacher_id", Indexed = true, Caption = "教師系統編號")]
        public int TeacherID { get; set; }

        /// <summary>
        /// 評鑑統計值
        /// 自評題的答案選項要放在題目後
        /// 個案題要列舉個案名稱
        /// <Statistics CSAttendCount='53' FeedBackCount='45' TeacherName='' CourseName='' SubjectName='' SubjectCode='' NewSubjectCode='' SchoolYear='' Semester=''>
        ///     <Question No='1~2' Content='本門課我的出席狀況是' Type='單選題' IsSelfAssessment='是' IsCase='否' Score='4.58'>
        ///         <Option No='1' Content='從不缺課' AnswerCount='6' />
        ///         <Option No='2' Content='缺課1~2次' AnswerCount='12' />
        ///         <Option No='3' Content='缺課3~4次' AnswerCount='5' />
        ///         <Option No='4' Content='缺課5次以上' AnswerCount='8' />
        ///     </Question>
        ///     <Question No='1~19' Content='本課程的內容和學習目標十分明確。' Type='單選題' IsSelfAssessment='否' IsCase='否' Score='4.58'>
        ///         <Option No='1' Content='非常不同意' AnswerCount='6' />
        ///         <Option No='2' Content='不同意' AnswerCount='12' />
        ///         <Option No='3' Content='普通' AnswerCount='5' />
        ///         <Option No='4' Content='同意' AnswerCount='8' />
        ///         <Option No='5' Content='非常同意' AnswerCount='16' />
        ///     </Question>
        ///     <Question No='20' 題目='以下個案能有效地提升學習效果' Type='單選題' IsSelfAssessment='否' IsCase='是'>
        ///         <Case ID="123" Content='我是個案1號' Score='4.58'>
        ///             <Option No='1' Content='非常不同意' AnswerCount='6' />
        ///             <Option No='2' Content='不同意' AnswerCount='12' />
        ///             <Option No='3' Content='普通' AnswerCount='5' />
        ///             <Option No='4' Content='同意' AnswerCount='8' />
        ///             <Option No='5' Content='非常同意' AnswerCount='16' />
        ///         </Case>
        ///         <Case ID="456" Content='我是個案1號' Score='4.58'>
        ///             <Option No='1' Content='非常不同意' AnswerCount='6' />
        ///             <Option No='2' Content='不同意' AnswerCount='12' />
        ///             <Option No='3' Content='普通' AnswerCount='5' />
        ///             <Option No='4' Content='同意' AnswerCount='8' />
        ///             <Option No='5' Content='非常同意' AnswerCount='16' />
        ///         </Case>
        ///     </Question>
        ///     <Question No='21~23' Content='本課程給您最大的收穫是什麼？' Type='問答題' IsSelfAssessment='否' IsCase='否'>
        ///         <Answers>
        ///             <Answer CaseID="" Score="增廣見聞。"/>
        ///         </Answers>
        ///     </Question>
        ///     <StatisticsGroup Content='第1～19題平均評鑑值' Score='4.56' />
        ///     <StatisticsGroup Content='第14、19題平均評鑑值' Score='4.33' />
        /// </Statistics>
        /// </summary>
        [Field(Field = "statistics_list", Indexed = false, Caption = "評鑑統計值")]
        public string StatisticsList { get; set; }

        /// <summary>
        /// 記錄時間
        /// </summary>
        [Field(Field = "time_stamp", Indexed = false, Caption = "記錄時間")]
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// 淺層複製物件
        /// </summary>
        /// <returns></returns>
        public TeacherStatistics Clone()
        {
            return this.MemberwiseClone() as TeacherStatistics;
        }
    }
}
