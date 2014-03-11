using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.UDT;
using System.Xml.Linq;

namespace TeachingEvaluation.BusinessLogic
{
    public class DataPool
    {
        private static readonly Lazy<DataPool> LazyInstance = new Lazy<DataPool>(() => new DataPool());
        public static DataPool Instance { get { return LazyInstance.Value; } }

        private AccessHelper Access;

        private Dictionary<string, UDT.AssignedSurvey> dicAssignedSurveys;
        private Dictionary<string, UDT.Survey> dicSurveys;

        private Dictionary<string, Dictionary<string, UDT.Question>> dicQuestions_Title;
        private Dictionary<string, UDT.Question> dicQuestions_UID;
        private Dictionary<string, List<UDT.Question>> dicQuestions_SurveyID;

        private Dictionary<string, UDT.QuestionOption> dicQuestionOptions_UID;
        private Dictionary<string, List<UDT.QuestionOption>> dicQuestionOptions_QuestionID;
        private Dictionary<string, List<UDT.QuestionOption>> dicQuestionOptions_QuestionTitle;
        private Dictionary<string, Dictionary<string, List<string>>> dicStatisticsGroups;

        private Dictionary<string, UDT.Case> dicCases;        
        private Dictionary<string, List<UDT.QuestionOption>> dicQuestionOptions_QuestionIdAndCaseID;

        private DataPool() 
        {
            Access = new AccessHelper();

            List<UDT.AssignedSurvey> AssignedSurveys = Access.Select<UDT.AssignedSurvey>();
            List<UDT.Survey> Surveys = Access.Select<UDT.Survey>();
            List<UDT.Question> Questions = Access.Select<UDT.Question>();
            List<UDT.QuestionOption> QuestionOptions = Access.Select<UDT.QuestionOption>();
            List<UDT.Case> Cases = Access.Select<UDT.Case>();

            if (Cases.Count > 0)
                dicCases = Cases.ToDictionary(x => x.UID);

            dicAssignedSurveys = new Dictionary<string, UDT.AssignedSurvey>();
            if (AssignedSurveys.Count > 0)
            {
                foreach (UDT.AssignedSurvey AssignedSurvey in AssignedSurveys)
                {
                    if (!dicAssignedSurveys.ContainsKey(AssignedSurvey.CourseID + "-" + AssignedSurvey.TeacherID))
                        dicAssignedSurveys.Add(AssignedSurvey.CourseID + "-" + AssignedSurvey.TeacherID, AssignedSurvey);
                }
            }

            dicSurveys = new Dictionary<string, UDT.Survey>();
            if (Surveys.Count > 0)
            {
                dicSurveys = Surveys.ToDictionary(x => x.UID);
            }

            dicQuestions_UID = new Dictionary<string, UDT.Question>();
            dicQuestions_Title = new Dictionary<string, Dictionary<string, UDT.Question>>();
            dicQuestions_SurveyID = new Dictionary<string, List<UDT.Question>>();
            if (Questions.Count > 0)
            {
                dicQuestions_UID = Questions.ToDictionary(x => x.UID);
                foreach (UDT.Question Q in Questions)
                {
                    if (!dicQuestions_Title.ContainsKey(Q.Title))
                        dicQuestions_Title.Add(Q.Title, new Dictionary<string,UDT.Question>());

                    dicQuestions_Title[Q.Title].Add(Q.UID, Q);

                    if (!dicQuestions_SurveyID.ContainsKey(Q.SurveyID.ToString()))
                        dicQuestions_SurveyID.Add(Q.SurveyID.ToString(), new List<UDT.Question>());

                    dicQuestions_SurveyID[Q.SurveyID.ToString()].Add(Q);
                }
            }

            dicQuestionOptions_UID = new Dictionary<string, UDT.QuestionOption>();
            dicQuestionOptions_QuestionID = new Dictionary<string, List<UDT.QuestionOption>>();
            dicQuestionOptions_QuestionTitle = new Dictionary<string, List<UDT.QuestionOption>>();
            if (QuestionOptions.Count > 0)
            {
                dicQuestionOptions_UID = QuestionOptions.ToDictionary(x => x.UID);
                foreach (UDT.QuestionOption Qo in QuestionOptions)
                {
                    if (!dicQuestionOptions_QuestionID.ContainsKey(Qo.QuestionID.ToString()))
                        dicQuestionOptions_QuestionID.Add(Qo.QuestionID.ToString(), new List<UDT.QuestionOption>());

                    dicQuestionOptions_QuestionID[Qo.QuestionID.ToString()].Add(Qo);

                    if (dicQuestions_UID.ContainsKey(Qo.QuestionID.ToString()))
                    {
                        string question_title = dicQuestions_UID[Qo.QuestionID.ToString()].Title;

                        if (!dicQuestionOptions_QuestionTitle.ContainsKey(question_title))
                            dicQuestionOptions_QuestionTitle.Add(question_title, new List<UDT.QuestionOption>());

                        dicQuestionOptions_QuestionTitle[question_title].Add(Qo);
                    }
                }
            }
            this.SetCaseQuestionOptions();
            this.SetStatisticsGroups();
        }

