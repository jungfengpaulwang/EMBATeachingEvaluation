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
using System.IO;
using System.Dynamic;
using System.Web;

namespace TeachingEvaluation.Export
{
    public partial class ExportEvaluation_UseCase : BaseForm
    {
        private QueryHelper Query;
        private AccessHelper Access;

        private int SchoolYear;
        private int Semester;

        public ExportEvaluation_UseCase()
        {
            InitializeComponent();
            this.Text = "列印個案評鑑統計";
            this.TitleText = "列印個案評鑑統計";
            Query = new QueryHelper();
            Access = new AccessHelper();

            this.Load += new EventHandler(ExportEvaluation_UseCase_Load);
        }

        private void ExportEvaluation_UseCase_Load(object sender, EventArgs e)
        {
            this.circularProgress.IsRunning = false;
            this.circularProgress.Visible = false;

            this.InitSchoolYear();
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

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Export_Click(object sender, EventArgs e)
        {
            int school_year = int.Parse(this.nudSchoolYear.Value + "");
            int school_year1 = int.Parse(this.nudSchoolYear1.Value + "");

            List<CourseRecord> Courses = new List<CourseRecord>();
            List<TeacherRecord> Teachers = new List<TeacherRecord>();
            Dictionary<string, CourseRecord> dicCourses = new Dictionary<string, CourseRecord>();
            Dictionary<string, TeacherRecord> dicTeachers = new Dictionary<string, TeacherRecord>();
            List<UDT.TeacherStatistics> Statistics = new List<UDT.TeacherStatistics>();
            Dictionary<string, List<DataSet>> dicTeacherStatistics = new Dictionary<string, List<DataSet>>();
            Dictionary<string, List<dynamic>> dicCases = new Dictionary<string, List<dynamic>>();
            Dictionary<string, UDT.Case> dicCaseUs = new Dictionary<string, UDT.Case>();
            List<dynamic> Cases = new List<dynamic>();
            this.circularProgress.Visible = true;
            this.circularProgress.IsRunning = true;
            this.Export.Enabled = false;
            Workbook wb = new Workbook();
            Task task = Task.Factory.StartNew(() =>
            {
                Statistics = Access.Select<UDT.TeacherStatistics>();
                List<UDT.Case> cases = Access.Select<UDT.Case>();
                if (cases.Count > 0)
                    dicCaseUs = cases.ToDictionary(x => x.UID);
                
                foreach (UDT.TeacherStatistics Statistic in Statistics)
                {
                    if (Statistic.SchoolYear < school_year || Statistic.SchoolYear > school_year1)
                        continue;

                    XDocument xDocument = new XDocument();
                    xDocument = XDocument.Parse(Statistic.StatisticsList, LoadOptions.None);
                    XElement xStatistics = xDocument.Element("Statistics");

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

                    //  <Case ID="123" Content='我是個案1號' Score='4.58'>
                    IEnumerable<XElement> xQuestions_Case = xDocument.Descendants("Case");
                    foreach (XElement xElement in xQuestions_Case)
                    {
                        dynamic o = new ExpandoObject();

                        o.SchoolYear = SchoolYear;
                        o.Semester = Semester;
                        o.CSAttendCount = CSAttendCount;
                        o.FeedBackCount = FeedBackCount;
                        o.NewSubjectCode = NewSubjectCode;
                        o.SubjectCode = SubjectCode;
                        o.SubjectName = SubjectName;
                        o.CourseName = CourseName;
                        o.TeacherName = TeacherName;
                        o.CaseID = xElement.Attribute("ID").Value;
                        o.CaseEnglishName = dicCaseUs[xElement.Attribute("ID").Value].EnglishName;
                        o.CaseName = dicCaseUs[xElement.Attribute("ID").Value].Name;

                        o.Content = xElement.Attribute("Content").Value;
                        o.Score = decimal.Parse(xElement.Attribute("Score").Value);
                        o.AvgScore = 0;

                        Cases.Add(o);

                        if (!dicCases.ContainsKey(xElement.Attribute("ID").Value))
                            dicCases.Add(xElement.Attribute("ID").Value, new List<dynamic>());
                        dicCases[xElement.Attribute("ID").Value].Add(o);
                    }
                }
                foreach (string key in dicCases.Keys)
                {
                    decimal score = 0;
                    int i = 0;
                    foreach (dynamic o in dicCases[key])
                    {
                        i++;
                        score += o.Score;
                    }
                    decimal avg_score = Math.Round(score / i, 2, MidpointRounding.AwayFromZero);
                    dicCases[key].ForEach(x=>x.AvgScore = avg_score);
                }
                Cases = Cases.OrderBy(x => int.Parse(x.SchoolYear + "")).ThenBy(x => int.Parse(x.Semester + "")).ThenBy(x => x.CaseID).ToList();

                wb.Worksheets.Cast<Worksheet>().ToList().ForEach((y) => wb.Worksheets.RemoveAt(y.Name));
                int idx = wb.Worksheets.Add();
                wb.Worksheets[idx].Name = school_year + "~" + school_year1 + "-個案教學評鑑統計表.xls";
                wb.Worksheets[idx].Cells[0, 0].PutValue("學年度");
                wb.Worksheets[idx].Cells[0, 1].PutValue("學期");
                wb.Worksheets[idx].Cells[0, 2].PutValue("課號");
                wb.Worksheets[idx].Cells[0, 3].PutValue("課程識別碼");
                wb.Worksheets[idx].Cells[0, 4].PutValue("課程");
                wb.Worksheets[idx].Cells[0, 5].PutValue("開課");
                wb.Worksheets[idx].Cells[0, 6].PutValue("任課教師");
                wb.Worksheets[idx].Cells[0, 7].PutValue("修課人數");
                wb.Worksheets[idx].Cells[0, 8].PutValue("填答人數");
                wb.Worksheets[idx].Cells[0, 9].PutValue("個案英文名稱");
                wb.Worksheets[idx].Cells[0, 10].PutValue("個案中文名稱");
                wb.Worksheets[idx].Cells[0, 11].PutValue("評鑑值");
                wb.Worksheets[idx].Cells[0, 12].PutValue("平均評鑑值");
                int j = 0;
                foreach (dynamic o in Cases)
                {           
                    j++;

                    wb.Worksheets[idx].Cells[j, 0].PutValue(int.Parse(o.SchoolYear));
                    wb.Worksheets[idx].Cells[j, 1].PutValue(int.Parse(o.Semester));
                    wb.Worksheets[idx].Cells[j, 2].PutValue(o.NewSubjectCode);
                    wb.Worksheets[idx].Cells[j, 3].PutValue(o.SubjectCode);
                    wb.Worksheets[idx].Cells[j, 4].PutValue(o.SubjectName);
                    wb.Worksheets[idx].Cells[j, 5].PutValue(o.CourseName);
                    wb.Worksheets[idx].Cells[j, 6].PutValue(o.TeacherName);
                    wb.Worksheets[idx].Cells[j, 7].PutValue(o.CSAttendCount);
                    wb.Worksheets[idx].Cells[j, 8].PutValue(o.FeedBackCount);
                    wb.Worksheets[idx].Cells[j, 9].PutValue(o.CaseEnglishName);
                    wb.Worksheets[idx].Cells[j, 10].PutValue(o.CaseName);
                    wb.Worksheets[idx].Cells[j, 11].PutValue(o.Score);
                    wb.Worksheets[idx].Cells[j, 12].PutValue(o.AvgScore);
                }
                if (wb.Worksheets.Count == 0)
                    throw new Exception("沒有資料。");
                else
                    wb.Worksheets.Cast<Worksheet>().ToList().ForEach(y => y.AutoFitColumns());
            });

            task.ContinueWith((x) =>
            {
                if (x.Exception != null)
                {
                    MessageBox.Show(x.Exception.InnerException.Message);
                    goto TheEnd;
                }
                SaveFileDialog sd = new SaveFileDialog();
                sd.Title = "另存新檔";
                sd.FileName = school_year + "~" + school_year1 + "-個案教學評鑑統計表.xls";
                sd.Filter = "Excel 2003 相容檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
                if (sd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        wb.Save(sd.FileName, FileFormatType.Excel2003);
                        System.Diagnostics.Process.Start(sd.FileName);
                    }
                    catch
                    {
                        MessageBox.Show("指定路徑無法存取。", "建立檔案失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                TheEnd:
                this.circularProgress.IsRunning = false;
                this.circularProgress.Visible = false;
                this.Export.Enabled = true;
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }
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