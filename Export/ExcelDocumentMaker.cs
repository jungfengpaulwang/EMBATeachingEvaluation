using Aspose.Cells;
using FISCA.Data;
using FISCA.UDT;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using ReportHelper;
using System.Drawing;

namespace TeachingEvaluation.Export
{
    class ExcelDocumentMaker
    {
        private AccessHelper Access;
        private QueryHelper Query;
        private List<UDT.QHRelation> QHRelations;
        List<UDT.Hierarchy> Hierarchies;

        private UDT.TeacherStatistics _Statistics;
        private List<DataBindedSheet> _DataBindedSheets;
        private Workbook _Template;

        private Dictionary<string, Dictionary<string, Color>> _DicEvaluationBackgroundColor;
        private Dictionary<string, Dictionary<string, Color>> _DicQuestionBackgroundColor;
        private Dictionary<CellObject, CellStyle> dicCellStyles;
        private string WorksheetName;

        public string SubjectID { get; set; }
        public string CourseID { get; set; }
        public string TeacherID { get; set; }
        public string SurveyID { get; set; }
        public string SubjectCode { get; set; }
        public string NewSubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public string SchoolYear { get; set; }
        public string Semester { get; set; }

        public ExcelDocumentMaker(Dictionary<string, Dictionary<string, Color>> dicEvaluationBackgroundColor, Dictionary<string, Dictionary<string, Color>> dicQuestionBackgroundColor, List<UDT.QHRelation> QHRelations, List<UDT.Hierarchy> Hierarchies, UDT.TeacherStatistics Statistics, Workbook wb)
        {
            this.Access = new AccessHelper();
            this.Query = new QueryHelper();

            this.QHRelations = QHRelations;
            this.Hierarchies = Hierarchies;
            this._Statistics = Statistics;
            this._Template = wb;
            this._DataBindedSheets = new List<DataBindedSheet>();
            this._DicEvaluationBackgroundColor = dicEvaluationBackgroundColor;
            this._DicQuestionBackgroundColor = dicQuestionBackgroundColor;
            this.dicCellStyles = new Dictionary<CellObject, CellStyle>();
        }
        
        //  依題目群組重新組合題目
        private Dictionary<UDT.Hierarchy, List<XElement>> GetHierarchyQuestions(UDT.TeacherStatistics Statistic)
        {
            Dictionary<string, UDT.Hierarchy> dicHierarchies = new Dictionary<string, UDT.Hierarchy>();
            if (Hierarchies.Count > 0)
                dicHierarchies = Hierarchies.ToDictionary(x => x.Title);
            Dictionary<string, UDT.Hierarchy> dicQuestionHierarchies = new Dictionary<string, UDT.Hierarchy>();
            foreach (UDT.QHRelation QHRelation in QHRelations)
            {
                if (dicHierarchies.ContainsKey(QHRelation.HierarchyTitle))
                    dicQuestionHierarchies.Add(QHRelation.QuestionID.ToString(), dicHierarchies[QHRelation.HierarchyTitle]);
            }
            Dictionary<UDT.Hierarchy, List<XElement>> dicHierarchyQuestions = new Dictionary<UDT.Hierarchy, List<XElement>>();
            XDocument xDocument = XDocument.Parse(Statistic.StatisticsList, LoadOptions.None);

            //  將所有題目加入「標題」群組
            foreach (XElement xElement in xDocument.Descendants("Question"))
            {
                string question_id = xElement.Attribute("ID").Value;
                if (dicQuestionHierarchies.ContainsKey(question_id))
                {
                    UDT.Hierarchy Hierarchy = dicQuestionHierarchies[question_id];
                    if (!dicHierarchyQuestions.ContainsKey(Hierarchy))
                        dicHierarchyQuestions.Add(Hierarchy, new List<XElement>());

                    dicHierarchyQuestions[Hierarchy].Add(xElement);
                }
            }

            //  依照標題之顯示順序及題號排序
            Dictionary<UDT.Hierarchy, List<XElement>> dicOrderedHierarchyQuestions = new Dictionary<UDT.Hierarchy, List<XElement>>();
            foreach (KeyValuePair<UDT.Hierarchy, List<XElement>> kv in dicHierarchyQuestions.OrderBy(x => x.Key.DisplayOrder))
            {
                if (!dicOrderedHierarchyQuestions.ContainsKey(kv.Key))
                    dicOrderedHierarchyQuestions.Add(kv.Key, new List<XElement>());

                dicOrderedHierarchyQuestions[kv.Key].AddRange(kv.Value.OrderBy(x => int.Parse(x.Attribute("No").Value)));
            }

            return dicOrderedHierarchyQuestions;
        }
        