        public static List<UDT.QuestionOption> GetCaseQuestionOptions(string QuestionID, string CaseID)
        {
            List<UDT.QuestionOption> Qos = new List<UDT.QuestionOption>();

            if (DataPool.Instance.dicQuestionOptions_QuestionIdAndCaseID.ContainsKey(QuestionID + "-" + CaseID))
                Qos = DataPool.Instance.dicQuestionOptions_QuestionIdAndCaseID[QuestionID + "-" + CaseID];

            return Qos;
        }

        private void SetCaseQuestionOptions()
        {
            List<UDT.Case> Cases = Access.Select<UDT.Case>();
            List<UDT.CaseUsage> CaseUsages = Access.Select<UDT.CaseUsage>();
            dicCases = new Dictionary<string, UDT.Case>();
            Dictionary<string, UDT.CaseUsage> dicCaseUsages = new Dictionary<string, UDT.CaseUsage>();            
            dicQuestionOptions_QuestionIdAndCaseID = new Dictionary<string, List<UDT.QuestionOption>>();

            if (Cases.Count == 0)
                return;
            if (CaseUsages.Count == 0)
                return;
            if (dicAssignedSurveys.Count == 0)
                return;
            if (dicQuestions_SurveyID.Count == 0)
                return;

            dicCases = Cases.ToDictionary(x => x.UID);
            foreach (UDT.CaseUsage CU in CaseUsages)
            {
                if (!dicCaseUsages.ContainsKey(CU.CourseID + "-" + CU.TeacherID))
                    dicCaseUsages.Add(CU.CourseID + "-" + CU.TeacherID, CU);
            }

            foreach (string key in dicAssignedSurveys.Keys)
            {
                UDT.AssignedSurvey assignedSurvey = dicAssignedSurveys[key];

                int CourseID = assignedSurvey.CourseID;
                int TeacherID = assignedSurvey.TeacherID;
                int SurveyID = assignedSurvey.SurveyID;

                UDT.CaseUsage CU = new UDT.CaseUsage();
                if (dicCaseUsages.ContainsKey(CourseID + "-" + TeacherID))
                    CU = dicCaseUsages[CourseID + "-" + TeacherID];
                else
                    continue;

                List<UDT.Question> Qs = new List<UDT.Question>();
                if (dicQuestions_SurveyID.ContainsKey(SurveyID.ToString()))
                    Qs = dicQuestions_SurveyID[SurveyID.ToString()];
                foreach (UDT.Question Q in Qs)
                {
                    if (!Q.IsCase)
                        continue;

                    if (Q.Type != "單選題" || Q.Type != "複選題")
                        continue;

                    List<UDT.QuestionOption> Qos = new List<UDT.QuestionOption>();
                    if (dicQuestionOptions_QuestionID.ContainsKey(Q.UID))
                        Qos = dicQuestionOptions_QuestionID[Q.UID];

                    if (!dicQuestionOptions_QuestionIdAndCaseID.ContainsKey(Q.UID + "-" + CU.CaseID))
                        dicQuestionOptions_QuestionIdAndCaseID.Add(Q.UID + "-" + CU.CaseID, Qos);
                }
            }
        }

