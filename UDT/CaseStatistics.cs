using System;
using FISCA.UDT;

namespace TeachingEvaluation.UDT
{
    /// <summary>
    /// 個案評鑑統計
    /// </summary>
    [FISCA.UDT.TableName("ischool.emba.teaching_evaluation.case_statistics")]
    public class CaseStatistics : ActiveRecord
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
        /// 個案系統編號
        /// </summary>
        [Field(Field = "ref_case_id", Indexed = true, Caption = "個案系統編號")]
        public int CaseID { get; set; }

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
        public CaseStatistics Clone()
        {
            return this.MemberwiseClone() as CaseStatistics;
        }
    }
}
