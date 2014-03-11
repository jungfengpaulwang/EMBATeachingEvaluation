using System;
using FISCA.UDT;

namespace TeachingEvaluation.UDT
{
    /// <summary>
    /// 層次清單
    /// </summary>
    [FISCA.UDT.TableName("ischool.emba.teaching_evaluation.hierarchy")]
    public class Hierarchy : ActiveRecord
    {
        /// <summary>
        /// 層次標題
        /// </summary>
        [Field(Field = "title", Indexed = false, Caption = "標題")]
        public string Title { get; set; }

        /// <summary>
        /// 顯示順序
        /// </summary>
        [Field(Field = "display_order", Indexed = false, Caption = "題號")]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 淺層複製物件
        /// </summary>
        /// <returns></returns>
        public Hierarchy Clone()
        {
            return this.MemberwiseClone() as Hierarchy;
        }
    }
}
