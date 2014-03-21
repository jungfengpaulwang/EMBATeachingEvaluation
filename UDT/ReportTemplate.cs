using FISCA.UDT;

namespace TeachingEvaluation.UDT
{
    [TableName("ischool.emba.teaching_evaluation.report_template")]
    public class ReportTemplate : ActiveRecord
    {
        /// <summary>
        /// 問卷樣版系統編號
        /// </summary>
        [Field(Field = "ref_survey_id", Indexed = true, Caption = "問卷樣版系統編號")]
        public int SurveyID { get; set; }

        /// <summary>
        /// 樣版檔：base64string 格式。
        /// </summary>
        [Field(Field = "template")]
        public string Template { get; set; }

        /// <summary>
        /// 淺層複製物件內容
        /// </summary>
        /// <returns></returns>
        public ReportTemplate Clone()
        {
            return this.MemberwiseClone() as ReportTemplate;
        }
    }
}