        //  報表標題        
        private DataBindedSheet GetReportHeader(XElement xStatistics, Workbook wb)
        {
            string CSAttendCount = xStatistics.Attribute("CSAttendCount").Value;    //  修課人數
            string FeedBackCount = xStatistics.Attribute("FeedBackCount").Value;    //  填答人數
            string TeacherName = HttpUtility.HtmlDecode(xStatistics.Attribute("TeacherName").Value);            //  授課教師
            string CourseName = HttpUtility.HtmlDecode(xStatistics.Attribute("CourseName").Value);              //  開課
            string SubjectName = HttpUtility.HtmlDecode(xStatistics.Attribute("SubjectName").Value);            //  課程
            string ClassName = xStatistics.Attribute("ClassName").Value;                    //  班次
            string SubjectCode = xStatistics.Attribute("SubjectCode").Value;                //  課程識別碼
            string NewSubjectCode = xStatistics.Attribute("NewSubjectCode").Value;  //  課號
            string SchoolYear = xStatistics.Attribute("SchoolYear").Value;                      //  學年度
            string Semester = xStatistics.Attribute("Semester").Value;                              //  學期
            string CourseID = xStatistics.Attribute("CourseID").Value;                              //  開課系統編號
            string TeacherID = xStatistics.Attribute("TeacherID").Value;                            //  授課教師系統編號
            string SurveyDate = xStatistics.Attribute("SurveyDate").Value;                      //  問卷調查日期
            string SurveyID = xStatistics.Attribute("SurveyID").Value;                              //  問卷系統編號
            string SubjectID = xStatistics.Attribute("SubjectID").Value;

            this.SubjectID = SubjectID;
            this.CourseID = CourseID;
            this.TeacherID = TeacherID;
            this.SurveyID = SurveyID;
            this.SubjectCode = SubjectCode;
            this.NewSubjectCode = NewSubjectCode;
            this.SubjectName = SubjectName;
            this.CourseName = CourseName;
            this.TeacherName = TeacherName;
            this.SchoolYear = SchoolYear;
            this.Semester = Semester;

            DataBindedSheet DataBindedSheet = new DataBindedSheet();

            DataBindedSheet.Worksheet = wb.Worksheets["報表標題"];
            DataBindedSheet.DataTables = new List<DataTable>();
            DataBindedSheet.DataTables.Add(SchoolYear.ToDataTable("學年度", "學年度"));
            DataBindedSheet.DataTables.Add(DataItems.SemesterItem.GetSemesterByCode(xStatistics.Attribute("Semester").Value).Name.ToDataTable("學期", "學期"));
            DataBindedSheet.DataTables.Add(SubjectName.ToDataTable("科目名稱", "科目名稱"));
            DataBindedSheet.DataTables.Add(NewSubjectCode.ToDataTable("課號", "課號"));
            DataBindedSheet.DataTables.Add(ClassName.ToDataTable("班次", "班次"));
            DataBindedSheet.DataTables.Add(TeacherName.ToDataTable("任課教師", "任課教師"));
            DataBindedSheet.DataTables.Add(CSAttendCount.ToDataTable("修課人數", "修課人數"));
            DataBindedSheet.DataTables.Add(FeedBackCount.ToDataTable("填答人數", "填答人數"));
            DataBindedSheet.DataTables.Add(SurveyDate.ToDataTable("問卷調查日期", "問卷調查日期"));

            this.WorksheetName = string.Format("{0}-{1}-{2}-{3}", this.SchoolYear, this.Semester, this.CourseName, this.TeacherName);

            return DataBindedSheet; 
        }

        //  群組標題
        private DataBindedSheet GetGroupHeader(int DisplayOrder, UDT.Hierarchy Hierarchy, Workbook wb)
        {
            DataBindedSheet DataBindedSheet = new DataBindedSheet();

            DataBindedSheet.Worksheet = wb.Worksheets[Hierarchy.Title + "-標題"];
            DataBindedSheet.DataTables = new List<DataTable>();
            DataBindedSheet.DataTables.Add((ToChineseNo(DisplayOrder)).ToDataTable("項次", "項次"));

            return DataBindedSheet;
        }

