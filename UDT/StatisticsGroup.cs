using System;
using FISCA.UDT;

namespace TeachingEvaluation.UDT
{
    /// <summary>
    /// 統計群組
    /// </summary>
    [FISCA.UDT.TableName("ischool.emba.teaching_evaluation.statistics_group")]
    public class StatisticsGroup : ActiveRecord
    {
        /// <summary>
        /// 名稱
        /// </summary>
        [Field(Field = "name", Indexed = true, Caption = "名稱")]
        public string Name { get; set; }

        /// <summary>
        /// 評鑑樣版系統編號
        /// </summary>
        [Field(Field = "ref_survey_id", Indexed = true, Caption = "評鑑樣版系統編號")]
        public int SurveyID { get; set; }

        /// <summary>
        /// 題號
        /// <Questions>
        ///     <Question QuestionID="123" DisplayOrder="1">本課程的內容和學習目標十分明確</Question>
        ///     <Question QuestionID="456" DisplayOrder="2">本課程上課內容充實，且符合教學大綱</Question>
        /// </Questions>
        /// </summary>
        [Field(Field = "display_order_list", Indexed = false, Caption = "題號")]
        public string DisplayOrderList { get; set; }

        /// <summary>
        /// 題目背景顏色
        /// </summary>
        [Field(Field = "question_bg_color", Indexed = false, Caption = "題目背景顏色")]
        public string QuestionBgColor { get; set; }

        /// <summary>
        /// 評鑑值背景顏色
        /// </summary>
        [Field(Field = "evaluation_bg_color", Indexed = false, Caption = "評鑑值背景顏色")]
        public string EvaluationBgColor { get; set; }

        /// <summary>
        /// 淺層複製物件
        /// </summary>
        /// <returns></returns>
        public StatisticsGroup Clone()
        {
            return this.MemberwiseClone() as StatisticsGroup;
        }
    }
}
