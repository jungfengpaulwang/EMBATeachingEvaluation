using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using FISCA.Data;
using FISCA.UDT;
using System.Threading.Tasks;
using K12.Data;
using Aspose.Cells;
using System.Xml.Linq;
using ReportHelper;
using System.IO;

namespace TeachingEvaluation.Export
{
    public partial class ExportEvaluation_Teacher : BaseForm
    {
        private QueryHelper Query;
        private AccessHelper Access;

        private int SchoolYear;
        private int Semester;

        public ExportEvaluation_Teacher()
        {
            InitializeComponent();
            this.Text = "列印教師評鑑統計";
            this.TitleText = "列印教師評鑑統計";
            Query = new QueryHelper();
            Access = new AccessHelper();

            this.Load += new EventHandler(ExportEvaluation_Teacher_Load);
        }

        private void ExportEvaluation_Teacher_Load(object sender, EventArgs e)
        {
            this.circularProgress.IsRunning = false;
            this.circularProgress.Visible = false;

            this.InitSchoolYear();
            this.InitSemester();
        }

        private void InitSchoolYear()
        {
            int DefaultSchoolYear;
            if (int.TryParse(K12.Data.School.DefaultSchoolYear, out DefaultSchoolYear))
            {
                this.nudSchoolYear.Value = decimal.Parse(DefaultSchoolYear.ToString());
                this.nudSchoolYear1.Value = decimal.Parse(DefaultSchoolYear.ToString());
            }
            else
            {
                this.nudSchoolYear.Value = decimal.Parse((DateTime.Today.Year - 1911).ToString());
                this.nudSchoolYear1.Value = decimal.Parse((DateTime.Today.Year - 1911).ToString());
            }
        }