        //  選擇題-題目
        private DataBindedSheet GetQuestion(XElement xElement, UDT.Hierarchy Hierarchy, Workbook wb)
        {
            DataBindedSheet DataBindedSheet = new DataBindedSheet();

            DataBindedSheet.Worksheet = wb.Worksheets[Hierarchy.Title + "-選擇題-題目"];
            DataBindedSheet.DataTables = new List<DataTable>();
            //  項次
            DataBindedSheet.DataTables.Add((xElement.Attribute("No").Value + ".").ToDataTable("項次", "項次"));
            //  題目
            DataBindedSheet.DataTables.Add(HttpUtility.HtmlDecode(xElement.Attribute("Content").Value).ToDataTable("題目", "題目"));
            //  題目高度
            CellObject co = new CellObject(0, 0, "題目", DataBindedSheet.Key, this.WorksheetName);
            CellStyle cs = new CellStyle();
            cs.SetAutoFitRow(true).Merge(1, 1);
            //  題目背景色
            if (this._DicQuestionBackgroundColor.ContainsKey(SurveyID))
            {
                if (this._DicQuestionBackgroundColor[SurveyID].ContainsKey(xElement.Attribute("No").Value))
                {
                    cs.SetFontBackGroundColor(this._DicQuestionBackgroundColor[SurveyID][xElement.Attribute("No").Value]);
                    this.dicCellStyles.Add(co, cs);
                    goto Outline;
                }
            }
            this.dicCellStyles.Add(co, cs);
            Outline:

            IEnumerable<XElement> Options = xElement.Descendants("Option");
            if (Options.Count() > 0) Options = Options.OrderBy(x => int.Parse(x.Attribute("No").Value));
                    
            string question_content = string.Empty;
            int i = 0;
            foreach (XElement xOption in Options)
            {
                i = i + 1;
                //  選項做答人數
                DataBindedSheet.DataTables.Add(xOption.Attribute("AnswerCount").Value.ToDataTable("做答人數-" + xOption.Attribute("No").Value, "做答人數-" + xOption.Attribute("No").Value));
                question_content += "(" + xOption.Attribute("No").Value + ")" + HttpUtility.HtmlDecode(xOption.Attribute("Content").Value);

                //  做答背景色
                co = new CellObject(0, 0, "做答人數-" + xOption.Attribute("No").Value, DataBindedSheet.Key, this.WorksheetName);
                cs = new CellStyle();
                if (this._DicQuestionBackgroundColor.ContainsKey(SurveyID))
                {
                    if (this._DicQuestionBackgroundColor[SurveyID].ContainsKey(xElement.Attribute("No").Value))
                    {
                        cs.SetFontBackGroundColor(this._DicQuestionBackgroundColor[SurveyID][xElement.Attribute("No").Value]);
                        this.dicCellStyles.Add(co, cs);
                    }
                }
            }
            for (int j = i; j < 6; j++)
            {
                //  補選項6背景色
                DataBindedSheet.DataTables.Add("".ToDataTable("做答人數-" + (j + 1), "做答人數-" + (j + 1)));
                co = new CellObject(0, 0, "做答人數-" + (j+1), DataBindedSheet.Key, this.WorksheetName);
                cs = new CellStyle();
                if (this._DicQuestionBackgroundColor.ContainsKey(SurveyID))
                {
                    if (this._DicQuestionBackgroundColor[SurveyID].ContainsKey(xElement.Attribute("No").Value))
                    {
                        cs.SetFontBackGroundColor(this._DicQuestionBackgroundColor[SurveyID][xElement.Attribute("No").Value]);
                        this.dicCellStyles.Add(co, cs);
                    }
                }

            }
            //  題目+選項
            DataBindedSheet.DataTables.Add(HttpUtility.HtmlDecode(xElement.Attribute("Content").Value + question_content).ToDataTable("題目+選項", "題目+選項"));
            //  評鑑值
            DataBindedSheet.DataTables.Add(xElement.Attribute("Score").Value.ToDataTable("評鑑值", "評鑑值"));
            //  評鑑值背景色
            co = new CellObject(0, 0, "評鑑值", DataBindedSheet.Key, this.WorksheetName);
            cs = new CellStyle();
            if (this._DicQuestionBackgroundColor.ContainsKey(SurveyID))
            {
                if (this._DicQuestionBackgroundColor[SurveyID].ContainsKey(xElement.Attribute("No").Value))
                {
                    cs.SetFontBackGroundColor(this._DicQuestionBackgroundColor[SurveyID][xElement.Attribute("No").Value]);
                    this.dicCellStyles.Add(co, cs);
                }
            }

            return DataBindedSheet;
        }

