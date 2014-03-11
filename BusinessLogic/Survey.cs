using System.Collections.Generic;

namespace TeachingEvaluation.BusinessLogic
{
    class Survey
    {
        protected internal List<Question> Questions { get; set; }
        protected internal UDT.Survey uSurvey { get; set; }
        public Survey(UDT.Survey survey)
        {
            this.Questions = new List<Question>();
            this.uSurvey = survey;

            this.InitQuestions();
        }

        protected internal void AddQuestion(Question Q)
        {
            this.Questions.Add(Q);
        }

        private void InitQuestions()
        {
            if (this.uSurvey == null)
                return;
            List<UDT.Question> uQs = DataPool.GetQuestionsBySurveyID(this.uSurvey.UID);
            uQs.ForEach((x) =>
            {
                Question Q = new Question(x);

                this.AddQuestion(Q);
            });
        }
    }
}