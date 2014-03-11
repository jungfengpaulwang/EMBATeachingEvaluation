
using System.Collections.Generic;
namespace TeachingEvaluation.BusinessLogic
{
    class Case
    {
        protected internal decimal? Score { get; set; }
        protected internal string Content 
        {   
            get 
            {
                if (this.uCase == null)
                    return string.Empty;
                else
                {
                    return (string.IsNullOrWhiteSpace(this.uCase.EnglishName) ? this.uCase.Name : this.uCase.EnglishName);
                }
            } 
        }
        protected internal UDT.Case uCase { get; set; }
        private List<QuestionOption> QuestionOptions;

        public Case(UDT.Case uC)
        {
            this.uCase = uC;
            this.QuestionOptions = new List<QuestionOption>();
            this.Score = null;
        }

        public void SetQuestionOptions(List<QuestionOption> Qos)
        {            
            this.QuestionOptions = Qos;
        }

        public List<QuestionOption> GetQuestionOptions()
        {
            return this.QuestionOptions;
        }
    }
}
//  <Question No='1~2' Content='本門課我的出席狀況是' Type='單選題' IsSelfAssessment='是' IsCase='否' Score='4.58'>