        //  個案題-題目
        private DataBindedSheet GetCaseQuestion(string DisplayOrder, XElement xElement, UDT.Hierarchy Hierarchy, Workbook wb)
        {
            DataBindedSheet DataBindedSheet = new DataBindedSheet();

            DataBindedSheet.Worksheet = wb.Worksheets[Hierarchy.Title + "-個案題-題目"];
            DataBindedSheet.DataTables = new List<DataTable>();
            //  項次
            DataBindedSheet.DataTables.Add((DisplayOrder + ".").ToDataTable("項次", "項次"));
            //  題目
            DataBindedSheet.DataTables.Add(HttpUtility.HtmlDecode(xElement.Attribute("Content").Value).ToDataTable("題目", "題目"));
            //  題目高度
            CellObject co = new CellObject(0, 0, "題目", DataBindedSheet.Key, this.WorksheetName);
            CellStyle cs = new CellStyle();
            cs.SetAutoFitRow(true).Merge(1, 1);
            this.dicCellStyles.Add(co, cs);
            return DataBindedSheet;
        }

        //  個案題-做答
        private DataBindedSheet GetCaseAnswer(string DisplayOrder, XElement xCase, UDT.Hierarchy Hierarchy, Workbook wb)
        {
            DataBindedSheet DataBindedSheet = new DataBindedSheet();

            DataBindedSheet.Worksheet = wb.Worksheets[Hierarchy.Title + "-個案題-做答"];
            DataBindedSheet.DataTables = new List<DataTable>();

            //  項次
            DataBindedSheet.DataTables.Add(DisplayOrder.ToDataTable("項次", "項次"));
            //  題目
            DataBindedSheet.DataTables.Add(HttpUtility.HtmlDecode(xCase.Attribute("Content").Value).ToDataTable("題目", "題目"));
            //  題目高度
            CellObject co = new CellObject(0, 0, "題目", DataBindedSheet.Key, this.WorksheetName);
            CellStyle cs = new CellStyle();
            cs.SetAutoFitRow(true).Merge(1, 1);
            this.dicCellStyles.Add(co, cs);
            //  評鑑值
            DataBindedSheet.DataTables.Add(xCase.Attribute("Score").Value.ToDataTable("評鑑值", "評鑑值"));

            IEnumerable<XElement> Options = xCase.Descendants("Option");
            if (Options.Count() > 0) Options = Options.OrderBy(x => int.Parse(x.Attribute("No").Value));
                    
            string question_content = string.Empty;
            foreach (XElement xOption in Options)
            {
                //  選項做答人數
                DataBindedSheet.DataTables.Add(xOption.Attribute("AnswerCount").Value.ToDataTable("做答人數-" + xOption.Attribute("No").Value, "做答人數-" + xOption.Attribute("No").Value));
                question_content += "(" + xOption.Attribute("No").Value + ")" + HttpUtility.HtmlDecode(xOption.Attribute("Content").Value);
            }
            //  題目+選項
            DataBindedSheet.DataTables.Add(HttpUtility.HtmlDecode(xCase.Attribute("Content").Value + question_content).ToDataTable("題目+選項", "題目+選項"));
            //  題目高度
            co = new CellObject(0, 0, "題目+選項", DataBindedSheet.Key, this.WorksheetName);
            cs = new CellStyle();
            cs.SetAutoFitRow(true).Merge(1, 1);
            this.dicCellStyles.Add(co, cs);
            return DataBindedSheet;
        }

