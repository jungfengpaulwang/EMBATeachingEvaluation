using System;
using FISCA.UDT;

namespace TeachingEvaluation.UDT
{
    /// <summary>
    /// 答案選項
    /// </summary>
    [FISCA.UDT.TableName("ischool.emba.teaching_evaluation.option_question")]
    public class QuestionOption : ActiveRecord
    {
        /// <summary>
        /// 問題系統編號
        /// </summary>
        [Field(Field = "ref_question_id", Indexed = true, Caption = "問題系統編號")]
        public int QuestionID { get; set; }

        /// <summary>
        /// 項目
        /// </summary>
        [Field(Field = "title", Indexed = false, Caption = "項目")]
        public string Title { get; set; }

        /// <summary>
        /// 題號
        /// </summary>
        [Field(Field = "display_order", Indexed = false, Caption = "題號")]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 淺層複製物件
        /// </summary>
        /// <returns></returns>
        public QuestionOption Clone()
        {
            return this.MemberwiseClone() as QuestionOption;
        }
    }
}
