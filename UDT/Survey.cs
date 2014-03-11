using System;
using FISCA.UDT;

namespace TeachingEvaluation.UDT
{
    /// <summary>
    /// 教學評鑑問卷樣版
    /// </summary>
    [FISCA.UDT.TableName("ischool.emba.teaching_evaluation.survey")]
    public class Survey : ActiveRecord
    {
        /// <summary>
        /// 名稱
        /// </summary>
        [Field(Field = "name", Indexed = true, Caption = "名稱")]
        public string Name { get; set; }

        /// <summary>
        /// 類別
        /// </summary>
        [Field(Field = "category", Indexed = false, Caption = "類別")]
        public string Category { get; set; }

        /// <summary>
        /// 說明
        /// </summary>
        [Field(Field = "description", Indexed = false, Caption = "說明")]
        public string Description { get; set; }

        /// <summary>
        /// 淺層複製物件
        /// </summary>
        /// <returns></returns>
        public Survey Clone()
        {
            return this.MemberwiseClone() as Survey;
        }
    }
}
