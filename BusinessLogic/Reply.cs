using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TeachingEvaluation.BusinessLogic
{
    class Reply
    {
        public List<Question> Questions { get; set; }

        public Reply(string answer)
        {
            this.Questions = new List<Question>();
            this.SetAnswers(answer);
        }

        public Question GetQuestionByID(string QuestionID)
        {
            IEnumerable<Question> filterQuestions = this.Questions.Where(x => x.uQuestion.UID == QuestionID);

            if (filterQuestions.Count() > 0)
                return filterQuestions.ElementAt(0);
            else
                return null;
        }

        private void SetAnswers(string answer)
        {
            /// 評鑑做答，格式如下：
            /// <Answers>
            ///     <Question QuestionID="">
            ///         <Answer CaseID="" Score="3">尚可</Answer>
            ///         <Answer CaseID="123" Score="5">很滿意</Answer>
            ///     </Question>
            /// </Answers>
            /// 
            XDocument xDocument = XDocument.Parse(answer, LoadOptions.None);
            IEnumerable<XElement> xElements = xDocument.Descendants("Question");
            foreach (XElement xElement in xElements)
            {
                string QuestionID = xElement.Attribute("QuestionID").Value;
                UDT.Question uQ = DataPool.GetUQuestionByID(QuestionID);
                Question Q = new Question(uQ);
                this.Questions.Add(Q);
                IEnumerable<XElement> xAnswers = xElement.Descendants("Answer");
                foreach(XElement xAnswer in xAnswers)
                {
                    Answer Answer = new Answer();

                    if (!string.IsNullOrEmpty(xAnswer.Attribute("CaseID").Value))
                    {
                        UDT.Case uCase = DataPool.GetUCase(xAnswer.Attribute("CaseID").Value);
                        Case Case = new Case(uCase);
                        Answer.Case = Case;
                    }
                    Answer.Score = xAnswer.Attribute("Score").Value;
                    Answer.Content = xAnswer.Value;

                    Q.AddAnswer(Answer);
                }
            }            
        }
    }
}