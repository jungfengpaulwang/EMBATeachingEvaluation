using System;
using FISCA.UDT;

namespace TeachingEvaluation.UDT
{
    /// <summary>
    /// 問卷達標百分比
    /// </summary>
    [FISCA.UDT.TableName("ischool.emba.teaching_evaluation.achieving_rate")]
    public class AchievingRate : ActiveRecord
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
        /// 問卷達標百分比：(填答問卷數/總問卷數)*100%，小數點後無條件進位。
        /// </summary>
        [Field(Field = "rate", Indexed = false, Caption = "問卷達標百分比")]
        public decimal Rate { get; set; }

        /// <summary>
        /// 最後修改時間
        /// </summary>
        [Field(Field = "time_stamp", Indexed = false, Caption = "最後修改時間")]
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// 最後修改人員
        /// </summary>
        [Field(Field = "update_account", Indexed = false, Caption = "最後修改人員")]
        public string UpdateAccount { get; set; }

        /// <summary>
        /// 淺層複製物件
        /// </summary>
        /// <returns></returns>
        public AchievingRate Clone()
        {
            return this.MemberwiseClone() as AchievingRate;
        }
    }
}
