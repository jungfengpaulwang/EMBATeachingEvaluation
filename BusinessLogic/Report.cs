using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data;
using System.Drawing;
using Aspose.Cells;
using ReportHelper;
using System.Threading.Tasks;
using FISCA.UDT;
using System.Windows.Forms;
using System.IO;

namespace TeachingEvaluation.BusinessLogic
{
//    public class Report
//    {
//        public static Workbook GetEvaluationReport_V_2013(int print_code, UDT.TeacherStatistics Statistic, Dictionary<string, Dictionary<string, Color>> dicQuestionBackgroundColor, Dictionary<string, Dictionary<string, Color>> dicEvaluationBackgroundColor, dynamic Parameter)
//        {
//            Workbook wb = new Workbook();
//            Dictionary<ReportHelper.CellObject, ReportHelper.CellStyle> dicCellStyles = new Dictionary<CellObject, CellStyle>();                    
//            XDocument xDocument = XDocument.Parse(Statistic.StatisticsList, LoadOptions.None);

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
//                        cs.SetBackGroundColor(dicQuestionBackgroundColor[SurveyID][xElement.Attribute("No").Value]);
//                        co = new CellObject(rowIndex, 1, pRow.Table.TableName, "DataSection", key);
//                        dicCellStyles.Add(co, cs);

//                        cs = new ReportHelper.CellStyle();
//                        cs.SetBackGroundColor(dicQuestionBackgroundColor[SurveyID][xElement.Attribute("No").Value]);
//                        co = new CellObject(rowIndex, 2, pRow.Table.TableName, "DataSection", key);
//                        dicCellStyles.Add(co, cs);

//                        cs = new ReportHelper.CellStyle();
//                        cs.SetBackGroundColor(dicQuestionBackgroundColor[SurveyID][xElement.Attribute("No").Value]);
//                        co = new CellObject(rowIndex, 3, pRow.Table.TableName, "DataSection", key);
//                        dicCellStyles.Add(co, cs);

//                        cs = new ReportHelper.CellStyle();
//                        cs.SetBackGroundColor(dicQuestionBackgroundColor[SurveyID][xElement.Attribute("No").Value]);
//                        co = new CellObject(rowIndex, 4, pRow.Table.TableName, "DataSection", key);
//                        dicCellStyles.Add(co, cs);

//                        cs = new ReportHelper.CellStyle();
//                        cs.SetBackGroundColor(dicQuestionBackgroundColor[SurveyID][xElement.Attribute("No").Value]);
//                        co = new CellObject(rowIndex, 5, pRow.Table.TableName, "DataSection", key);
//                        dicCellStyles.Add(co, cs);

//                        cs = new ReportHelper.CellStyle();
//                        cs.SetBackGroundColor(dicQuestionBackgroundColor[SurveyID][xElement.Attribute("No").Value]);
//                        co = new CellObject(rowIndex, 6, pRow.Table.TableName, "DataSection", key);
//                        dicCellStyles.Add(co, cs);

//                        cs = new ReportHelper.CellStyle();
//                        cs.SetBackGroundColor(dicQuestionBackgroundColor[SurveyID][xElement.Attribute("No").Value]);
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
//                        cs.SetBackGroundColor(dicEvaluationBackgroundColor[SurveyID][xElement.Attribute("Content").Value]);
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

//                    dataSet_DataSection.Tables.Add(dataTable_NoneSelfAssessment);

//                    if (!dicTeacherStatistics.ContainsKey(key))
//                        dicTeacherStatistics.Add(key, new List<DataSet>());

//                    dicTeacherStatistics[key].Add(dataSet_PageHeader);
//                    dicTeacherStatistics[key].Add(dataSet_DataSection);
//                }

//                MemoryStream ms = new MemoryStream(Properties.Resources.TeachingEvaluationTemplate);
//                wb = ReportHelper.Report.Produce(dicTeacherStatistics, ms, false, dicCellStyles);
//                if (wb.Worksheets.Count == 0)
//                    throw new Exception("沒有資料。");
//                else
//                {
//                    //wb.Worksheets.Cast<Worksheet>().ToList().ForEach(y => y.AutoFitColumns());
//                    wb.Worksheets.Cast<Worksheet>().ToList().ForEach((y) =>
//                    {
//                        for (int i = 6; i <= y.Cells.MaxDataRow; i++)
//                        {
//                            if (y.Cells.GetRowHeight(i) == 18)
//                                continue;

//                            y.AutoFitRow(i);
//                        }
//                    });
//                }
//                return wb;
//        }
//    }
}
