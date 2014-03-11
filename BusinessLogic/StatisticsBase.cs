using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FISCA.UDT;
using System.Web;

namespace TeachingEvaluation.BusinessLogic
{
    abstract class StatisticsBase
    {
        public string CSAttendCount { get; set; }
        public string FeedBackCount { get; set; }
        public string TeacherName { get; set; }
        public string CourseName { get; set; }
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public string NewSubjectCode { get; set; }
        public string SchoolYear { get; set; }
        public string Semester { get; set; }
        public string SubjectID { get; set; }
        public string CourseID { get; set; }
        public string TeacherID { get; set; }
        public string SurveyDate { get; set; }
        public Survey Survey { get; set; }
        public List<Reply> Replys { get; set; } 

        public StatisticsBase(string CourseID, string TeacherID)
        {
            UDT.AssignedSurvey assignedSurvey = DataPool.GetAssignedSurveyByCourseIdAndTeacherID(CourseID, TeacherID);
            if (assignedSurvey == null)
                return;
            UDT.Survey survey = DataPool.GetSurveyByID(assignedSurvey.SurveyID.ToString());
            this.Survey = new Survey(survey);
            this.Replys = new List<Reply>();
        }

        public void SetAnswer(string Answer)
        {
            /// 評鑑做答，格式如下：
            /// <Answers>
            ///     <Question QuestionID="">
            ///         <Answer CaseID="" Score="3">尚可</Answer>
            ///         <Answer CaseID="123" Score="5">很滿意</Answer>
            ///     </Question>
            /// </Answers>
            Reply reply = new Reply(Answer);
            this.Replys.Add(reply);
        }
        