        //  平均評鑑值
        private DataBindedSheet GetAvgScore(XElement xElement, UDT.Hierarchy Hierarchy, Workbook wb)
        {
            DataBindedSheet DataBindedSheet = new DataBindedSheet();

            DataBindedSheet.Worksheet = wb.Worksheets[Hierarchy.Title + "-平均評鑑值"];
            DataBindedSheet.DataTables = new List<DataTable>();
            DataBindedSheet.DataTables.Add(HttpUtility.HtmlDecode(xElement.Attribute("Content").Value).ToDataTable("群組名稱", "群組名稱"));
            DataBindedSheet.DataTables.Add(xElement.Attribute("Score").Value.ToDataTable("評鑑值", "評鑑值"));

            //  評鑑值背景色
            if (this._DicEvaluationBackgroundColor.ContainsKey(this.SurveyID))
            {
                if (this._DicEvaluationBackgroundColor[this.SurveyID].ContainsKey(HttpUtility.HtmlDecode(xElement.Attribute("Content").Value)))
                {
                    CellObject co = new CellObject(0, 0, "評鑑值", DataBindedSheet.Key, this.WorksheetName);
                    CellStyle cs = new CellStyle();
                    cs.SetFontBackGroundColor(this._DicEvaluationBackgroundColor[this.SurveyID][HttpUtility.HtmlDecode(xElement.Attribute("Content").Value)]);
                    dicCellStyles.Add(co, cs);
                }
            }
            
            return DataBindedSheet;
        }

        //  問答題-題目
        private DataBindedSheet GetEssayQuestion(XElement xElement, UDT.Hierarchy Hierarchy, Workbook wb)
        {
            DataBindedSheet DataBindedSheet = new DataBindedSheet();

            DataBindedSheet.Worksheet = wb.Worksheets[Hierarchy.Title + "-問答題-題目"];
            DataBindedSheet.DataTables = new List<DataTable>();
            //  項次
            DataBindedSheet.DataTables.Add((xElement.Attribute("No").Value + ".").ToDataTable("項次", "項次"));
            //  題目
            DataBindedSheet.DataTables.Add(HttpUtility.HtmlDecode(xElement.Attribute("Content").Value).ToDataTable("題目", "題目"));
            //  題目高度
            CellObject co = new CellObject(0, 0, "題目", DataBindedSheet.Key, this.WorksheetName);
            CellStyle cs = new CellStyle();
            cs.SetAutoFitRow(true).Merge(1, 1);
            this.dicCellStyles.Add(co, cs);

            return DataBindedSheet;
        }

        //  問答題-做答
        private DataBindedSheet GetEssayAnswer(string DisplayOrder, XElement xEssay, UDT.Hierarchy Hierarchy, Workbook wb)
        {
            DataBindedSheet DataBindedSheet = new DataBindedSheet();

            DataBindedSheet.Worksheet = wb.Worksheets[Hierarchy.Title + "-問答題-做答"];
            DataBindedSheet.DataTables = new List<DataTable>();

            //  項次
            DataBindedSheet.DataTables.Add(DisplayOrder.ToDataTable("項次", "項次"));
            //  做答
            DataBindedSheet.DataTables.Add(xEssay.Value.ToDataTable("內容", "內容"));
            //  題目高度
            CellObject co = new CellObject(0, 0, "內容", DataBindedSheet.Key, this.WorksheetName);
            CellStyle cs = new CellStyle();
            cs.SetAutoFitRow(true).Merge(1, 1);
            this.dicCellStyles.Add(co, cs);

            return DataBindedSheet;
        }
        
