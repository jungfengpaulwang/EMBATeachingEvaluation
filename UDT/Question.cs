using System;
using FISCA.UDT;

namespace TeachingEvaluation.UDT
{
    /// <summary>
    /// 問卷題目
    /// </summary>
    [FISCA.UDT.TableName("ischool.emba.teaching_evaluation.question_survey")]
    public class Question : ActiveRecord
    {
        /// <summary>
        /// 問卷系統編號
        /// </summary>
        [Field(Field = "ref_survey_id", Indexed = true, Caption = "問卷系統編號")]
        public int SurveyID { get; set; }

        /// <summary>
        /// 題目
        /// </summary>
        [Field(Field = "title", Indexed = false, Caption = "題目")]
        public string Title { get; set; }

        /// <summary>
        /// 題目類型：單選題、問答題
        /// </summary>
        [Field(Field = "type", Indexed = false, Caption = "題目類型")]
        public string Type { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        [Field(Field = "is_required", Indexed = false, Caption = "是否必填")]
        public bool IsRequired { get; set; }

        /// <summary>
        /// 個案題型
        /// </summary>
        [Field(Field = "is_case", Indexed = false, Caption = "個案題型")]
        public bool IsCase { get; set; }

        /// <summary>
        /// 自評題型
        /// </summary>
        [Field(Field = "is_self_assessment", Indexed = false, Caption = "自評題型")]
        public bool IsSelfAssessment { get; set; }

        /// <summary>
        /// 不列入評鑑值計算
        /// </summary>
        [Field(Field = "is_none_calculated", Indexed = false, Caption = "不列入評鑑值計算")]
        public bool IsNoneCalculated { get; set; }

        /// <summary>
        /// 題號
        /// </summary>
        [Field(Field = "display_order", Indexed = false, Caption = "題號")]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 淺層複製物件
        /// </summary>
        /// <returns></returns>
        public Question Clone()
        {
            return this.MemberwiseClone() as Question;
        }
    }
}