        public static UDT.AssignedSurvey GetAssignedSurveyByCourseIdAndTeacherID(string CourseID, string TeacherID)
        {
            if (DataPool.Instance.dicAssignedSurveys.ContainsKey(CourseID + "-" + TeacherID))
                return DataPool.Instance.dicAssignedSurveys[CourseID + "-" + TeacherID];
            else
                return null;
        }

        public static UDT.Survey GetSurveyByID(string SurveyID)
        {
            if (DataPool.Instance.dicSurveys.ContainsKey(SurveyID))
                return DataPool.Instance.dicSurveys[SurveyID];
            else
                return null;
        }

        public static UDT.Question GetUQuestionByID(string QuestionID)
        {
            if (DataPool.Instance.dicQuestions_UID.ContainsKey(QuestionID))
                return DataPool.Instance.dicQuestions_UID[QuestionID];
            else
                return null;
        }

        public static List<UDT.Question> GetQuestionsBySurveyID(string SurveyID)
        {
            if (DataPool.Instance.dicQuestions_SurveyID.ContainsKey(SurveyID))
                return DataPool.Instance.dicQuestions_SurveyID[SurveyID];
            else
                return new List<UDT.Question>();
        }

        public static List<UDT.QuestionOption> GetQuestionOptionsByQuestionID(string QuestionID)
        {
            if (DataPool.Instance.dicQuestionOptions_QuestionID.ContainsKey(QuestionID))
                return DataPool.Instance.dicQuestionOptions_QuestionID[QuestionID];
            else
                return new List<UDT.QuestionOption>();
        }

        private void SetStatisticsGroups()
        {
            /// <Questions>
            ///     <Question QuestionID="123" DisplayOrder="1">本課程的內容和學習目標十分明確</Question>
            ///     <Question QuestionID="456" DisplayOrder="2">本課程上課內容充實，且符合教學大綱</Question>
            /// </Questions>
            List<UDT.StatisticsGroup> uStatisticsGroups = Access.Select<UDT.StatisticsGroup>();
            dicStatisticsGroups = new Dictionary<string, Dictionary<string, List<string>>>();
            foreach (UDT.StatisticsGroup uStatisticsGroup in uStatisticsGroups)
            {
                if (!dicStatisticsGroups.ContainsKey(uStatisticsGroup.SurveyID.ToString()))
                    dicStatisticsGroups.Add(uStatisticsGroup.SurveyID.ToString(), new Dictionary<string, List<string>>());

                if (!dicStatisticsGroups[uStatisticsGroup.SurveyID.ToString()].ContainsKey(uStatisticsGroup.Name))
                    dicStatisticsGroups[uStatisticsGroup.SurveyID.ToString()].Add(uStatisticsGroup.Name, new List<string>());

                XDocument xDocument = XDocument.Parse(uStatisticsGroup.DisplayOrderList);
                IEnumerable<XElement> xElements = xDocument.Descendants("Question");
                foreach (XElement xElement in xElements)
                {
                    dicStatisticsGroups[uStatisticsGroup.SurveyID.ToString()][uStatisticsGroup.Name].Add(xElement.Attribute("QuestionID").Value);
                }
            }
        }

        public static Dictionary<string, List<string>> GetStatisticsGroups(string SurveyID)
        {
            Dictionary<string, List<string>> dicQuestionIDs = new Dictionary<string, List<string>>();

            if (DataPool.Instance.dicStatisticsGroups.ContainsKey(SurveyID))
                dicQuestionIDs = DataPool.Instance.dicStatisticsGroups[SurveyID];

            return dicQuestionIDs;
        }

        public static UDT.Case GetUCase(string CaseID)
        {
            if (DataPool.Instance.dicCases.ContainsKey(CaseID))
                return DataPool.Instance.dicCases[CaseID];
            else
                return null;
        }
    }
}