        /// <summary>
        /// CourseID-TeacherID, Workbook
        /// </summary>
        /// <returns>Dictionary<string, Workbook></returns>
        public Workbook Produce()
        {
            List<string> Worksheets = new List<string>();
            this._Template.Worksheets.Cast<Worksheet>().ToList().ForEach(x => Worksheets.Add(x.Name));
                
            XDocument xDocument = XDocument.Parse(_Statistics.StatisticsList, LoadOptions.None);
            XElement xStatistics = xDocument.Element("Statistics");
            string SurveyID = xStatistics.Attribute("SurveyID").Value;
            Workbook wb = this._Template;

            if (!Worksheets.Contains("報表標題"))
                throw new Exception("教學意見調查表樣版必須有「報表標題」工作表。");
            //  報表標題：樣版一定要有「報表標題」工作表，且以其為排版依據
            this._DataBindedSheets.Add(this.GetReportHeader(xStatistics, wb));

            //  依照題目群組順序產生題目
            Dictionary<UDT.Hierarchy, List<XElement>> dicOrderedHierarchyQuestions = this.GetHierarchyQuestions(_Statistics);
            int i = 1;
            foreach (UDT.Hierarchy Hierarchy in dicOrderedHierarchyQuestions.Keys)
            {
                //  樣版中沒有此題目群組就跳過
                if (!Worksheets.Contains(Hierarchy.Title + "-標題"))
                    continue;

                //  1、題目群組標題
                this._DataBindedSheets.Add(this.GetGroupHeader(i++, Hierarchy, wb));

                //  2、題目
                List<XElement> CaseElements = new List<XElement>();
                foreach (XElement xElement in dicOrderedHierarchyQuestions[Hierarchy])
                {
                    if (xElement.Attribute("Type").Value != "問答題" && xElement.Attribute("IsCase").Value != "是")
                    {
                        if (Worksheets.Contains(Hierarchy.Title + "-選擇題-題目"))
                            this._DataBindedSheets.Add(this.GetQuestion(xElement, Hierarchy, wb));
                    }
                    if (xElement.Attribute("IsCase").Value == "是")
                    {
                        CaseElements.Add(xElement);
                    }
                    if (xElement.Attribute("Type").Value == "問答題")
                    {
                        //  樣版中有問答題
                        if (Worksheets.Contains(Hierarchy.Title + "-問答題-題目"))
                        {
                            this._DataBindedSheets.Add(this.GetEssayQuestion(xElement, Hierarchy, wb));
                            if (Worksheets.Contains(Hierarchy.Title + "-問答題-做答"))
                            {
                                IEnumerable<XElement> xAnswers = xElement.Descendants("Answer");
                                int z = 1;
                                foreach (XElement xAnswer in xAnswers)
                                {
                                    if (string.IsNullOrWhiteSpace(xAnswer.Value))
                                        continue;
                                    string DisplayOrder = xElement.Attribute("No").Value + "-" + z++;
                                    this._DataBindedSheets.Add(this.GetEssayAnswer(DisplayOrder, xAnswer, Hierarchy, wb));
                                }
                            }
                            if (Worksheets.Contains(Hierarchy.Title + "-問答題-空白列"))
                            {
                                DataBindedSheet dataBindedSheet_問答題_空白列 = new DataBindedSheet();
                                dataBindedSheet_問答題_空白列.Worksheet = wb.Worksheets[Hierarchy.Title + "-問答題-空白列"];
                                dataBindedSheet_問答題_空白列.DataTables = new List<DataTable>();
                                this._DataBindedSheets.Add(dataBindedSheet_問答題_空白列);
                            }
                        }
                    }
                }
                //  「平均平鑑值」與「個案」只能放在題目最下方顯示
                //  平均平鑑值
                if (Worksheets.Contains(Hierarchy.Title + "-平均評鑑值") && xDocument.Descendants("StatisticsGroup").Count() > 0)
                {
                    foreach (XElement xElement in xDocument.Descendants("StatisticsGroup"))
                        this._DataBindedSheets.Add(this.GetAvgScore(xElement, Hierarchy, wb));

                    if (Worksheets.Contains(Hierarchy.Title + "-平均評鑑值-空白列"))
                    {
                        DataBindedSheet dataBindedSheet_平均評鑑值_空白列 = new DataBindedSheet();
                        dataBindedSheet_平均評鑑值_空白列.Worksheet = wb.Worksheets[Hierarchy.Title + "-平均評鑑值-空白列"];
                        dataBindedSheet_平均評鑑值_空白列.DataTables = new List<DataTable>();
                        this._DataBindedSheets.Add(dataBindedSheet_平均評鑑值_空白列);
                    }
                }
                //  個案
                if (CaseElements.Count > 0)
                {
                    if (Worksheets.Contains(Hierarchy.Title + "-個案題-題目"))
                    {
                        foreach (XElement xElement in CaseElements)
                        {
                            string DisplayOrder = xElement.Attribute("No").Value;
                            this._DataBindedSheets.Add(this.GetCaseQuestion(DisplayOrder, xElement, Hierarchy, wb));
                            if (Worksheets.Contains(Hierarchy.Title + "-個案題-做答"))
                            {
                                //  個案做答
                                IEnumerable<XElement> xCases = xElement.Descendants("Case");
                                int z = 1;
                                foreach (XElement xCase in xCases)
                                {
                                    DisplayOrder = xElement.Attribute("No").Value + "-" + z++;
                                    this._DataBindedSheets.Add(this.GetCaseAnswer(DisplayOrder, xCase, Hierarchy, wb));
                                }
                            }
                        }
                    }                        
                }
                //  3、群組空白列
                if (Worksheets.Contains(Hierarchy.Title + "-空白列"))
                {
                    DataBindedSheet dataBindedSheet_Group_空白列 = new DataBindedSheet();
                    dataBindedSheet_Group_空白列.Worksheet = wb.Worksheets[Hierarchy.Title + "-空白列"];
                    dataBindedSheet_Group_空白列.DataTables = new List<DataTable>();
                    this._DataBindedSheets.Add(dataBindedSheet_Group_空白列);
                }
            }

            Workbook report = this.GenerateWorkbook();
            //  將「List<DataBindedSheet>」轉換為 Workbook
            //Dictionary<string, Workbook> dicCourseTeacherStatistics = new Dictionary<string, Workbook>();                
            return report;
        }
        private string ToChineseNo(int no)
        {
            switch (no.ToString())
            {
                case "1": return "一";
                case "2": return "二";
                case "3": return "三";
                case "4": return "四";
                case "5": return "五";
                case "6": return "六";
                case "7": return "七";
                case "8": return "八";
                case "9": return "九";
                default: return "";
            }
        }

