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
        ///      <Statistics CSAttendCount="53" FeedBackCount="31" TeacherName="李存修" CourseName="財務管理 03" SubjectName="財務管理" SubjectCode="740 M3050" NewSubjectCode="EMBA7006" SchoolYear="102" Semester="1" ClassName="03" CourseID="614" SubjectID="16559" TeacherID="334" SurveyDate="2013/12/5~2013/12/11" SurveyID="100260">    
        ///      <Question ID="100261" No="1" Content="本門課我的出席狀況是" Type="單選題" IsSelfAssessment="是" IsCase="否" Score="1.23">
        ///          <Option ID="162628" No="1" Content="從不缺課" AnswerCount="24" />
        ///         <Option ID="162629" No="2" Content="缺課1~2次" AnswerCount="7" />
        ///          <Option ID="162630" No="3" Content="缺課3~4次" AnswerCount="0" />
        ///          <Option ID="162631" No="4" Content="缺課5次以上" AnswerCount="0" />
        ///      </Question>
        ///      <Question ID="122113" No="2" Content="教師對本課程是否安排課後學習活動(可複選):" Type="複選題" IsSelfAssessment="是" IsCase="否" Score="2.45">
        ///        <Option ID="122784" No="1" Content="閱讀資料" AnswerCount="13" />
        ///        <Option ID="122785" No="2" Content="繳交心得報告" AnswerCount="16" />
        ///        <Option ID="122786" No="3" Content="寫報告" AnswerCount="25" />
        ///        <Option ID="122787" No="4" Content="準備口頭報告" AnswerCount="8" />
        ///        <Option ID="122788" No="5" Content="其他" AnswerCount="0" />
        ///        <Option ID="122789" No="6" Content="無特殊安排" AnswerCount="0" />
        ///      </Question>
        ///      <Question ID="104105" No="14" Content="以下個案能有效地提升學習效果:" Type="單選題" IsSelfAssessment="否" IsCase="是">
        ///           <Case ID="119256" Content="Continental Carriers" Score="4.19">
        ///               <Option ID="122532" No="1" Content="非常不同意" AnswerCount="0" />
        ///               <Option ID="122533" No="2" Content="不同意" AnswerCount="0" />
        ///               <Option ID="122534" No="3" Content="普通" AnswerCount="3" />
        ///               <Option ID="122535" No="4" Content="同意" AnswerCount="19" />
        ///               <Option ID="122536" No="5" Content="非常同意" AnswerCount="9" />
        ///           </Case>
        ///      </Question>
        ///      <Question ID="104137" No="1" Content="本課程對我而言，最大的收穫是？" Type="問答題" IsSelfAssessment="否" IsCase="否">
        ///          <Answers>
        ///               <Answer>更了解財經知識</Answer>
        ///               <Answer></Answer>
        ///          <Answers>
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