        private void InitSemester()
        {
            this.cboSemester.DataSource = DataItems.SemesterItem.GetSemesterList();
            this.cboSemester.ValueMember = "Value";
            this.cboSemester.DisplayMember = "Name";

            this.cboSemester1.DataSource = DataItems.SemesterItem.GetSemesterList();
            this.cboSemester1.ValueMember = "Value";
            this.cboSemester1.DisplayMember = "Name";

            this.cboSemester.SelectedValue = K12.Data.School.DefaultSemester;
            this.cboSemester1.SelectedValue = K12.Data.School.DefaultSemester;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private void Export_Click(object sender, EventArgs e)
        //{
        //    int school_year = int.Parse(this.nudSchoolYear.Value + "");
        //    int semester = int.Parse((this.cboSemester.SelectedItem as DataItems.SemesterItem).Value);
        //    int school_year1 = int.Parse(this.nudSchoolYear1.Value + "");
        //    int semester1 = int.Parse((this.cboSemester1.SelectedItem as DataItems.SemesterItem).Value);

        //    List<CourseRecord> Courses = new List<CourseRecord>();
        //    List<TeacherRecord> Teachers = new List<TeacherRecord>();
        //    Dictionary<string, CourseRecord> dicCourses = new Dictionary<string, CourseRecord>();
        //    Dictionary<string, TeacherRecord> dicTeachers = new Dictionary<string, TeacherRecord>();
        //    List<UDT.TeacherStatistics> Statistics = new List<UDT.TeacherStatistics>();
        //    Dictionary<string, List<DataSet>> dicTeacherStatistics = new Dictionary<string, List<DataSet>>();
        //    this.circularProgress.Visible = true;
        //    this.circularProgress.IsRunning = true;
        //    this.Export.Enabled = false;
        //    Workbook wb = new Workbook();
        //    Dictionary<ReportHelper.CellObject, ReportHelper.CellStyle> dicCellStyles = new Dictionary<CellObject,CellStyle>();
        //    Task task = Task.Factory.StartNew(() =>
        //    {
        //        Statistics = Access.Select<UDT.TeacherStatistics>();
        //        Statistics = Statistics.OrderBy(x => x.SchoolYear).ThenBy(x => x.Semester).ThenBy(x => x.CourseID).ThenBy(x => x.TeacherID).ToList();
        //        List<UDT.StatisticsGroup> StatisticsGroups = Access.Select<UDT.StatisticsGroup>();
        //        Dictionary<string, Dictionary<string, Color>> dicQuestionBackgroundColor = new Dictionary<string, Dictionary<string, Color>>();
        //        Dictionary<string, Dictionary<string, Color>> dicEvaluationBackgroundColor = new Dictionary<string, Dictionary<string, Color>>();
        //        StatisticsGroups.ForEach((x) =>
        //        {
        //            XDocument xDocument = XDocument.Parse(x.DisplayOrderList, LoadOptions.None);
        //            List<XElement> xElements = xDocument.Descendants("Question").ToList();
        //            if (!string.IsNullOrEmpty(x.QuestionBgColor) && xElements.Count() > 0)
        //            {
        //                foreach (string display_order in xElements.Select(y => y.Attribute("DisplayOrder").Value))
        //                {
        //                    if (!dicQuestionBackgroundColor.ContainsKey(x.SurveyID.ToString()))
        //                        dicQuestionBackgroundColor.Add(x.SurveyID.ToString(), new Dictionary<string, Color>());

        //                    dicQuestionBackgroundColor[x.SurveyID.ToString()].Add(display_order, Color.FromName(x.QuestionBgColor));
        //                }
        //            }
        //            if (!dicEvaluationBackgroundColor.ContainsKey(x.SurveyID.ToString()))
        //                dicEvaluationBackgroundColor.Add(x.SurveyID.ToString(), new Dictionary<string, Color>());

        //            dicEvaluationBackgroundColor[x.SurveyID.ToString()].Add(x.Name, Color.FromName(x.EvaluationBgColor));
        //        });
        //        foreach (UDT.TeacherStatistics Statistic in Statistics)
        //        {
        //            if (Statistic.SchoolYear < school_year || Statistic.SchoolYear > school_year1)
        //                continue;
        //            if (Statistic.SchoolYear == school_year && Statistic.Semester < semester)
        //                continue;
        //            if (Statistic.SchoolYear == school_year1 && Statistic.Semester > semester1)
        //                continue;

        //            XDocument xDocument = new XDocument();
        //            xDocument = XDocument.Parse(Statistic.StatisticsList.Replace("&", "&#038;"), LoadOptions.None);
        //            XElement xStatistics = xDocument.Element("Statistics");
        //            IEnumerable<XElement> xQuestions_SelfAssessment = xDocument.Descendants("Question").Where(x => (x.Attribute("IsSelfAssessment").Value == "是" && x.Attribute("Type").Value == "單選題"));
        //            IEnumerable<XElement> xQuestions_NoneSelfAssessment = xDocument.Descendants("Question").Where(x => (x.Attribute("IsSelfAssessment").Value == "否" && x.Attribute("Type").Value == "單選題" && x.Attribute("IsCase").Value == "否"));
        //            IEnumerable<XElement> xQuestions_Case = xDocument.Descendants("Question").Where(x => (x.Attribute("IsCase").Value == "是" && x.Attribute("Type").Value == "單選題"));
        //            IEnumerable<XElement> xQuestions_Essay = xDocument.Descendants("Question").Where(x => x.Attribute("Type").Value == "問答題");
        //            IEnumerable<XElement> xStatisticsGroup = xDocument.Descendants("StatisticsGroup");

        //            #region 表頭

        //            DataSet dataSet_PageHeader = new DataSet("PageHeader");
        //            string CSAttendCount = xStatistics.Attribute("CSAttendCount").Value;
        //            string FeedBackCount = xStatistics.Attribute("FeedBackCount").Value;
        //            string TeacherName = xStatistics.Attribute("TeacherName").Value;
        //            string CourseName = xStatistics.Attribute("CourseName").Value;
        //            string SubjectName = xStatistics.Attribute("SubjectName").Value;
        //            string ClassName = xStatistics.Attribute("ClassName").Value;
        //            string SubjectCode = xStatistics.Attribute("SubjectCode").Value;
        //            string NewSubjectCode = xStatistics.Attribute("NewSubjectCode").Value;
        //            string SchoolYear = xStatistics.Attribute("SchoolYear").Value;
        //            string Semester = xStatistics.Attribute("Semester").Value;
        //            string SurveyDate = xStatistics.Attribute("SurveyDate").Value;
        //            string SurveyID = xStatistics.Attribute("SurveyID").Value;
                    
        //            dataSet_PageHeader.Tables.Add(CSAttendCount.ToDataTable("CSAttendCount", "CSAttendCount"));
        //            dataSet_PageHeader.Tables.Add(FeedBackCount.ToDataTable("FeedBackCount", "FeedBackCount"));
        //            dataSet_PageHeader.Tables.Add(TeacherName.ToDataTable("TeacherName", "TeacherName"));
        //            dataSet_PageHeader.Tables.Add(CourseName.ToDataTable("CourseName", "CourseName"));
        //            dataSet_PageHeader.Tables.Add(SubjectName.ToDataTable("SubjectName", "SubjectName"));
        //            dataSet_PageHeader.Tables.Add(ClassName.ToDataTable("ClassName", "ClassName"));
        //            dataSet_PageHeader.Tables.Add(SubjectCode.ToDataTable("SubjectCode", "SubjectCode"));
        //            dataSet_PageHeader.Tables.Add(NewSubjectCode.ToDataTable("NewSubjectCode", "NewSubjectCode"));
        //            dataSet_PageHeader.Tables.Add(SchoolYear.ToDataTable("SchoolYear", "SchoolYear"));
        //            dataSet_PageHeader.Tables.Add(DataItems.SemesterItem.GetSemesterByCode(xStatistics.Attribute("Semester").Value).Name.ToDataTable("Semester", "Semester"));
        //            dataSet_PageHeader.Tables.Add(SurveyDate.ToDataTable("SurveyDate", "SurveyDate"));

        //            string key = SchoolYear + "-" + Semester + "-" + xStatistics.Attribute("CourseName").Value + "-" + xStatistics.Attribute("TeacherName").Value;

        //            #region 自評題
        //            DataTable EnrollRecordTable = new DataTable("SelfAssessmentContent");
        //            EnrollRecordTable.Columns.Add("Content");
        //            EnrollRecordTable.Columns.Add("Option1AnswerCount");
        //            EnrollRecordTable.Columns.Add("Option2AnswerCount");
        //            EnrollRecordTable.Columns.Add("Option3AnswerCount");
        //            EnrollRecordTable.Columns.Add("Option4AnswerCount");
        //            EnrollRecordTable.Columns.Add("Option5AnswerCount");
        //            EnrollRecordTable.Columns.Add("Score");

        //            foreach (XElement xElement in xQuestions_SelfAssessment)
        //            {
        //                DataRow pRow = EnrollRecordTable.NewRow();

        //                string question_content = xElement.Attribute("Content").Value;
        //                pRow["Option1AnswerCount"] = 0;
        //                pRow["Option2AnswerCount"] = 0;
        //                pRow["Option3AnswerCount"] = 0;
        //                pRow["Option4AnswerCount"] = 0;
        //                pRow["Option5AnswerCount"] = 0;
        //                pRow["Score"] = xElement.Attribute("Score").Value;
        //                IEnumerable<XElement> Options = xElement.Descendants("Option");
        //                int j = 0;
        //                foreach (XElement xOption in Options)
        //                {
        //                    j++;
        //                    pRow["Option" + j + "AnswerCount"] = xOption.Attribute("AnswerCount").Value;
        //                    question_content += "(" + xOption.Attribute("No").Value + ")" + xOption.Attribute("Content").Value + "、";
        //                }
        //                if (question_content.EndsWith("、"))
        //                    question_content = question_content.Remove(question_content.Length - 1, 1);
        //                pRow["Content"] = question_content;
        //                EnrollRecordTable.Rows.Add(pRow);
        //            }
        //            dataSet_PageHeader.Tables.Add(EnrollRecordTable);
        //            #endregion

        //            #endregion

        //            #region 資料區

        //            DataSet dataSet_DataSection = new DataSet("DataSection");

        //            #region 非自評、統計群組、問答題
        //            DataTable dataTable_NoneSelfAssessment = new DataTable("NoneSelfAssessmentContent");
        //            dataTable_NoneSelfAssessment.Columns.Add("No");
        //            dataTable_NoneSelfAssessment.Columns.Add("Content");
        //            dataTable_NoneSelfAssessment.Columns.Add("Option1AnswerCount");
        //            dataTable_NoneSelfAssessment.Columns.Add("Option2AnswerCount");
        //            dataTable_NoneSelfAssessment.Columns.Add("Option3AnswerCount");
        //            dataTable_NoneSelfAssessment.Columns.Add("Option4AnswerCount");
        //            dataTable_NoneSelfAssessment.Columns.Add("Option5AnswerCount");
        //            dataTable_NoneSelfAssessment.Columns.Add("Score");
        //            //  單選題、非自評
        //            foreach (XElement xElement in xQuestions_NoneSelfAssessment)
        //            {
        //                DataRow pRow = dataTable_NoneSelfAssessment.NewRow();

        //                pRow["No"] = xElement.Attribute("No").Value + ".";
        //                pRow["Content"] = xElement.Attribute("Content").Value;
        //                pRow["Option1AnswerCount"] = 0;
        //                pRow["Option2AnswerCount"] = 0;
        //                pRow["Option3AnswerCount"] = 0;
        //                pRow["Option4AnswerCount"] = 0;
        //                pRow["Option5AnswerCount"] = 0;
        //                pRow["Score"] = xElement.Attribute("Score").Value;
        //                IEnumerable<XElement> Options = xElement.Descendants("Option");
        //                int j = 0;
        //                foreach (XElement xOption in Options)
        //                {
        //                    j++;
        //                    pRow["Option" + j + "AnswerCount"] = xOption.Attribute("AnswerCount").Value;
        //                }
        //                dataTable_NoneSelfAssessment.Rows.Add(pRow);

        //                int rowIndex = dataTable_NoneSelfAssessment.Rows.Count - 1;
        //                ReportHelper.CellStyle cs = new ReportHelper.CellStyle(HorizontalAlignment.Center);
        //                ReportHelper.CellObject co = new CellObject(rowIndex, 7, pRow.Table.TableName, "DataSection", key);
        //                dicCellStyles.Add(co, cs);
                        
        //                cs = new ReportHelper.CellStyle("Times New Roman");
        //                co = new CellObject(rowIndex, 0, pRow.Table.TableName, "DataSection", key);
        //                dicCellStyles.Add(co, cs);

        //                if (dicQuestionBackgroundColor.ContainsKey(SurveyID))
        //                {
        //                    if (dicQuestionBackgroundColor[SurveyID].ContainsKey(xElement.Attribute("No").Value))
        //                    {
        //                        cs = new ReportHelper.CellStyle();
        //                        cs.SetFontBackGroundColor(dicQuestionBackgroundColor[SurveyID][xElement.Attribute("No").Value]);
        //                        co = new CellObject(rowIndex, 1, pRow.Table.TableName, "DataSection", key);
        //                        dicCellStyles.Add(co, cs);

        //                        cs = new ReportHelper.CellStyle();
        //                        cs.SetFontBackGroundColor(dicQuestionBackgroundColor[SurveyID][xElement.Attribute("No").Value]);
        //                        co = new CellObject(rowIndex, 2, pRow.Table.TableName, "DataSection", key);
        //                        dicCellStyles.Add(co, cs);

        //                        cs = new ReportHelper.CellStyle();
        //                        cs.SetFontBackGroundColor(dicQuestionBackgroundColor[SurveyID][xElement.Attribute("No").Value]);
        //                        co = new CellObject(rowIndex, 3, pRow.Table.TableName, "DataSection", key);
        //                        dicCellStyles.Add(co, cs);

        //                        cs = new ReportHelper.CellStyle();
        //                        cs.SetFontBackGroundColor(dicQuestionBackgroundColor[SurveyID][xElement.Attribute("No").Value]);
        //                        co = new CellObject(rowIndex, 4, pRow.Table.TableName, "DataSection", key);
        //                        dicCellStyles.Add(co, cs);

        //                        cs = new ReportHelper.CellStyle();
        //                        cs.SetFontBackGroundColor(dicQuestionBackgroundColor[SurveyID][xElement.Attribute("No").Value]);
        //                        co = new CellObject(rowIndex, 5, pRow.Table.TableName, "DataSection", key);
        //                        dicCellStyles.Add(co, cs);

        //                        cs = new ReportHelper.CellStyle();
        //                        cs.SetFontBackGroundColor(dicQuestionBackgroundColor[SurveyID][xElement.Attribute("No").Value]);
        //                        co = new CellObject(rowIndex, 6, pRow.Table.TableName, "DataSection", key);
        //                        dicCellStyles.Add(co, cs);

        //                        cs = new ReportHelper.CellStyle();
        //                        cs.SetFontBackGroundColor(dicQuestionBackgroundColor[SurveyID][xElement.Attribute("No").Value]);
        //                        co = new CellObject(rowIndex, 7, pRow.Table.TableName, "DataSection", key);
        //                        dicCellStyles.Add(co, cs);
        //                    }
        //                }
        //            }
        //            //  統計群組
        //            foreach (XElement xElement in xStatisticsGroup)
        //            {
        //                DataRow pRow = dataTable_NoneSelfAssessment.NewRow();

        //                pRow["No"] = string.Empty;
        //                pRow["Content"] = xElement.Attribute("Content").Value;
        //                pRow["Option1AnswerCount"] = string.Empty;
        //                pRow["Option2AnswerCount"] = string.Empty;
        //                pRow["Option3AnswerCount"] = string.Empty;
        //                pRow["Option4AnswerCount"] = string.Empty;
        //                pRow["Option5AnswerCount"] = string.Empty;
        //                pRow["Score"] = xElement.Attribute("Score").Value;

        //                dataTable_NoneSelfAssessment.Rows.Add(pRow);

        //                int rowIndex = dataTable_NoneSelfAssessment.Rows.Count - 1;
        //                ReportHelper.CellStyle cs = new ReportHelper.CellStyle("標楷體", true, true);
        //                ReportHelper.CellObject co = new CellObject(rowIndex, 1, pRow.Table.TableName, "DataSection", key);
        //                dicCellStyles.Add(co, cs);

        //                cs = new ReportHelper.CellStyle();
        //                cs.SetRowHeight(18);
        //                co = new CellObject(rowIndex, 0, pRow.Table.TableName, "DataSection", key);
        //                dicCellStyles.Add(co, cs);

        //                if (dicEvaluationBackgroundColor.ContainsKey(SurveyID))
        //                {
        //                    if (dicEvaluationBackgroundColor[SurveyID].ContainsKey(xElement.Attribute("Content").Value))
        //                    {
        //                        cs = new ReportHelper.CellStyle();
        //                        cs.Bold = false;
        //                        cs.Underline = true;
        //                        cs.SetFontBackGroundColor(dicEvaluationBackgroundColor[SurveyID][xElement.Attribute("Content").Value]);
        //                        co = new CellObject(rowIndex, 7, pRow.Table.TableName, "DataSection", key);
        //                        dicCellStyles.Add(co, cs);
        //                    }
        //                    else
        //                    {
        //                        cs = new ReportHelper.CellStyle(false, true);
        //                        co = new CellObject(rowIndex, 7, pRow.Table.TableName, "DataSection", key);
        //                        dicCellStyles.Add(co, cs);
        //                    }
        //                }
        //                else
        //                {
        //                    cs = new ReportHelper.CellStyle(false, true);
        //                    co = new CellObject(rowIndex, 7, pRow.Table.TableName, "DataSection", key);
        //                    dicCellStyles.Add(co, cs);
        //                }
        //            }
        //            if (xQuestions_Case.Count() > 0)
        //            {
        //                //  空一行
        //                DataRow pppppRow = dataTable_NoneSelfAssessment.NewRow();

        //                pppppRow["No"] = string.Empty;
        //                pppppRow["Content"] = string.Empty;
        //                pppppRow["Option1AnswerCount"] = string.Empty;
        //                pppppRow["Option2AnswerCount"] = string.Empty;
        //                pppppRow["Option3AnswerCount"] = string.Empty;
        //                pppppRow["Option4AnswerCount"] = string.Empty;
        //                pppppRow["Option5AnswerCount"] = string.Empty;
        //                pppppRow["Score"] = string.Empty;

        //                dataTable_NoneSelfAssessment.Rows.Add(pppppRow);
        //            }

        //            //  個案題
        //            foreach (XElement xElement in xQuestions_Case)
        //            {
        //                DataRow pRow = dataTable_NoneSelfAssessment.NewRow();

        //                pRow["No"] = xElement.Attribute("No").Value + ".";
        //                pRow["Content"] = xElement.Attribute("Content").Value;
        //                pRow["Option1AnswerCount"] = string.Empty;
        //                pRow["Option2AnswerCount"] = string.Empty;
        //                pRow["Option3AnswerCount"] = string.Empty;
        //                pRow["Option4AnswerCount"] = string.Empty;
        //                pRow["Option5AnswerCount"] = string.Empty;
        //                pRow["Score"] = string.Empty;

        //                dataTable_NoneSelfAssessment.Rows.Add(pRow);

        //                int rowIndex = dataTable_NoneSelfAssessment.Rows.Count - 1;
        //                ReportHelper.CellStyle cs = new ReportHelper.CellStyle("Times New Roman", false, false);
        //                ReportHelper.CellObject co = new CellObject(rowIndex, 0, pRow.Table.TableName, "DataSection", key);
        //                dicCellStyles.Add(co, cs);

        //                IEnumerable<XElement> xCases = xElement.Descendants("Case");
        //                int z = 0;
        //                foreach (XElement xCase in xCases)
        //                {
        //                    z++;

        //                    DataRow ppRow = dataTable_NoneSelfAssessment.NewRow();

        //                    ppRow["No"] = Utility.GetLoumaNumber(z) + ".";
        //                    ppRow["Content"] = xCase.Attribute("Content").Value;
        //                    ppRow["Option1AnswerCount"] = string.Empty;
        //                    ppRow["Option2AnswerCount"] = string.Empty;
        //                    ppRow["Option3AnswerCount"] = string.Empty;
        //                    ppRow["Option4AnswerCount"] = string.Empty;
        //                    ppRow["Option5AnswerCount"] = string.Empty;
        //                    ppRow["Score"] = xCase.Attribute("Score").Value;

        //                    IEnumerable<XElement> Options = xCase.Descendants("Option");
        //                    int j = 0;
        //                    foreach (XElement xOption in Options)
        //                    {
        //                        j++;
        //                        ppRow["Option" + j + "AnswerCount"] = xOption.Attribute("AnswerCount").Value;
        //                    }
        //                    dataTable_NoneSelfAssessment.Rows.Add(ppRow);

        //                    rowIndex = dataTable_NoneSelfAssessment.Rows.Count - 1;
        //                    cs = new ReportHelper.CellStyle(HorizontalAlignment.Center);
        //                    co = new CellObject(rowIndex, 0, pRow.Table.TableName, "DataSection", key);
        //                    dicCellStyles.Add(co, cs);

        //                    cs = new ReportHelper.CellStyle("新細明體", true, false);
        //                    co = new CellObject(rowIndex, 0, pRow.Table.TableName, "DataSection", key);
        //                    dicCellStyles.Add(co, cs);

        //                    cs = new ReportHelper.CellStyle(10, false, false);
        //                    co = new CellObject(rowIndex, 1, pRow.Table.TableName, "DataSection", key);
        //                    dicCellStyles.Add(co, cs);
        //                }
        //            }
        //            //  空一行
        //            DataRow pppRow = dataTable_NoneSelfAssessment.NewRow();

        //            pppRow["No"] = string.Empty;
        //            pppRow["Content"] = string.Empty;
        //            pppRow["Option1AnswerCount"] = string.Empty;
        //            pppRow["Option2AnswerCount"] = string.Empty;
        //            pppRow["Option3AnswerCount"] = string.Empty;
        //            pppRow["Option4AnswerCount"] = string.Empty;
        //            pppRow["Option5AnswerCount"] = string.Empty;
        //            pppRow["Score"] = string.Empty;

        //            dataTable_NoneSelfAssessment.Rows.Add(pppRow);

        //            //  學生文字意見(問答題)
        //            pppRow = dataTable_NoneSelfAssessment.NewRow();

        //            pppRow["No"] = string.Empty;
        //            pppRow["Content"] = "學生文字意見";
        //            pppRow["Option1AnswerCount"] = string.Empty;
        //            pppRow["Option2AnswerCount"] = string.Empty;
        //            pppRow["Option3AnswerCount"] = string.Empty;
        //            pppRow["Option4AnswerCount"] = string.Empty;
        //            pppRow["Option5AnswerCount"] = string.Empty;
        //            pppRow["Score"] = string.Empty;

        //            dataTable_NoneSelfAssessment.Rows.Add(pppRow);

        //            int rowIndex2 = dataTable_NoneSelfAssessment.Rows.Count - 1;
        //            ReportHelper.CellStyle cs2 = new ReportHelper.CellStyle("標楷體", true, true);
        //            ReportHelper.CellObject co2 = new CellObject(rowIndex2, 1, pppRow.Table.TableName, "DataSection", key);
        //            dicCellStyles.Add(co2, cs2);

        //            cs2 = new ReportHelper.CellStyle();
        //            cs2.SetRowHeight(18);
        //            co2 = new CellObject(rowIndex2, 0, pppRow.Table.TableName, "DataSection", key);
        //            dicCellStyles.Add(co2, cs2);

        //            foreach (XElement xElement in xQuestions_Essay)
        //            {
        //                DataRow pRow = dataTable_NoneSelfAssessment.NewRow();

        //                pRow["No"] = xElement.Attribute("No").Value + ".";
        //                pRow["Content"] = xElement.Attribute("Content").Value;
        //                pRow["Option1AnswerCount"] = string.Empty;
        //                pRow["Option2AnswerCount"] = string.Empty;
        //                pRow["Option3AnswerCount"] = string.Empty;
        //                pRow["Option4AnswerCount"] = string.Empty;
        //                pRow["Option5AnswerCount"] = string.Empty;
        //                pRow["Score"] = string.Empty;

        //                dataTable_NoneSelfAssessment.Rows.Add(pRow);

        //                int rowIndex = dataTable_NoneSelfAssessment.Rows.Count - 1;

        //                cs2 = new ReportHelper.CellStyle("標楷體", true, true);
        //                co2 = new CellObject(rowIndex, 1, pppRow.Table.TableName, "DataSection", key);
        //                dicCellStyles.Add(co2, cs2);

        //                cs2 = new ReportHelper.CellStyle();
        //                cs2.SetRowHeight(18);
        //                co2 = new CellObject(rowIndex, 0, pRow.Table.TableName, "DataSection", key);
        //                dicCellStyles.Add(co2, cs2);

        //                ReportHelper.CellStyle cs = new ReportHelper.CellStyle("Times New Roman", true, false);
        //                ReportHelper.CellObject co = new CellObject(rowIndex, 0, pRow.Table.TableName, "DataSection", key);
        //                dicCellStyles.Add(co, cs);

        //                IEnumerable<XElement> xAnswers = xElement.Descendants("Answer");
        //                int k = 0;
        //                foreach (XElement xElement_Answer in xAnswers)
        //                {
        //                    DataRow Row = dataTable_NoneSelfAssessment.NewRow();
        //                    k++;
        //                    Row["No"] = k + ".";
        //                    Row["Content"] = xElement_Answer.Value;
        //                    Row["Option1AnswerCount"] = string.Empty;
        //                    Row["Option2AnswerCount"] = string.Empty;
        //                    Row["Option3AnswerCount"] = string.Empty;
        //                    Row["Option4AnswerCount"] = string.Empty;
        //                    Row["Option5AnswerCount"] = string.Empty;
        //                    Row["Score"] = string.Empty;

        //                    dataTable_NoneSelfAssessment.Rows.Add(Row);

        //                    int rowIndex3 = dataTable_NoneSelfAssessment.Rows.Count - 1;
        //                    ReportHelper.CellStyle cs3 = new ReportHelper.CellStyle("Times New Roman", false, false);
        //                    ReportHelper.CellObject co3 = new CellObject(rowIndex3, 0, pRow.Table.TableName, "DataSection", key);
        //                    dicCellStyles.Add(co3, cs3);
        //                }

        //                //  空一行
        //                DataRow ppppRow = dataTable_NoneSelfAssessment.NewRow();

        //                ppppRow["No"] = string.Empty;
        //                ppppRow["Content"] = string.Empty;
        //                ppppRow["Option1AnswerCount"] = string.Empty;
        //                ppppRow["Option2AnswerCount"] = string.Empty;
        //                ppppRow["Option3AnswerCount"] = string.Empty;
        //                ppppRow["Option4AnswerCount"] = string.Empty;
        //                ppppRow["Option5AnswerCount"] = string.Empty;
        //                ppppRow["Score"] = string.Empty;

        //                dataTable_NoneSelfAssessment.Rows.Add(ppppRow);
        //            }      
        //            #endregion

        //            #endregion

        //            dataSet_DataSection.Tables.Add(dataTable_NoneSelfAssessment);

        //            if (!dicTeacherStatistics.ContainsKey(key))
        //                dicTeacherStatistics.Add(key, new List<DataSet>());

        //            dicTeacherStatistics[key].Add(dataSet_PageHeader);
        //            dicTeacherStatistics[key].Add(dataSet_DataSection);
        //        }

        //        MemoryStream ms = new MemoryStream(Properties.Resources.TeachingEvaluationTemplate);
        //        wb = Report.Produce(dicTeacherStatistics, ms, false, dicCellStyles);
        //        if (wb.Worksheets.Count == 0)
        //            throw new Exception("沒有資料。");
        //        else
        //        {
        //            //wb.Worksheets.Cast<Worksheet>().ToList().ForEach(y => y.AutoFitColumns());
        //            wb.Worksheets.Cast<Worksheet>().ToList().ForEach((y) =>
        //            {
        //                for (int i = 6; i <= y.Cells.MaxDataRow; i++ )
        //                {
        //                    if (y.Cells.GetRowHeight(i) == 18)
        //                        continue;

        //                    y.AutoFitRow(i);
        //                }
        //            });
        //        }
        //    });

        //    task.ContinueWith((x) =>
        //    {
        //        if (x.Exception != null)
        //        {
        //            MessageBox.Show(x.Exception.InnerException.Message);
        //            goto TheEnd;
        //        }
        //        SaveFileDialog sd = new SaveFileDialog();
        //        sd.Title = "另存新檔";
        //        sd.FileName = string.Format("{0}-{1}~{2}-{3}-授課教師教學評鑑統計表.xls", school_year, semester, school_year1, semester1);
        //        sd.Filter = "Excel 2003 相容檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
        //        if (sd.ShowDialog() == DialogResult.OK)
        //        {
        //            try
        //            {
        //                wb.Save(sd.FileName, FileFormatType.Excel2003);
        //                System.Diagnostics.Process.Start(sd.FileName);
        //            }
        //            catch
        //            {
        //                MessageBox.Show("指定路徑無法存取。", "建立檔案失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }
        //        }

        //        TheEnd:
        //        this.circularProgress.IsRunning = false;
        //        this.circularProgress.Visible = false;
        //        this.Export.Enabled = true;
        //    }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        //}
    }
}

//<Statistics CSAttendCount='62' FeedBackCount='3' TeacherName='林嬋娟' CourseName='管理會計 03' SubjectName='管理會計' SubjectCode='740 M3010' NewSubjectCode='EMBA7002' SchoolYear='101' Semester='0'>
//    <Question No='1' Content='本門課我的出席狀況是' Type='單選題' IsSelfAssessment='是' IsCase='否' Score='1.67'>
//        <Option No='1' Content='從不缺課' AnswerCount='1' />
//        <Option No='2' Content='缺課1~2節' AnswerCount='2' />
//        <Option No='3' Content='缺課3~4節' AnswerCount='0' />
//    </Question>
//    <Question No='2' Content='我認為本課程我可以獲得的成績範圍為' Type='單選題' IsSelfAssessment='是' IsCase='否' Score='2.67'>
//        <Option No='1' Content='0~59分' AnswerCount='0' />
//        <Option No='2' Content='60~69分' AnswerCount='2' />
//        <Option No='3' Content='70~79分' AnswerCount='0' />
//        <Option No='4' Content='80~89分' AnswerCount='1' />
//        <Option No='5' Content='90~100分' AnswerCount='0' />
//    </Question>
//    <Question No='1' Content='本課程的內容和學習目標十分明確。' Type='單選題' IsSelfAssessment='否' IsCase='否' Score='2.67'>
//        <Option No='1' Content='非常不同意' AnswerCount='0' />
//        <Option No='2' Content='不同意' AnswerCount='2' />
//        <Option No='3' Content='普通' AnswerCount='0' />
//        <Option No='4' Content='同意' AnswerCount='1' />
//        <Option No='5' Content='非常同意' AnswerCount='0' />
//    </Question>
//    <Question No='2' Content='本課程上課內容充實、且符合教學大綱。' Type='單選題' IsSelfAssessment='否' IsCase='否' Score='2.67'>
//        <Option No='1' Content='非常不同意' AnswerCount='0' />
//        <Option No='2' Content='不同意' AnswerCount='2' />
//        <Option No='3' Content='普通' AnswerCount='0' />
//        <Option No='4' Content='同意' AnswerCount='1' />
//        <Option No='5' Content='非常同意' AnswerCount='0' />
//    </Question>
//    <Question No='3' Content='本課程的主要及輔助教材內容難易適中。' Type='單選題' IsSelfAssessment='否' IsCase='否' Score='4'>
//        <Option No='1' Content='非常不同意' AnswerCount='0' />
//        <Option No='2' Content='不同意' AnswerCount='0' />
//        <Option No='3' Content='普通' AnswerCount='0' />
//        <Option No='4' Content='同意' AnswerCount='1' />
//        <Option No='5' Content='非常同意' AnswerCount='0' />
//    </Question>
//    <Question No='4' Content='本課程的理論說明與實務運用比例適當。' Type='單選題' IsSelfAssessment='否' IsCase='否' Score='4'>
//        <Option No='1' Content='非常不同意' AnswerCount='0' />
//        <Option No='2' Content='不同意' AnswerCount='0' />
//        <Option No='3' Content='普通' AnswerCount='0' />
//        <Option No='4' Content='同意' AnswerCount='1' />
//        <Option No='5' Content='非常同意' AnswerCount='0' />
//    </Question>
//    <Question No='5' Content='本課程使用的個案數量適當。' Type='單選題' IsSelfAssessment='否' IsCase='否' Score='4'>
//        <Option No='1' Content='非常不同意' AnswerCount='0' />
//        <Option No='2' Content='不同意' AnswerCount='0' />
//        <Option No='3' Content='普通' AnswerCount='0' />
//        <Option No='4' Content='同意' AnswerCount='1' />
//        <Option No='5' Content='非常同意' AnswerCount='0' />
//    </Question>
//    <Question No='6' Content='個案的選擇契合單元主題，並達成預定的學習目標。' Type='單選題' IsSelfAssessment='否' IsCase='否' Score='4'>
//        <Option No='1' Content='非常不同意' AnswerCount='0' />
//        <Option No='2' Content='不同意' AnswerCount='0' />
//        <Option No='3' Content='普通' AnswerCount='0' />
//        <Option No='4' Content='同意' AnswerCount='1' />
//        <Option No='5' Content='非常同意' AnswerCount='0' />
//    </Question>
//    <Question No='7' Content='個案內容具有高度的實務攸關性。' Type='單選題' IsSelfAssessment='否' IsCase='否' Score='4'>
//        <Option No='1' Content='非常不同意' AnswerCount='0' />
//        <Option No='2' Content='不同意' AnswerCount='0' />
//        <Option No='3' Content='普通' AnswerCount='0' />
//        <Option No='4' Content='同意' AnswerCount='1' />
//        <Option No='5' Content='非常同意' AnswerCount='0' />
//    </Question>
//    <Question No='8' Content='綜合而言，本課程內容設計(含個案)有助於我的學習。' Type='單選題' IsSelfAssessment='否' IsCase='否' Score='5'>
//        <Option No='1' Content='非常不同意' AnswerCount='0' />
//        <Option No='2' Content='不同意' AnswerCount='0' />
//        <Option No='3' Content='普通' AnswerCount='0' />
//        <Option No='4' Content='同意' AnswerCount='0' />
//        <Option No='5' Content='非常同意' AnswerCount='1' />
//    </Question>
//    <Question No='9' Content='老師對於各個單元的進度控制恰當。' Type='單選題' IsSelfAssessment='否' IsCase='否' Score='4'>
//        <Option No='1' Content='非常不同意' AnswerCount='0' />
//        <Option No='2' Content='不同意' AnswerCount='0' />
//        <Option No='3' Content='普通' AnswerCount='0' />
//        <Option No='4' Content='同意' AnswerCount='1' />
//        <Option No='5' Content='非常同意' AnswerCount='0' />
//    </Question>
//    <Question No='10' Content='老師採用的教學方法能有效地提升學習效果。' Type='單選題' IsSelfAssessment='否' IsCase='否' Score='4'>
//        <Option No='1' Content='非常不同意' AnswerCount='0' />
//        <Option No='2' Content='不同意' AnswerCount='0' />
//        <Option No='3' Content='普通' AnswerCount='0' />
//        <Option No='4' Content='同意' AnswerCount='1' />
//        <Option No='5' Content='非常同意' AnswerCount='0' />
//    </Question>
//    <Question No='11' Content='老師的解說清楚而詳盡，有助於提升學習效果。' Type='單選題' IsSelfAssessment='否' IsCase='否' Score='4'>
//        <Option No='1' Content='非常不同意' AnswerCount='0' />
//        <Option No='2' Content='不同意' AnswerCount='0' />
//        <Option No='3' Content='普通' AnswerCount='0' />
//        <Option No='4' Content='同意' AnswerCount='1' />
//        <Option No='5' Content='非常同意' AnswerCount='0' />
//    </Question>
//    <Question No='12' Content='老師能有效地引導學生發問及表達意見，有助於學習與思考。' Type='單選題' IsSelfAssessment='否' IsCase='否' Score='4'>
//        <Option No='1' Content='非常不同意' AnswerCount='0' />
//        <Option No='2' Content='不同意' AnswerCount='0' />
//        <Option No='3' Content='普通' AnswerCount='0' />
//        <Option No='4' Content='同意' AnswerCount='1' />
//        <Option No='5' Content='非常同意' AnswerCount='0' />
//    </Question>
//    <Question No='13' Content='老師能及時地批改回覆作業，並明確地給予評論或建議。' Type='單選題' IsSelfAssessment='否' IsCase='否' Score='4'>
//        <Option No='1' Content='非常不同意' AnswerCount='0' />
//        <Option No='2' Content='不同意' AnswerCount='0' />
//        <Option No='3' Content='普通' AnswerCount='0' />
//        <Option No='4' Content='同意' AnswerCount='1' />
//        <Option No='5' Content='非常同意' AnswerCount='0' />
//    </Question>
//    <Question No='14' Content='綜合而言，本課程的老師非常稱職。' Type='單選題' IsSelfAssessment='否' IsCase='否' Score='5'>
//        <Option No='1' Content='非常不同意' AnswerCount='0' />
//        <Option No='2' Content='不同意' AnswerCount='0' />
//        <Option No='3' Content='普通' AnswerCount='0' />
//        <Option No='4' Content='同意' AnswerCount='0' />
//        <Option No='5' Content='非常同意' AnswerCount='1' />
//    </Question>
//    <Question No='15' Content='課程參與的評分準則，能有效鼓勵學生參與討論。' Type='單選題' IsSelfAssessment='否' IsCase='否' Score='4'>
//        <Option No='1' Content='非常不同意' AnswerCount='0' />
//        <Option No='2' Content='不同意' AnswerCount='0' />
//        <Option No='3' Content='普通' AnswerCount='0' />
//        <Option No='4' Content='同意' AnswerCount='1' />
//        <Option No='5' Content='非常同意' AnswerCount='0' />
//    </Question>
//    <Question No='16' Content='繳交作業或報告有助於我的學習。' Type='單選題' IsSelfAssessment='否' IsCase='否' Score='4'>
//        <Option No='1' Content='非常不同意' AnswerCount='0' />
//        <Option No='2' Content='不同意' AnswerCount='0' />
//        <Option No='3' Content='普通' AnswerCount='0' />
//        <Option No='4' Content='同意' AnswerCount='1' />
//        <Option No='5' Content='非常同意' AnswerCount='0' />
//    </Question>
//    <Question No='17' Content='與小組其他成員的互動討論能有效地提升學習效果。' Type='單選題' IsSelfAssessment='否' IsCase='否' Score='4'>
//        <Option No='1' Content='非常不同意' AnswerCount='0' />
//        <Option No='2' Content='不同意' AnswerCount='0' />
//        <Option No='3' Content='普通' AnswerCount='0' />
//        <Option No='4' Content='同意' AnswerCount='1' />
//        <Option No='5' Content='非常同意' AnswerCount='0' />
//    </Question>
//    <Question No='18' Content='教學助教對我的學習很有幫助。' Type='單選題' IsSelfAssessment='否' IsCase='否' Score='4'>
//        <Option No='1' Content='非常不同意' AnswerCount='0' />
//        <Option No='2' Content='不同意' AnswerCount='0' />
//        <Option No='3' Content='普通' AnswerCount='0' />
//        <Option No='4' Content='同意' AnswerCount='1' />
//        <Option No='5' Content='非常同意' AnswerCount='0' />
//    </Question>
//    <Question No='19' Content='整體來說，本課程的教學與學習效果非常好。' Type='單選題' IsSelfAssessment='否' IsCase='否' Score='5'>
//        <Option No='1' Content='非常不同意' AnswerCount='0' />
//        <Option No='2' Content='不同意' AnswerCount='0' />
//        <Option No='3' Content='普通' AnswerCount='0' />
//        <Option No='4' Content='同意' AnswerCount='0' />
//        <Option No='5' Content='非常同意' AnswerCount='1' />
//    </Question>
//    <Question No='20' Content='你覺得下列個案是否有幫助？' Type='單選題' IsSelfAssessment='否' IsCase='是'>
//        <Case ID='96058' Content='Case Two' Score='4'>
//            <Option No='1' Content='非常不同意' AnswerCount='0' />
//            <Option No='2' Content='不同意' AnswerCount='0' />
//            <Option No='3' Content='普通' AnswerCount='0' />
//            <Option No='4' Content='同意' AnswerCount='2' />
//            <Option No='5' Content='非常同意' AnswerCount='0' />
//        </Case>
//    </Question>
//    <Question No='21' Content='本課程給您最大的收獲是什麼？' Type='問答題' IsSelfAssessment='否' IsCase='否'>
//      <Answers>
//          <Answer>一無所獲</Answer>
//          <Answer>獲益匪淺</Answer>
//      </Answers>
//    </Question>
//    <Question No='22' Content='請您提供本門課程的優缺點(包括教學設備或環境等方面)。' Type='問答題' IsSelfAssessment='否' IsCase='否'></Question>
//    <Question No='23' Content='其他想傳達給授課老師有關課程後續進行的意見。' Type='問答題' IsSelfAssessment='否' IsCase='否'></Question>
//    <StatisticsGroup Content='第1~19題平均平鑑值' Score='4.02' />
//    <StatisticsGroup Content='第14、19題平均評鑑值' Score='5' />
//</Statistics>