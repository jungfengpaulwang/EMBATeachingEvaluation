using System;
using FISCA.UDT;

namespace TeachingEvaluation.UDT
{
    /// <summary>
    /// 課程評鑑統計
    /// </summary>
    [FISCA.UDT.TableName("ischool.emba.teaching_evaluation.subject_statistics")]
    public class SubjectStatistics : ActiveRecord
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
        /// 課程識別碼
        /// </summary>
        [Field(Field = "subject_code", Indexed = false, Caption = "課程識別碼")]
        public string SubjectCode { get; set; }

        /// <summary>
        /// 課號 (6碼系所代碼+4碼課號)
        /// </summary>
        [Field(Field = "new_subject_code", Indexed = false, Caption = "課號")]
        public string NewSubjectCode { get; set; }

        /// <summary>
        /// 評鑑統計值
        /// <Statistics 修課人數='53' 評鑑人數='45'>
        ///     <StatisticsGroup 統計群組='平均評鑑值' 評鑑值='4.56'>
        ///     <StatisticsGroup 統計群組='關鍵評鑑值' 評鑑值='4.33'>
        ///     <Question 題號="1" 題目="大家好，我是題目" 評鑑值='4.58'>
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
        public SubjectStatistics Clone()
        {
            return this.MemberwiseClone() as SubjectStatistics;
        }
    }
}
