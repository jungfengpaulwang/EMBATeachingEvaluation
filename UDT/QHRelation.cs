using System;
using FISCA.UDT;

namespace TeachingEvaluation.UDT
{
    /// <summary>
    /// 層次清單
    /// </summary>
    [FISCA.UDT.TableName("ischool.emba.teaching_evaluation.qh_relation")]
    public class QHRelation : ActiveRecord
    {
        /// <summary>
        /// 層次標題
        /// </summary>
        [Field(Field = "hierarchy_title", Indexed = false, Caption = "層次標題")]
        public string HierarchyTitle { get; set; }

        /// <summary>
        /// 問題系統編號
        /// </summary>
        [Field(Field = "ref_question_id", Indexed = false, Caption = "問題系統編號")]
        public int QuestionID { get; set; }

        /// <summary>
        /// 淺層複製物件
        /// </summary>
        /// <returns></returns>
        public QHRelation Clone()
        {
            return this.MemberwiseClone() as QHRelation;
        }
    }
}
