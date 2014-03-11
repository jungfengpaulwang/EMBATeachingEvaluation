
namespace TeachingEvaluation.BusinessLogic
{
    class QuestionOption
    {
        protected internal int No { get { return (this.uQuestionOption == null ? 0 : this.uQuestionOption.DisplayOrder); } }
        protected internal string Content { get { return (this.uQuestionOption == null ? string.Empty : this.uQuestionOption.Title); } }
        protected internal string CaseID { get; set; }
        protected internal int AnswerCount { get; set; }
        protected internal UDT.QuestionOption uQuestionOption { get; set; }

        public QuestionOption(UDT.QuestionOption uQ)
        {
            this.uQuestionOption = uQ;
            this.AnswerCount = 0;
        }
    }
}
//<Option No='1' Content='從不缺課' AnswerCount='6' />