        private Workbook GenerateWorkbook()
        {
            Workbook workbook = new Workbook();
            workbook.Worksheets.Cast<Worksheet>().ToList().ForEach(x => workbook.Worksheets.RemoveAt(x.Name));

            List<DataBindedSheet> TemplateSheets = this._DataBindedSheets;

            int instanceSheetIndex = workbook.Worksheets.Add();
            workbook.Worksheets[instanceSheetIndex].Name = this.WorksheetName;
            //workbook.Worksheets.AddCopy(TemplateSheets.ElementAt(0).Worksheet.Name);   
            workbook.Worksheets[instanceSheetIndex].Copy(TemplateSheets.ElementAt(0).Worksheet);
            Worksheet instanceSheet = workbook.Worksheets[instanceSheetIndex];

            int i = 0;
            DataSet dataSet = new DataSet();
            dataSet.Tables.AddRange(TemplateSheets.ElementAt(0).DataTables.ToArray());
            DocumentHelper.GenerateSheet(dataSet, instanceSheet, i, null);
            if (TemplateSheets.Count > 1)
            {
                TemplateSheets.RemoveAt(0);
                foreach (DataBindedSheet sheet in TemplateSheets)
                {
                    dataSet = new DataSet(sheet.Key);
                    dataSet.Tables.AddRange(sheet.DataTables.ToArray());
                    i = instanceSheet.Cells.MaxRow + 1;
                    DocumentHelper.CloneTemplate(instanceSheet, sheet.Worksheet, i);
                    DocumentHelper.GenerateSheet(dataSet, instanceSheet, i, this.dicCellStyles);
                }
            }

            //  移除樣版檔
            TemplateSheets.ForEach(x => workbook.Worksheets.RemoveAt(x.Worksheet.Name));

            //  移除報表中的變數
            DocumentHelper.RemoveReportVariable(workbook);

            // 置換工作表名稱中的保留字
            workbook.Worksheets.Cast<Worksheet>().ToList().ForEach((x) =>
            {
                x.Name = x.Name.Replace("：", "꞉").Replace(":", "꞉").Replace("/", "⁄").Replace("／", "⁄").Replace(@"\", "∖").Replace("＼", "∖").Replace("?", "_").Replace("？", "_").Replace("*", "✻").Replace("＊", "✻").Replace("[", "〔").Replace("〔", "〔").Replace("]", "〕").Replace("〕", "〕");
            });
            return workbook;
        }    
    }

    public class DataBindedSheet
    {
        public Worksheet Worksheet { get; set; }
        public List<DataTable> DataTables { get; set; }

        public string Key { get; set; }

        public DataBindedSheet()
        {
            this.Key = Guid.NewGuid().ToString();
        }
    }
}