        private List<Question> CalcQuestionScore()
        {
            if (this.Survey == null)
                return new List<Question>();

            foreach (Question Q in this.Survey.Questions)
            {
                //  問答題不需計算
                if (Q.uQuestion.Type == "問答題")
                    continue;

                //  不列入評鑑值計算者不需計算
                //if (Q.uQuestion.IsNoneCalculated)
                //    continue;                    

                List<QuestionOption> Qos = Q.QuestionOptions;
                Dictionary<string, QuestionOption> dicQuestionOptions = new Dictionary<string, QuestionOption>();
                if (Qos.Count == 0)
                    continue;
                foreach (QuestionOption Qo in Qos)
                {
                    if (!dicQuestionOptions.ContainsKey(Qo.uQuestionOption.Title))
                        dicQuestionOptions.Add(Qo.uQuestionOption.Title, Qo);
                }

                List<Answer> Answers = new List<Answer>();
                foreach (Reply reply in this.Replys)
                {
                    Question QQ = reply.GetQuestionByID(Q.uQuestion.UID);
                    if (QQ != null)
                        Answers.AddRange(QQ.Answers);
                }
                foreach (Answer answer in Answers)
                {                    
                    if (answer.Case != null)
                    {
                        Q.AddCase(answer.Case);
                    }
                    if (dicQuestionOptions.ContainsKey(answer.Content))
                        dicQuestionOptions[answer.Content].AnswerCount += 1;
                }
                List<Case> qCases = Q.Cases;
                foreach (Case Case in qCases)
                {
                    List<QuestionOption> cQuestionOptions = new List<QuestionOption>();
                    foreach (QuestionOption Qo in Qos)
                    {
                        QuestionOption nQuestionOption = new QuestionOption(Qo.uQuestionOption);
                        cQuestionOptions.Add(nQuestionOption);
                    }
                    Case.SetQuestionOptions(cQuestionOptions);
                }
                foreach (Answer answer in Answers)
                {
                    if (answer.Case == null)
                        continue;

                    if (answer.Case.uCase == null)
                        continue;

                    Case Case = Q.GetCaseByID(answer.Case.uCase.UID);
                    if (Case == null)
                        continue;
                    foreach (QuestionOption Qo in Case.GetQuestionOptions())
                    {
                        if (Qo.Content == answer.Content)
                            Qo.AnswerCount += 1;
                    }
                }
                if (Q.uQuestion.IsCase)
                {
                    List<Case> Cases = Q.Cases;
                    decimal tAnswerCounts = 0;
                    decimal tSumScores = 0;
                    foreach (Case cazz in Cases)
                    {
                        decimal AnswerCounts = 0;
                        decimal SumScores = 0;
                        cazz.GetQuestionOptions().ForEach((x) =>
                        {
                            AnswerCounts += x.AnswerCount;
                            SumScores += x.No * x.AnswerCount;
                            tAnswerCounts += x.AnswerCount;
                            tSumScores += x.No * x.AnswerCount;
                        });
                        if (AnswerCounts > 0)
                            cazz.Score = Math.Round(SumScores / AnswerCounts, 2, MidpointRounding.AwayFromZero);
                    }
                    if (tAnswerCounts > 0)
                        Q.Score = Math.Round(tSumScores / tAnswerCounts, 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    decimal AnswerCounts = 0;
                    decimal SumScores = 0;
                    Qos.ForEach((x) =>
                    {
                        AnswerCounts += x.AnswerCount;
                        SumScores += x.No * x.AnswerCount;
                    });
                    if (AnswerCounts > 0)
                        Q.Score = Math.Round(SumScores / AnswerCounts, 2, MidpointRounding.AwayFromZero);
                }
            }

            return this.Survey.Questions;
        }

        private Dictionary<string, decimal?> CalcStatisticsGroupScore(List<Question> CalculatedQuestions)
        {
            Dictionary<string, decimal?> dicStatisticsGroupScores = new Dictionary<string, decimal?>();
            Dictionary<string, List<string>> dicStatisticsGroups = DataPool.GetStatisticsGroups(this.Survey.uSurvey.UID);
            foreach (string key in dicStatisticsGroups.Keys)
            {
                List<string> QuestionIDs = dicStatisticsGroups[key];
                decimal AnswerCounts = 0;
                decimal SumScores = 0;
                foreach (Question Q in CalculatedQuestions)
                {
                    if (!Q.Score.HasValue)
                        continue;

                    if (QuestionIDs.Contains(Q.uQuestion.UID))
                    {
                        AnswerCounts += 1;
                        SumScores += Q.Score.Value;
                    }
                }
                if (!dicStatisticsGroupScores.ContainsKey(key))
                {
                    if (AnswerCounts > 0)
                        dicStatisticsGroupScores.Add(key, Math.Round(SumScores / AnswerCounts, 2, MidpointRounding.AwayFromZero));
                    else
                        dicStatisticsGroupScores.Add(key, null);
                }
            }

            return dicStatisticsGroupScores;
        }

        private string GetEssayString()
        {
            StringBuilder sb = new StringBuilder();

            //if (this.Replys.Count == 0)
            //    return string.Empty;

            List<UDT.Question> oQs = DataPool.GetQuestionsBySurveyID(this.Survey.uSurvey.UID).Where(x => x.Type == "問答題").ToList();
            List<Question> Qs = new List<Question>();
            this.Replys.ForEach((y) => Qs.AddRange(y.Questions.Where(x=>x.uQuestion.Type == "問答題")));
            Qs = Qs.OrderBy(x=>x.No).ToList();
            Dictionary<string, List<Question>> dicQuestions = new Dictionary<string,List<Question>>();
            Qs.ForEach((x) =>
            {
                if (!dicQuestions.ContainsKey(x.uQuestion.UID))
                    dicQuestions.Add(x.uQuestion.UID, new List<Question>());

                dicQuestions[x.uQuestion.UID].Add(x);
            });
            oQs.ForEach((x) =>
            {
                if (!dicQuestions.ContainsKey(x.UID))
                    dicQuestions.Add(x.UID, new List<Question>());

                Question q = new Question(x);
                dicQuestions[x.UID].Add(q);
            });            
                
            foreach(string key in dicQuestions.Keys.OrderBy(x=>x))
            {
                Question q = dicQuestions[key].ElementAt(0);
                sb.Append(string.Format(@"<Question ID=""{5}"" No=""{0}"" Content=""{1}"" Type=""{2}"" IsSelfAssessment=""{3}"" IsCase=""{4}"">", q.uQuestion.DisplayOrder, HttpUtility.HtmlEncode(q.uQuestion.Title), q.uQuestion.Type, q.uQuestion.IsSelfAssessment ? "是" : "否", q.uQuestion.IsCase ? "是" : "否", q.uQuestion.UID));
                sb.Append("<Answers>");
                List<Answer> Answers = new List<Answer>();
                dicQuestions[key].ForEach((x) =>
                {
                    if (x.Answers != null)
                        Answers.AddRange(x.Answers);
                });
                foreach (Answer answer in Answers)
                {
                    sb.Append(string.Format(@"<Answer>{0}</Answer>", answer.Score));
                }
                sb.Append("</Answers></Question>");
            }
            return sb.ToString();
        }

        private string GetCaseString(Question q)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format(@"<Question ID=""{5}"" No=""{0}"" Content=""{1}"" Type=""{2}"" IsSelfAssessment=""{3}"" IsCase=""{4}"">", q.No, HttpUtility.HtmlEncode(q.Content), q.uQuestion.Type, q.uQuestion.IsSelfAssessment ? "是" : "否", q.uQuestion.IsCase ? "是" : "否", q.uQuestion.UID));

