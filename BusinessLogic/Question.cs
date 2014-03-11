
using System.Collections.Generic;
using System.Linq;

namespace TeachingEvaluation.BusinessLogic
{
    class Question
    {
        protected internal decimal? Score { get; set; }
        protected internal int No { get { return (this.uQuestion == null ?  0 : this.uQuestion.DisplayOrder); } }
        protected internal string Content { get { return (this.uQuestion == null ? string.Empty : this.uQuestion.Title); } }
        protected internal UDT.Question uQuestion { get; set; }
        protected internal List<QuestionOption> QuestionOptions { get; set; }
        protected internal List<Case> Cases { get; set; }
        protected internal List<Answer> Answers { get; set; }

        public Question(UDT.Question uQ)
        {
            this.uQuestion = uQ;
            this.QuestionOptions = new List<QuestionOption>();
            this.Answers = new List<Answer>();
            this.Cases = new List<Case>();
            this.InitQuestionOptions();
            this.Score = null;
        }

        private void InitQuestionOptions()
        {
            if (this.uQuestion == null)
                return;
            List<UDT.QuestionOption> uQOs = DataPool.GetQuestionOptionsByQuestionID(this.uQuestion.UID);

            uQOs.ForEach((x) =>
            {
                QuestionOption QO = new QuestionOption(x);

                this.AddQuestionOption(QO);
            });
        }

        protected internal void AddQuestionOption(QuestionOption QO)
        {
            this.QuestionOptions.Add(QO);
        }

        protected internal void AddAnswer(Answer answer)
        {
            this.Answers.Add(answer);
        }

        protected internal void AddCase(Case cazz)
        {
            if (this.Cases.Where(x => x.uCase.UID == cazz.uCase.UID).Count() == 0)
                this.Cases.Add(cazz);
        }

        protected internal Case GetCaseByID(string CaseID)
        {
            IEnumerable<Case> filterCases = this.Cases.Where(x => x.uCase.UID == CaseID);
            if (filterCases.Count() > 0)
                return filterCases.ElementAt(0);
            else
                return null;
        }
    }
}
//  <Question No='1~2' Content='本門課我的出席狀況是' Type='單選題' IsSelfAssessment='是' IsCase='否' Score='4.58'>