            List<Case> Cases = q.Cases;
            if (Cases.Count == 0)
                return sb.Append("</Question>").ToString();

            foreach (Case cazz in Cases)
            {
                if (cazz.uCase == null)
                    continue;

                sb.Append(string.Format(@"<Case ID=""{0}"" Content=""{1}"" Score=""{2}"">", cazz.uCase.UID, HttpUtility.HtmlEncode(cazz.Content), cazz.Score.HasValue ? cazz.Score.Value.ToString() : string.Empty));
                List<QuestionOption> CQos = cazz.GetQuestionOptions();
                foreach (QuestionOption Qo in CQos)
                    sb.Append(string.Format(@"<Option ID=""{3}"" No=""{0}"" Content=""{1}"" AnswerCount=""{2}"" />", Qo.No, HttpUtility.HtmlEncode(Qo.Content), Qo.AnswerCount, Qo.uQuestionOption.UID));

                sb.Append("</Case>");
            }

            return sb.Append("</Question>").ToString();
        }

        private string GetSingleChoiceNoneCaseString(Question q)
        {
            List<QuestionOption> Qos = q.QuestionOptions;
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format(@"<Question ID=""{6}"" No=""{0}"" Content=""{1}"" Type=""{2}"" IsSelfAssessment=""{3}"" IsCase=""{4}"" Score=""{5}"">", q.No, HttpUtility.HtmlEncode(q.Content), q.uQuestion.Type, q.uQuestion.IsSelfAssessment ? "是" : "否", q.uQuestion.IsCase ? "是" : "否", q.Score.HasValue ? q.Score.Value.ToString() : string.Empty, q.uQuestion.UID));
            foreach (QuestionOption Qo in Qos)
                sb.Append(string.Format(@"<Option ID=""{3}"" No=""{0}"" Content=""{1}"" AnswerCount=""{2}"" />", Qo.No, HttpUtility.HtmlEncode(Qo.Content), Qo.AnswerCount, Qo.uQuestionOption.UID));

            return sb.Append("</Question>").ToString();
        }

        private string GetStatisticsGroupString(List<Question> CalculatedQuestions)
        {
            Dictionary<string, decimal?> dicStatisticsGroupScores = this.CalcStatisticsGroupScore(CalculatedQuestions);
            StringBuilder sb = new StringBuilder();
            foreach (string key in dicStatisticsGroupScores.Keys)
            {
                decimal? Score = dicStatisticsGroupScores[key];
                sb.Append(string.Format(@"<StatisticsGroup Content=""{0}"" Score=""{1}"" />", HttpUtility.HtmlEncode(key), Score.HasValue ? Score.Value.ToString() : string.Empty));
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            List<Question> CalculatedQuestions = this.CalcQuestionScore();
            CalculatedQuestions = CalculatedQuestions.OrderByDescending(x => x.uQuestion.IsSelfAssessment).ThenBy(x => x.No).ToList();

            StringBuilder sb = new StringBuilder(string.Format(@"<Statistics CSAttendCount=""{0}"" FeedBackCount=""{1}"" TeacherName=""{2}"" CourseName=""{3}"" SubjectName=""{4}"" SubjectCode=""{5}"" NewSubjectCode=""{6}"" SchoolYear=""{7}"" Semester=""{8}"" ClassName=""{9}"" CourseID=""{10}"" SubjectID=""{11}"" TeacherID=""{12}"" SurveyDate=""{13}"" SurveyID=""{14}"">", this.CSAttendCount, this.FeedBackCount, HttpUtility.HtmlEncode(this.TeacherName), HttpUtility.HtmlEncode(this.CourseName), HttpUtility.HtmlEncode(this.SubjectName), this.SubjectCode, this.NewSubjectCode, this.SchoolYear, this.Semester, this.ClassName, this.CourseID, this.SubjectID, this.TeacherID, this.SurveyDate, this.Survey.uSurvey.UID));
            foreach (Question q in CalculatedQuestions)
            {
                if ((q.uQuestion.Type == "單選題" || q.uQuestion.Type == "複選題") && !q.uQuestion.IsCase)
                    sb.Append(this.GetSingleChoiceNoneCaseString(q));

                if ((q.uQuestion.Type == "單選題" || q.uQuestion.Type == "複選題") && q.uQuestion.IsCase)
                    sb.Append(this.GetCaseString(q));
            }
            //  問答題
            sb.Append(this.GetEssayString());
            //  平均平鑑值
            string StatisticsGroupString = this.GetStatisticsGroupString(CalculatedQuestions);
            if (!string.IsNullOrEmpty(StatisticsGroupString))
                sb.Append(StatisticsGroupString);

            return sb.Append("</Statistics>").ToString();
        }
    }
}
