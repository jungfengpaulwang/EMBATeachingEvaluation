using Aspose.Cells;
using FISCA.Data;
using FISCA.Presentation.Controls;
using FISCA.UDT;
using K12.Data;
using ReportHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml.Linq;

namespace TeachingEvaluation.Export
{
    public partial class ExportEvaluation_ComplexQuerie_2014 : BaseForm
    {
        private AccessHelper Access;
        private QueryHelper Query;

        private List<UDT.TeacherStatistics> Statistics;
        private List<CourseRecord> Courses;
        private List<dynamic> Teachers;
        private List<dynamic> Subjects;

        private bool form_loaded;

        public ExportEvaluation_ComplexQuerie_2014()
        {
            InitializeComponent();

            this.InitSchoolYear();
            this.InitSemester();

            this.Courses = new List<CourseRecord>();
            this.Statistics = new List<UDT.TeacherStatistics>();

            this.Load += new EventHandler(Form_Load);
            this.dgvData.MouseClick += new MouseEventHandler(dgvData_MouseClick);

            Access = new AccessHelper();
            Query = new QueryHelper();
        }

        private void dgvData_MouseClick(object sender, MouseEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            DataGridView.HitTestInfo hit = dgv.HitTest(e.X, e.Y);

            if (hit.Type == DataGridViewHitTestType.TopLeftHeader)
            {
                dgvData.CurrentCell = null;
                dgvData.SelectAll();
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            this.circularProgress.Visible = true;
            this.circularProgress.IsRunning = true;
            this.btnExit.Enabled = false;
            this.btnQuery.Enabled = false;
            this.btnExport.Enabled = false;
            int school_year = int.Parse(this.nudSchoolYear.Value + "");
            int school_year1 = int.Parse(this.nudSchoolYear1.Value + "");
            int semester = int.Parse((this.cboSemester.SelectedItem as DataItems.SemesterItem).Value);
            int semester1 = int.Parse((this.cboSemester1.SelectedItem as DataItems.SemesterItem).Value);

            Teachers = new List<dynamic>();
            Subjects = new List<dynamic>();

            Task task = Task.Factory.StartNew(() =>
            {
                //  開課
                Courses = Course.SelectAll();

                //  授課教師
                string SQL = string.Format(@"SELECT  distinct teacher.id as teacher_id, teacher.teacher_name, course.school_year, course.semester FROM course LEFT JOIN $ischool.emba.course_instructor ON $ischool.emba.course_instructor.ref_course_id = course.id LEFT JOIN teacher ON teacher.id = $ischool.emba.course_instructor.ref_teacher_id LEFT JOIN tag_teacher ON tag_teacher.ref_teacher_id = teacher.id LEFT JOIN tag ON tag.id = tag_teacher.ref_tag_id WHERE tag.category = 'Teacher' AND tag.prefix = '教師' Order by teacher_name");

                DataTable dataTable = Query.Select(SQL);
                foreach (DataRow row in dataTable.Rows)
                {
                    Teachers.Add(new { ID = row["teacher_id"] + "", Name = row["teacher_name"] + "", SchoolYear = row["school_year"] + "", Semester = row["semester"] + "" });
                }

                //  課程
                SQL = string.Format(@"select distinct subject.uid as subject_id, subject.name as subject_name, course.school_year, course.semester from course join $ischool.emba.course_ext as ce on ce.ref_course_id=course.id join $ischool.emba.subject as subject on subject.uid=ce.ref_subject_id");
                
                dataTable = Query.Select(SQL);
                foreach (DataRow row in dataTable.Rows)
                    Subjects.Add(new { UID = row["subject_id"] + "", Name = row["subject_name"] + "", SchoolYear = row["school_year"] + "", Semester = row["semester"] + "" });

                //  評鑑值
                Statistics = Access.Select<UDT.TeacherStatistics>();
                Statistics = Statistics.OrderBy(x => x.SchoolYear).ThenBy(x => x.Semester).ThenBy(x => x.CourseID).ThenBy(x => x.TeacherID).ToList();
            });
            task.ContinueWith((x) =>
            {
                this.circularProgress.Visible = false;
                this.circularProgress.IsRunning = false;
                this.btnExit.Enabled = true;
                this.btnQuery.Enabled = true;
                this.btnExport.Enabled = true;
                this.form_loaded = true;
                if (x.Exception != null)
                {
                    MessageBox.Show(x.Exception.InnerException.Message);
                    return;
                }
                this.InitSubject(school_year, school_year1, semester, semester1);
                this.InitCourse(school_year, school_year1, semester, semester1);
                this.InitTeacher(school_year, school_year1, semester, semester1);
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

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

        //  課程
        private void InitSubject(int school_year, int school_year1, int semester, int semester1)
        {
            List<dynamic> new_subjects = new List<dynamic>();
            Dictionary<string, string> dicSubjects = new Dictionary<string, string>();
            new_subjects.Add(new { ID = "", Name = "" });
            foreach (dynamic o in this.Subjects)
            {
                if (int.Parse(o.SchoolYear) < school_year || int.Parse(o.SchoolYear) > school_year1)
                    continue;
                if (int.Parse(o.SchoolYear) == school_year && int.Parse(o.Semester) < semester)
                    continue;
                if (int.Parse(o.SchoolYear) == school_year1 && int.Parse(o.Semester) > semester1)
                    continue;

                if (!dicSubjects.ContainsKey((string)o.UID))
                {
                    dicSubjects.Add((string)o.UID, (string)o.Name);
                    new_subjects.Add(new { ID = (string)o.UID, Name = (string)o.Name });
                }
            }
            this.cboSubject.DataSource = new_subjects;
            this.cboSubject.ValueMember = "ID";
            this.cboSubject.DisplayMember = "Name";
            this.cboSubject.AutoCompleteMode = AutoCompleteMode.Suggest;
            this.cboSubject.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void InitCourse(int school_year, int school_year1, int semester, int semester1)
        {
            //  開課
            List<dynamic> oCourses = new List<dynamic>();
            oCourses.Add(new { ID = "", Name = "" });
            foreach(CourseRecord Course in this.Courses)
            {
                if (Course.SchoolYear < school_year || Course.SchoolYear > school_year1)
                    continue;
                if (Course.SchoolYear == school_year && Course.Semester < semester)
                    continue;
                if (Course.SchoolYear == school_year1 && Course.Semester > semester1)
                    continue;

                oCourses.Add(new { ID = Course.ID, Name = Course.Name });
            }
            this.cboCourse.DataSource = oCourses;
            this.cboCourse.ValueMember = "ID";
            this.cboCourse.DisplayMember = "Name";
            this.cboCourse.AutoCompleteMode = AutoCompleteMode.Suggest;
            this.cboCourse.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        //  授課教師
        private void InitTeacher(int school_year, int school_year1, int semester, int semester1)
        {
            List<dynamic> new_teachers = new List<dynamic>();
            Dictionary<string, string> dicTeachers = new Dictionary<string, string>();
            new_teachers.Add(new { ID = "", Name = "" });
            foreach (dynamic o in this.Teachers)
            {
                if (int.Parse(o.SchoolYear) < school_year || int.Parse(o.SchoolYear) > school_year1)
                    continue;
                if (int.Parse(o.SchoolYear) == school_year && int.Parse(o.Semester) < semester)
                    continue;
                if (int.Parse(o.SchoolYear) == school_year1 && int.Parse(o.Semester) > semester1)
                    continue;

                if (!dicTeachers.ContainsKey((string)o.ID))
                {
                    dicTeachers.Add((string)o.ID, (string)o.Name);
                    new_teachers.Add(new { ID = (string)o.ID, Name = (string)o.Name });
                }
            }
            this.cboTeacher.DataSource = new_teachers;
            this.cboTeacher.ValueMember = "ID";
            this.cboTeacher.DisplayMember = "Name";
            this.cboTeacher.AutoCompleteMode = AutoCompleteMode.Suggest;
            this.cboTeacher.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {            
            this.dgvData.Rows.Clear();

            List<string> DGV_Keys = new List<string>();

            int school_year = int.Parse(this.nudSchoolYear.Value + "");
            int school_year1 = int.Parse(this.nudSchoolYear1.Value + "");
            int semester = int.Parse((this.cboSemester.SelectedItem as DataItems.SemesterItem).Value);
            int semester1 = int.Parse((this.cboSemester1.SelectedItem as DataItems.SemesterItem).Value);

            string subject_id = this.cboSubject.SelectedValue + "";
            string course_id = this.cboCourse.SelectedValue + "";
            string teacher_id = this.cboTeacher.SelectedValue + "";

            List<dynamic> DataGridViewSource = new List<dynamic>();
            this.circularProgress.Visible = true;
            this.circularProgress.IsRunning = true;
            Task task = Task.Factory.StartNew(() =>
            {
                foreach (UDT.TeacherStatistics TeacherStatistics in this.Statistics)
                {
                    bool wanted = false;

                    int oSchoolYear = int.Parse(TeacherStatistics.SchoolYear + "");
                    int oSemester = int.Parse(TeacherStatistics.Semester + "");

                    if (oSchoolYear < school_year || oSchoolYear > school_year1)
                        continue;
                    if (oSchoolYear == school_year && oSemester < semester)
                        continue;
                    if (oSchoolYear == school_year1 && oSemester > semester1)
                        continue;

                    //StringBuilder sb = new StringBuilder(string.Format(@"<Statistics CSAttendCount=""{0}"" FeedBackCount=""{1}"" TeacherName=""{2}"" CourseName=""{3}"" SubjectName=""{4}"" SubjectCode=""{5}"" NewSubjectCode=""{6}"" SchoolYear=""{7}"" Semester=""{8}"" ClassName=""{9}"" CourseID=""{10}"" SubjectID=""{11}"" TeacherID=""{12}"" SurveyDate=""{13}"" SurveyID=""{14}"">", this.CSAttendCount, this.FeedBackCount, this.TeacherName, this.CourseName, this.SubjectName, this.SubjectCode, this.NewSubjectCode, this.SchoolYear, this.Semester, this.ClassName, this.CourseID, this.SubjectID, this.TeacherID, this.SurveyDate, this.Survey.uSurvey.UID));

                    XDocument xDocument = new XDocument();
                    xDocument = XDocument.Parse(TeacherStatistics.StatisticsList, LoadOptions.None);
                    XElement xStatistics = xDocument.Element("Statistics");

                    string CSAttendCount = xStatistics.Attribute("CSAttendCount").Value;
                    string FeedBackCount = xStatistics.Attribute("FeedBackCount").Value;
                    string TeacherName = xStatistics.Attribute("TeacherName").Value;
                    string TeacherID = xStatistics.Attribute("TeacherID").Value;
                    string CourseName = xStatistics.Attribute("CourseName").Value;
                    string CourseID = xStatistics.Attribute("CourseID").Value;
                    string SubjectName = xStatistics.Attribute("SubjectName").Value;
                    string SubjectID = xStatistics.Attribute("SubjectID").Value;
                    string ClassName = xStatistics.Attribute("ClassName").Value;
                    string SubjectCode = xStatistics.Attribute("SubjectCode").Value;
                    string NewSubjectCode = xStatistics.Attribute("NewSubjectCode").Value;
                    string SchoolYear = xStatistics.Attribute("SchoolYear").Value;
                    string Semester = xStatistics.Attribute("Semester").Value;
                    string SurveyDate = xStatistics.Attribute("SurveyDate").Value;
                    string SurveyID = xStatistics.Attribute("SurveyID").Value;

                    if (SubjectID == subject_id)
                        wanted = true;
                    if (TeacherID == teacher_id)
                        wanted = true;
                    if (CourseID == course_id)
                        wanted = true;

                    if (string.IsNullOrEmpty(subject_id) && string.IsNullOrEmpty(teacher_id) && string.IsNullOrEmpty(course_id))
                        wanted = true;

                    if (wanted)
                    {
                        dynamic o = new ExpandoObject();

                        o.SchoolYear = SchoolYear;
                        o.Semester = Semester;
                        o.SubjectName = SubjectName;
                        o.TeacherName = TeacherName;
                        o.CourseName = CourseName;
                        o.TeacherStatistics = TeacherStatistics;

                        DataGridViewSource.Add(o);
                    }
                }            
            });
            task.ContinueWith((x) =>
            {
                this.circularProgress.Visible = false;
                this.circularProgress.IsRunning = false;
                if (x.Exception != null)
                {
                    MessageBox.Show(x.Exception.InnerException.Message);
                    return;
                }
                foreach (dynamic o in DataGridViewSource)
                {
                    List<object> source = new List<object>();

                    source.Add(o.SchoolYear);
                    source.Add(o.Semester);
                    source.Add(o.SubjectName);
                    source.Add(o.CourseName);
                    source.Add(o.TeacherName);

                    int idx = this.dgvData.Rows.Add(source.ToArray());
                    this.dgvData.Rows[idx].Tag = o.TeacherStatistics as UDT.TeacherStatistics;
                }
                if (this.dgvData.Rows.Count == 0)
                    MessageBox.Show("沒有資料。");
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());            
        }

        private string NoToChineseNo(int no)
        {
            string[] ChineseNo = new string[] { "〇", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };

            if (no >= 0 && no <= 10)
                return ChineseNo.ElementAt(no);
            else
                return "";
        }

        private void MakeGroupData(DataTable EnrollRecordTable, UDT.Hierarchy Hierarchy, string WorksheetName, Dictionary<ReportHelper.CellObject, ReportHelper.CellStyle> dicCellStyles)
        {
            DataRow pRow = EnrollRecordTable.NewRow();
            
            pRow["No"] = NoToChineseNo(Hierarchy.DisplayOrder) + "、" + Hierarchy.Title;

            if (Hierarchy.DisplayOrder == 1)
            {
                pRow["Option1AnswerCount"] = "(1)";
                pRow["Option2AnswerCount"] = "(2)";
                pRow["Option3AnswerCount"] = "(3)";
                pRow["Option4AnswerCount"] = "(4)";
                pRow["Option5AnswerCount"] = "(5)";
                pRow["Option6AnswerCount"] = "(6)";
                //pRow["Score"] = "評鑑值";
            }

            EnrollRecordTable.Rows.Add(pRow);

            CellObject co = new CellObject(EnrollRecordTable.Rows.Count - 1, 0, EnrollRecordTable.TableName, "DataSection", WorksheetName);
            CellStyle cs = new CellStyle();

            //  題目群組首 Style：標楷體、粗、12號字、水平靠左、垂直靠上
            cs.SetRowHeight(21.75).SetFontName("標楷體").SetFontSize(12).SetFontBold(true).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Left).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Top);            
            dicCellStyles.Add(co, cs);

            //  答案選項標題 Style：TimesNewRoman、10號字、水平置中、垂直靠下
            for (int i = 2; i <= 7; i++)
            {
                co = new CellObject(EnrollRecordTable.Rows.Count - 1, i, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                cs = new CellStyle();
                cs.SetFontName("Times New Roman").SetFontSize(10).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Center).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Bottom);
                dicCellStyles.Add(co, cs);
            }
            ////  評鑑值標題
            //co = new CellObject(EnrollRecordTable.Rows.Count - 1, 8, EnrollRecordTable.TableName, "DataSection", WorksheetName);
            //cs = new CellStyle();
            //cs.SetFontName("標楷體").SetFontSize(12).SetFontBold(true).SetFontHorizontalAlignment(HorizontalAlignment.Center).SetFontVerticalAlignment(HorizontalAlignment.Right);
            //dicCellStyles.Add(co, cs);
        }

        private void MakeDetailOneData(DataTable EnrollRecordTable, UDT.Hierarchy Hierarchy, List<XElement> xElements, string WorksheetName, Dictionary<ReportHelper.CellObject, ReportHelper.CellStyle> dicCellStyles)
        {
            if (Hierarchy.DisplayOrder != 1)
                return;

            foreach (XElement xElement in xElements)
            {
                DataRow pRow = EnrollRecordTable.NewRow();

                pRow["No"] = xElement.Attribute("No").Value + ".";
                pRow["Content"] = HttpUtility.HtmlDecode(xElement.Attribute("Content").Value);
                IEnumerable<XElement> Options = xElement.Descendants("Option");
                if (Options.Count() > 0) Options = Options.OrderBy(x => int.Parse(x.Attribute("No").Value));
                int j = 0;
                string question_content = string.Empty;
                foreach (XElement xOption in Options)
                {
                    pRow["Option" + xOption.Attribute("No").Value + "AnswerCount"] = xOption.Attribute("AnswerCount").Value;
                    question_content += "(" + xOption.Attribute("No").Value + ")" + HttpUtility.HtmlDecode(xOption.Attribute("Content").Value);
                }
                //if (question_content.EndsWith("、"))
                //    question_content = question_content.Remove(question_content.Length - 1, 1);
                pRow["Content"] += question_content;
                //pRow["Score"] = xElement.Attribute("Score").Value;

                EnrollRecordTable.Rows.Add(pRow);

                CellObject co = new CellObject(EnrollRecordTable.Rows.Count - 1, 0, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                CellStyle cs = new CellStyle();

                //  題號 Style：Times New Roman、12號字、水平置中、垂直靠上
                cs.SetRowHeight(35).SetFontName("Times New Roman").SetFontSize(12).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Center).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Top);
                dicCellStyles.Add(co, cs);

                //  題目及答案選項 Style：12號字、水平靠左、垂直靠上
                co = new CellObject(EnrollRecordTable.Rows.Count - 1, 1, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                cs = new CellStyle();
                cs.SetFontName("新細明體").SetFontSize(12).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Left).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Top).SetAutoFitRow(true);
                dicCellStyles.Add(co, cs);

                //  答題數
                j = 2;
                foreach (XElement xOption in Options)
                {
                    co = new CellObject(EnrollRecordTable.Rows.Count - 1, j, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                    cs = new CellStyle();
                    cs.SetFontName("新細明體").SetFontSize(12).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Bottom).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Center);
                    dicCellStyles.Add(co, cs);
                    j++;
                }
            }
        }

        private void MakeDetailTwoData(DataTable EnrollRecordTable, UDT.Hierarchy Hierarchy, List<XElement> xElements, IEnumerable<XElement> xStatisticsGroup, string WorksheetName, Dictionary<ReportHelper.CellObject, ReportHelper.CellStyle> dicCellStyles, Dictionary<string, Dictionary<string, Color>> dicQuestionBackgroundColor, Dictionary<string, Dictionary<string, Color>> dicEvaluationBackgroundColor, string SurveyID)
        {
            if (Hierarchy.DisplayOrder != 2)
                return;

            if (xElements.Count == 0) return;
            
            IEnumerable<XElement> Options = xElements.ElementAt(0).Descendants("Option");
            if (Options.Count() > 0) Options = Options.OrderBy(x => int.Parse(x.Attribute("No").Value));

            //  列舉答案選項
            DataRow pRow = EnrollRecordTable.NewRow();
            string option_content = string.Empty; 
            int j = 0;
            foreach (XElement xOption in Options)
            {
                option_content += "(" + xOption.Attribute("No").Value + ")" + HttpUtility.HtmlDecode(xOption.Attribute("Content").Value) + "  ";
            }
            pRow["Content"] = option_content;
            pRow["Score"] = "評鑑值";
            EnrollRecordTable.Rows.Add(pRow);

            CellObject co = new CellObject(EnrollRecordTable.Rows.Count - 1, 1, EnrollRecordTable.TableName, "DataSection", WorksheetName);
            CellStyle cs = new CellStyle();

            //  答題選項 Style：粗體、標楷體、12號字、垂直靠下
            cs.SetRowHeight(18).SetFontBold(true).SetFontName("標楷體").SetFontSize(12).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Bottom);
            dicCellStyles.Add(co, cs);

            co = new CellObject(EnrollRecordTable.Rows.Count - 1, 8, EnrollRecordTable.TableName, "DataSection", WorksheetName);
            cs = new CellStyle();

            //  評鑑值 Style：粗體、標楷體、12號字、水平置中、垂直靠下
            cs.SetFontName("標楷體").SetFontBold(true).SetFontSize(12).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Center).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Bottom);
            dicCellStyles.Add(co, cs);

            //  列舉答案題號
            pRow = EnrollRecordTable.NewRow();
            foreach (XElement xOption in Options)
            {
                pRow["Option" + xOption.Attribute("No").Value + "AnswerCount"] = "(" + xOption.Attribute("No").Value + ")";
            }
            EnrollRecordTable.Rows.Add(pRow);

            //  答案項次 Style：Times New Roman、10號字、水平置中、垂直靠下
            j = 2;
            foreach (XElement xOption in Options)
            {
                co = new CellObject(EnrollRecordTable.Rows.Count - 1, j, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                cs = new CellStyle();

                cs.SetRowHeight(14.25).SetFontName("Times New Roman").SetFontSize(10).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Bottom).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Center);
                dicCellStyles.Add(co, cs);
                j++;
            }

            //  列舉學生做答
            foreach (XElement xElement in xElements)
            {
                if (xElement.Attribute("IsCase").Value == "是")
                    continue;

                pRow = EnrollRecordTable.NewRow();

                pRow["No"] = xElement.Attribute("No").Value + ".";
                pRow["Content"] = HttpUtility.HtmlDecode(xElement.Attribute("Content").Value);

                foreach (XElement xOption in xElement.Descendants("Option"))
                {
                    pRow["Option" + xOption.Attribute("No").Value + "AnswerCount"] = xOption.Attribute("AnswerCount").Value;
                }
                if (xElement.Attribute("Score") != null)
                    pRow["Score"] = xElement.Attribute("Score").Value;

                EnrollRecordTable.Rows.Add(pRow);

                //  題號 Style：Times New Roman、12號字、水平置中、垂直靠上
                co = new CellObject(EnrollRecordTable.Rows.Count - 1, 0, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                cs = new CellStyle();
                cs.SetRowHeight(16.5).SetFontName("Times New Roman").SetFontSize(12).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Top).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Center);
                dicCellStyles.Add(co, cs);

                //  題目 Style：新細明體、12號字、水平靠左、垂直靠上
                co = new CellObject(EnrollRecordTable.Rows.Count - 1, 1, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                cs = new CellStyle();
                cs.SetFontName("新細明體").SetFontSize(12).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Left).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Top).SetAutoFitRow(true); 
                if (dicQuestionBackgroundColor.ContainsKey(SurveyID))
                    if (dicQuestionBackgroundColor[SurveyID].ContainsKey(xElement.Attribute("No").Value))
                        cs.SetFontBackGroundColor(dicQuestionBackgroundColor[SurveyID][xElement.Attribute("No").Value]);
                dicCellStyles.Add(co, cs);

                for (int zz = 2; zz <= 7; zz++)
                {
                    //  答題數 Style：新細明體、12號字、水平置中、垂直靠下
                    co = new CellObject(EnrollRecordTable.Rows.Count - 1, zz, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                    cs = new CellStyle();
                    cs.SetFontName("新細明體").SetFontSize(12).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Bottom).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Center);
                    if (dicQuestionBackgroundColor.ContainsKey(SurveyID))
                        if (dicQuestionBackgroundColor[SurveyID].ContainsKey(xElement.Attribute("No").Value))
                            cs.SetFontBackGroundColor(dicQuestionBackgroundColor[SurveyID][xElement.Attribute("No").Value]);
                    dicCellStyles.Add(co, cs);
                }

                //  評鑑值 Style：新細明體、12號字、垂直靠下、水平置中
                co = new CellObject(EnrollRecordTable.Rows.Count - 1, 8, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                cs = new CellStyle();
                cs.SetFontName("新細明體").SetFontSize(12).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Bottom).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Center);
                if (dicQuestionBackgroundColor.ContainsKey(SurveyID))
                    if (dicQuestionBackgroundColor[SurveyID].ContainsKey(xElement.Attribute("No").Value))
                        cs.SetFontBackGroundColor(dicQuestionBackgroundColor[SurveyID][xElement.Attribute("No").Value]);
                dicCellStyles.Add(co, cs);
            }
            //  平均評鑑值
            foreach (XElement xElement in xStatisticsGroup)
            {
                pRow = EnrollRecordTable.NewRow();

                //pRow["No"] = string.Empty;
                pRow["Content"] = HttpUtility.HtmlDecode(xElement.Attribute("Content").Value);
                pRow["Score"] = xElement.Attribute("Score").Value;

                EnrollRecordTable.Rows.Add(pRow);

                //  平均評鑑值標題 Style：粗體、底線、標楷體、12號字、水平靠左、垂直靠上
                co = new CellObject(EnrollRecordTable.Rows.Count - 1, 1, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                cs = new CellStyle();
                cs.SetRowHeight(18).SetFontBold(true).SetFontUnderline(true).SetFontName("標楷體").SetFontSize(12).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Left).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Top);
                dicCellStyles.Add(co, cs);

                //  評鑑值 Style：新細明體、12號字、垂直靠下、水平置中
                co = new CellObject(EnrollRecordTable.Rows.Count - 1, 8, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                cs = new CellStyle();
                cs.SetFontName("新細明體").SetFontSize(12).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Bottom).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Center);
                //  評鑑值背景色
                if (dicEvaluationBackgroundColor.ContainsKey(SurveyID))
                    if (dicEvaluationBackgroundColor[SurveyID].ContainsKey(HttpUtility.HtmlDecode(xElement.Attribute("Content").Value)))
                        cs.SetFontBackGroundColor(dicEvaluationBackgroundColor[SurveyID][HttpUtility.HtmlDecode(xElement.Attribute("Content").Value)]);
                dicCellStyles.Add(co, cs);
            }
            if (xElements.Where(x => x.Attribute("IsCase").Value == "是").Count() > 0)
            {
                //  空一行
                pRow = EnrollRecordTable.NewRow();
                EnrollRecordTable.Rows.Add(pRow);

                co = new CellObject(EnrollRecordTable.Rows.Count - 1, 0, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                cs = new CellStyle();
                cs.SetRowHeight(9.5);
                dicCellStyles.Add(co, cs);
            }
            //  個案題
            foreach (XElement xElement in xElements)
            {
                if (xElement.Attribute("IsCase").Value == "否")
                    continue;

                pRow = EnrollRecordTable.NewRow();

                pRow["No"] = xElement.Attribute("No").Value + ".";
                pRow["Content"] = HttpUtility.HtmlDecode(xElement.Attribute("Content").Value);

                EnrollRecordTable.Rows.Add(pRow);

                //  題號 Style：Times New Roman、12號字、垂直靠上，水平置中
                co = new CellObject(EnrollRecordTable.Rows.Count - 1, 0, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                cs = new CellStyle();
                cs.SetRowHeight(18).SetFontName("Times New Roman").SetFontSize(12).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Top).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Center);
                dicCellStyles.Add(co, cs);

                //  題目 Style：新細明體、12號字、垂直靠上，水平靠左
                co = new CellObject(EnrollRecordTable.Rows.Count - 1, 1, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                cs = new CellStyle();
                cs.SetFontName("新細明體").SetFontSize(12).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Left).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Top).SetAutoFitRow(true);
                dicCellStyles.Add(co, cs);

                IEnumerable<XElement> xCases = xElement.Descendants("Case");
                int z = 0;
                foreach (XElement xCase in xCases)
                {
                    z++;

                    pRow = EnrollRecordTable.NewRow();

                    pRow["No"] = xElement.Attribute("No").Value + "-" + z;
                    pRow["Content"] = HttpUtility.HtmlDecode(xCase.Attribute("Content").Value);
                    pRow["Score"] = xCase.Attribute("Score").Value;

                    Options = xCase.Descendants("Option");
                    if (Options.Count() > 0) Options = Options.OrderBy(x => int.Parse(x.Attribute("No").Value));
                    foreach (XElement xOption in Options)
                    {
                        pRow["Option" + xOption.Attribute("No").Value + "AnswerCount"] = xOption.Attribute("AnswerCount").Value;
                    }
                    EnrollRecordTable.Rows.Add(pRow);

                    //  題號 Style：Times New Roman、11號字、垂直靠上，水平置中
                    co = new CellObject(EnrollRecordTable.Rows.Count - 1, 0, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                    cs = new CellStyle();
                    cs.SetFontName("Times New Roman").SetFontSize(11).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Top).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Center);
                    dicCellStyles.Add(co, cs);

                    //  題目 Style：新細明體、12號字、垂直靠上，水平靠左
                    co = new CellObject(EnrollRecordTable.Rows.Count - 1, 1, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                    cs = new CellStyle();
                    cs.SetFontName("新細明體").SetFontSize(12).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Left).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Top).SetAutoFitRow(true);
                    dicCellStyles.Add(co, cs);

                    j = 2;
                    foreach (XElement xOption in Options)
                    {
                        //  答題數 Style：新細明體、12號字、水平置中、垂直靠下
                        co = new CellObject(EnrollRecordTable.Rows.Count - 1, j, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                        cs = new CellStyle();
                        cs.SetFontName("新細明體").SetFontSize(12).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Bottom).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Center);
                        dicCellStyles.Add(co, cs);
                        j++;
                    }

                    //  評鑑值 Style：新細明體、12號字、垂直靠下、水平置中
                    co = new CellObject(EnrollRecordTable.Rows.Count - 1, 8, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                    cs = new CellStyle();
                    cs.SetFontName("新細明體").SetFontSize(12).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Bottom).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Center);
                    dicCellStyles.Add(co, cs);
                }
            }
            //  空一行
            pRow = EnrollRecordTable.NewRow();
            EnrollRecordTable.Rows.Add(pRow);

            if (xElements.Where(x => x.Attribute("IsCase").Value == "是").Count() > 0)
            {
                co = new CellObject(EnrollRecordTable.Rows.Count - 1, 0, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                cs = new CellStyle();
                cs.SetRowHeight(11);
                dicCellStyles.Add(co, cs);
            }
            else
            {
                co = new CellObject(EnrollRecordTable.Rows.Count - 1, 0, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                cs = new CellStyle();
                cs.SetRowHeight(17.5);
                dicCellStyles.Add(co, cs);
            }
        }

        private void MakeDetailThreeData(DataTable EnrollRecordTable, UDT.Hierarchy Hierarchy, List<XElement> xElements, string WorksheetName, Dictionary<ReportHelper.CellObject, ReportHelper.CellStyle> dicCellStyles)
        {
            if (Hierarchy.DisplayOrder != 3)
                return;

            foreach (XElement xElement in xElements)
            {
                DataRow pRow = EnrollRecordTable.NewRow();

                pRow["No"] = xElement.Attribute("No").Value + ".";
                pRow["Content"] = HttpUtility.HtmlDecode(xElement.Attribute("Content").Value);
                IEnumerable<XElement> Options = xElement.Descendants("Option");
                if (Options.Count() > 0) Options = Options.OrderBy(x => int.Parse(x.Attribute("No").Value));
                int j = 0;
                foreach (XElement xOption in Options)
                {
                    pRow["Option" + xOption.Attribute("No").Value + "AnswerCount"] = xOption.Attribute("AnswerCount").Value;
                }
                if (xElement.Attribute("Score") != null)
                    pRow["Score"] = xElement.Attribute("Score").Value;

                EnrollRecordTable.Rows.Add(pRow);

                CellObject co = new CellObject(EnrollRecordTable.Rows.Count - 1, 0, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                CellStyle cs = new CellStyle();

                //  題號 Style：Times New Roman、12號字、垂直靠上，水平置中
                cs.SetRowHeight(18.75).SetFontName("Times New Roman").SetFontSize(12).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Top).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Center);
                dicCellStyles.Add(co, cs);

                co = new CellObject(EnrollRecordTable.Rows.Count - 1, 1, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                cs = new CellStyle();

                //  題目 Style：新細明體、12號字、垂直靠上，水平靠左
                cs.SetFontName("新細明體").SetFontSize(12).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Left).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Top).SetAutoFitRow(true);
                dicCellStyles.Add(co, cs);

                j = 2;
                foreach (XElement xOption in Options)
                {
                    //  答題數 Style：新細明體、12號字、水平置中、垂直靠下
                    co = new CellObject(EnrollRecordTable.Rows.Count - 1, j, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                    cs = new CellStyle();
                    cs.SetFontName("新細明體").SetFontSize(12).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Bottom).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Center);
                    dicCellStyles.Add(co, cs);
                    j++;
                }

                //  評鑑值 Style：新細明體、12號字、垂直靠下、水平置中
                co = new CellObject(EnrollRecordTable.Rows.Count - 1, 8, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                cs = new CellStyle();
                cs.SetFontName("新細明體").SetFontSize(12).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Bottom).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Center);
                dicCellStyles.Add(co, cs);
            }

            //  空一行            
            DataRow Row = EnrollRecordTable.NewRow();
            EnrollRecordTable.Rows.Add(Row);

            CellObject cc = new CellObject(EnrollRecordTable.Rows.Count - 1, 0, EnrollRecordTable.TableName, "DataSection", WorksheetName);
            CellStyle ss = new CellStyle();
            ss.SetRowHeight(11);
            dicCellStyles.Add(cc, ss);
        }

        private void MakeDetailFourData(DataTable EnrollRecordTable, UDT.Hierarchy Hierarchy, List<XElement> xElements, string WorksheetName, Dictionary<ReportHelper.CellObject, ReportHelper.CellStyle> dicCellStyles)
        {
            if (Hierarchy.DisplayOrder != 4)
                return;

            foreach (XElement xElement in xElements)
            {
                DataRow pRow = EnrollRecordTable.NewRow();

                pRow["No"] = xElement.Attribute("No").Value + ".";
                pRow["Content"] = HttpUtility.HtmlDecode(xElement.Attribute("Content").Value);

                EnrollRecordTable.Rows.Add(pRow);

                CellObject co = new CellObject(EnrollRecordTable.Rows.Count - 1, 0, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                CellStyle cs = new CellStyle();

                //  題號 Style：Times New Roman、12號字、垂直靠上，水平置中
                cs.SetRowHeight(16.5).SetFontName("Times New Roman").SetFontSize(12).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Top).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Center);
                dicCellStyles.Add(co, cs);

                co = new CellObject(EnrollRecordTable.Rows.Count - 1, 1, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                cs = new CellStyle();

                //  題目 Style：粗體、底線、標楷體、12號字、垂直靠上，水平靠左
                cs.SetFontBold(true).SetFontUnderline(true).SetFontName("標楷體").SetFontSize(12).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Top).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Left);
                dicCellStyles.Add(co, cs);

                IEnumerable<XElement> xAnswers = xElement.Descendants("Answer");
                int k = 0;
                foreach (XElement xElement_Answer in xAnswers)
                {
                    if (string.IsNullOrWhiteSpace(xElement_Answer.Value))
                        continue;

                    DataRow Row = EnrollRecordTable.NewRow();
                    k++;
                    Row["No"] = xElement.Attribute("No").Value + "-" + k;
                    Row["Content"] = HttpUtility.HtmlDecode(xElement_Answer.Value);

                    EnrollRecordTable.Rows.Add(Row);

                    co = new CellObject(EnrollRecordTable.Rows.Count - 1, 0, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                    cs = new CellStyle();

                    //  回答項次 Style：Times New Roman、11號字、垂直靠上，水平置中
                    cs.SetFontName("Times New Roman").SetFontSize(11).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Top).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Center);
                    dicCellStyles.Add(co, cs);

                    co = new CellObject(EnrollRecordTable.Rows.Count - 1, 1, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                    cs = new CellStyle();

                    //  回答內容 Style：新細明體、12號字、垂直靠上，水平靠左、自動調整列高
                    cs.SetFontName("新細明體").SetFontSize(12).SetFontVerticalAlignment(CellStyle.VerticalAlignment.Top).SetFontHorizontalAlignment(CellStyle.HorizontalAlignment.Left).SetAutoFitRow(true).Merge(1, 8);
                    dicCellStyles.Add(co, cs);
                }

                //  空一行
                pRow = EnrollRecordTable.NewRow();
                EnrollRecordTable.Rows.Add(pRow);

                co = new CellObject(EnrollRecordTable.Rows.Count - 1, 0, EnrollRecordTable.TableName, "DataSection", WorksheetName);
                cs = new CellStyle();
                cs.SetRowHeight(16.5);
                dicCellStyles.Add(co, cs);
            }
        }

        private void MakeTableColumn(DataTable EnrollRecordTable)
        {
            EnrollRecordTable.Columns.Add("No");
            EnrollRecordTable.Columns.Add("Content");
            EnrollRecordTable.Columns.Add("Option1AnswerCount");
            EnrollRecordTable.Columns.Add("Option2AnswerCount");
            EnrollRecordTable.Columns.Add("Option3AnswerCount");
            EnrollRecordTable.Columns.Add("Option4AnswerCount");
            EnrollRecordTable.Columns.Add("Option5AnswerCount");
            EnrollRecordTable.Columns.Add("Option6AnswerCount");
            EnrollRecordTable.Columns.Add("Score");
        }

        private Workbook MakeExcelDocument(UDT.TeacherStatistics Statistic, Dictionary<string, UDT.Hierarchy> dicQuestionHierarchies)
        {
            Dictionary<UDT.Hierarchy, List<XElement>> dicHierarchyQuestions = new Dictionary<UDT.Hierarchy, List<XElement>>();
            List<UDT.StatisticsGroup> StatisticsGroups = Access.Select<UDT.StatisticsGroup>();
            Dictionary<string, Dictionary<string, Color>> dicQuestionBackgroundColor = new Dictionary<string, Dictionary<string, Color>>();
            Dictionary<string, Dictionary<string, Color>> dicEvaluationBackgroundColor = new Dictionary<string, Dictionary<string, Color>>();
            StatisticsGroups.ForEach((x) =>
            {
                XDocument xxDocument = XDocument.Parse(x.DisplayOrderList, LoadOptions.None);
                List<XElement> xElements = xxDocument.Descendants("Question").ToList();
                if (!string.IsNullOrEmpty(x.QuestionBgColor) && xElements.Count() > 0)
                {
                    foreach (string display_order in xElements.Select(y => y.Attribute("DisplayOrder").Value))
                    {
                        if (!dicQuestionBackgroundColor.ContainsKey(x.SurveyID.ToString()))
                            dicQuestionBackgroundColor.Add(x.SurveyID.ToString(), new Dictionary<string, Color>());

                        if (!dicQuestionBackgroundColor[x.SurveyID.ToString()].ContainsKey(display_order))
                            dicQuestionBackgroundColor[x.SurveyID.ToString()].Add(display_order, Color.FromName(x.QuestionBgColor));
                        else
                        {
                            if (Color.FromName(x.QuestionBgColor) != Color.White)
                                dicQuestionBackgroundColor[x.SurveyID.ToString()][display_order] = Color.FromName(x.QuestionBgColor);
                        }
                    }
                }
                if (!dicEvaluationBackgroundColor.ContainsKey(x.SurveyID.ToString()))
                    dicEvaluationBackgroundColor.Add(x.SurveyID.ToString(), new Dictionary<string, Color>());

                if (!dicEvaluationBackgroundColor[x.SurveyID.ToString()].ContainsKey(x.Name))
                    dicEvaluationBackgroundColor[x.SurveyID.ToString()].Add(x.Name, Color.FromName(x.EvaluationBgColor));
            });
            
            XDocument xDocument = XDocument.Parse(Statistic.StatisticsList, LoadOptions.None);
            XElement xStatistics = xDocument.Element("Statistics");

            #region 表頭

            DataSet dataSet_PageHeader = new DataSet("PageHeader");

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

            dataSet_PageHeader.Tables.Add(CSAttendCount.ToDataTable("CSAttendCount", "CSAttendCount"));
            dataSet_PageHeader.Tables.Add(FeedBackCount.ToDataTable("FeedBackCount", "FeedBackCount"));
            dataSet_PageHeader.Tables.Add(TeacherName.ToDataTable("TeacherName", "TeacherName"));
            dataSet_PageHeader.Tables.Add(CourseName.ToDataTable("CourseName", "CourseName"));
            dataSet_PageHeader.Tables.Add(SubjectName.ToDataTable("SubjectName", "SubjectName"));
            dataSet_PageHeader.Tables.Add(ClassName.ToDataTable("ClassName", "ClassName"));
            dataSet_PageHeader.Tables.Add(SubjectCode.ToDataTable("SubjectCode", "SubjectCode"));
            dataSet_PageHeader.Tables.Add(NewSubjectCode.ToDataTable("NewSubjectCode", "NewSubjectCode"));
            dataSet_PageHeader.Tables.Add(SchoolYear.ToDataTable("SchoolYear", "SchoolYear"));
            dataSet_PageHeader.Tables.Add(DataItems.SemesterItem.GetSemesterByCode(xStatistics.Attribute("Semester").Value).Name.ToDataTable("Semester", "Semester"));
            dataSet_PageHeader.Tables.Add(SurveyDate.ToDataTable("SurveyDate", "SurveyDate"));

            #endregion

            #region 群組化題目並排序

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
            foreach(KeyValuePair<UDT.Hierarchy, List<XElement>> kv in dicHierarchyQuestions.OrderBy(x=>x.Key.DisplayOrder))
            {
                if (!dicOrderedHierarchyQuestions.ContainsKey(kv.Key))
                    dicOrderedHierarchyQuestions.Add(kv.Key, new List<XElement>());

                dicOrderedHierarchyQuestions[kv.Key].AddRange(kv.Value.OrderBy(x => int.Parse(x.Attribute("No").Value)));
            }

            #endregion

            string report_name = SchoolYear + "-" + Semester + "-" + CourseName + "-" + TeacherName;
            Dictionary<ReportHelper.CellObject, ReportHelper.CellStyle> dicCellStyles = new Dictionary<CellObject, CellStyle>();

            #region 報表資料的部份

            //  先產生資料容器
            DataTable EnrollRecordTable = new DataTable("NoneSelfAssessmentContent");
            this.MakeTableColumn(EnrollRecordTable);

            //  接著將資料倒進容器中
            foreach (UDT.Hierarchy Hierarchy in dicOrderedHierarchyQuestions.Keys)
            {
                //  群組首
                this.MakeGroupData(EnrollRecordTable, Hierarchy, report_name, dicCellStyles);

                //  題目及答案
                this.MakeDetailOneData(EnrollRecordTable, Hierarchy, dicOrderedHierarchyQuestions[Hierarchy], report_name, dicCellStyles);
                this.MakeDetailTwoData(EnrollRecordTable, Hierarchy, dicOrderedHierarchyQuestions[Hierarchy], xDocument.Descendants("StatisticsGroup"), report_name, dicCellStyles, dicQuestionBackgroundColor, dicEvaluationBackgroundColor, SurveyID);
                this.MakeDetailThreeData(EnrollRecordTable, Hierarchy, dicOrderedHierarchyQuestions[Hierarchy], report_name, dicCellStyles);
                this.MakeDetailFourData(EnrollRecordTable, Hierarchy, dicOrderedHierarchyQuestions[Hierarchy], report_name, dicCellStyles);
            }

            #endregion

            DataSet dataSet_DataSection = new DataSet("DataSection");
            dataSet_DataSection.Tables.Add(EnrollRecordTable);

            Dictionary<string, List<DataSet>> dicDataSources = new Dictionary<string, List<DataSet>>();
            dicDataSources.Add(report_name, new List<DataSet>() { dataSet_PageHeader, dataSet_DataSection });
            MemoryStream ms = new MemoryStream(Properties.Resources.新版_教學意見表);
            Workbook wb = Report.Produce(dicDataSources, ms, false, dicCellStyles);

            return wb;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (this.dgvData.SelectedRows.Count == 0)
            {
                MessageBox.Show("請先選擇列印資料。");
                return;
            }
            this.circularProgress.Visible = true;
            this.circularProgress.IsRunning = true;
            this.btnQuery.Enabled = false;
            this.btnExport.Enabled = false;
            this.btnExit.Enabled = false;
            string file_name = string.Empty;
            int option = 3;
            if (this.radioSubject.Checked)
                option = 1;
            if (this.radioCourse.Checked)
                option = 2;
            Workbook wb;
            Dictionary<string, List<UDT.TeacherStatistics>> dicStatistics = new Dictionary<string, List<UDT.TeacherStatistics>>();
            Dictionary<string, Workbook> dicFiles = new Dictionary<string, Workbook>();
            List<UDT.TeacherStatistics> Statistics = new List<UDT.TeacherStatistics>();
            this.dgvData.SelectedRows.Cast<DataGridViewRow>().ToList().ForEach(x => Statistics.Add(x.Tag as UDT.TeacherStatistics));

            Task task = Task.Factory.StartNew(() =>
            {
                List<UDT.TeacherStatistics> SelectedStatistics = new List<UDT.TeacherStatistics>();
                List<UDT.QHRelation> QHRelations = Access.Select<UDT.QHRelation>();
                List<UDT.Hierarchy> Hierarchies = Access.Select<UDT.Hierarchy>();
                Dictionary<string, UDT.Hierarchy> dicHierarchies = new Dictionary<string, UDT.Hierarchy>();
                if (Hierarchies.Count > 0)
                    dicHierarchies = Hierarchies.ToDictionary(x => x.Title);
                Dictionary<string, UDT.Hierarchy> dicQuestionHierarchies = new Dictionary<string, UDT.Hierarchy>();
                foreach (UDT.QHRelation QHRelation in QHRelations)
                {
                    if (dicHierarchies.ContainsKey(QHRelation.HierarchyTitle))
                        dicQuestionHierarchies.Add(QHRelation.QuestionID.ToString(), dicHierarchies[QHRelation.HierarchyTitle]);
                }
                //key = SchoolYear + "-" + Semester + "-" + SubjectName;
                wb = new Workbook();
                Statistics.ForEach((x) =>
                {
                    XDocument xDocument = XDocument.Parse(x.StatisticsList, LoadOptions.None);
                    XElement xStatistics = xDocument.Element("Statistics");

                    string TeacherName = HttpUtility.HtmlDecode(xStatistics.Attribute("TeacherName").Value);            // 授課教師
                    string CourseName = HttpUtility.HtmlDecode(xStatistics.Attribute("CourseName").Value);              //  開課
                    string SubjectName = HttpUtility.HtmlDecode(xStatistics.Attribute("SubjectName").Value);            //   課程
                    string SchoolYear = xStatistics.Attribute("SchoolYear").Value;                   //   學年度
                    string Semester = xStatistics.Attribute("Semester").Value;                         //    學期

                    if (option == 1)
                        file_name = SubjectName + "-課程教學評鑑統計表";
                    if (option == 2)
                        file_name = CourseName + "-開課教學評鑑統計表";
                    if (option == 3)
                        file_name = TeacherName + "-授課教師教學評鑑統計表";

                    if (!dicStatistics.ContainsKey(file_name))
                        dicStatistics.Add(file_name, new List<UDT.TeacherStatistics>());

                    dicStatistics[file_name].Add(x);
                });
                foreach (string key in dicStatistics.Keys)
                {
                    Workbook new_workbook = new Workbook();
                    //new_workbook.Worksheets.Cast<Worksheet>().ToList().ForEach(x => new_workbook.Worksheets.RemoveAt(x.Index));
                    foreach (UDT.TeacherStatistics Statistic in dicStatistics[key])
                    {
                        wb = MakeExcelDocument(Statistic, dicQuestionHierarchies);
                        new_workbook.Combine(wb);
                        wb.Worksheets.Cast<Worksheet>().ToList().ForEach(x => wb.Worksheets.RemoveAt(x.Index));
                    }
                    new_workbook.Worksheets.Cast<Worksheet>().ToList().ForEach((x) => 
                    {
                        if (x.Cells.MaxDataColumn == 0 && x.Cells.MaxDataRow == 0)
                            new_workbook.Worksheets.RemoveAt(x.Index);
                    });
                    dicFiles.Add(key, new_workbook);
                }
            });

            task.ContinueWith((x) =>
            {
                this.circularProgress.IsRunning = false;
                this.circularProgress.Visible = false;
                this.btnQuery.Enabled = true;
                this.btnExport.Enabled = true;
                this.btnExit.Enabled = true;

                if (x.Exception != null)
                {
                    MessageBox.Show(x.Exception.InnerException.Message);
                    return;
                }

                string filePath = string.Empty;

                System.Windows.Forms.FolderBrowserDialog folder = new FolderBrowserDialog();
                do
                {
                    DialogResult dr = folder.ShowDialog();
                    if (dr == DialogResult.OK)
                        filePath = folder.SelectedPath;
                    if (dr == DialogResult.Cancel)
                        return;
                } while (!System.IO.Directory.Exists(filePath));

                foreach (string fileName in dicFiles.Keys)
                {
                    try
                    {
                        //  檔案名稱不能有下列字元<>:"/\|?*
                        string new_fileName = fileName.Replace("：", "꞉").Replace(":", "꞉").Replace("/", "⁄").Replace("／", "⁄").Replace(@"\", "∖").Replace("＼", "∖").Replace("?", "_").Replace("？", "_").Replace("*", "✻").Replace("＊", "✻").Replace("<", "〈").Replace("＜", "〈").Replace(">", "〉").Replace("＞", "〉").Replace("\"", "''").Replace("”", "''").Replace("|", "ㅣ").Replace("｜", "ㅣ");
                        dicFiles[fileName].Save(Path.Combine(filePath, new_fileName + ".xls"), FileFormatType.Excel2003);
                        System.Diagnostics.Process.Start(filePath);
                    }
                    catch
                    {
                        MessageBox.Show("指定路徑無法存取。", "建立檔案失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void nudSchoolYear_ValueChanged(object sender, EventArgs e)
        {
            if (!this.form_loaded)
                return;

            int school_year = int.Parse(this.nudSchoolYear.Value + "");
            int school_year1 = int.Parse(this.nudSchoolYear1.Value + "");
            int semester = int.Parse((this.cboSemester.SelectedItem as DataItems.SemesterItem).Value);
            int semester1 = int.Parse((this.cboSemester1.SelectedItem as DataItems.SemesterItem).Value);

            this.InitCourse(school_year, school_year1, semester, semester1);
            this.InitSubject(school_year, school_year1, semester, semester1);
            this.InitTeacher(school_year, school_year1, semester, semester1);
        }

        private void cboSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.form_loaded)
                return;

            int school_year = int.Parse(this.nudSchoolYear.Value + "");
            int school_year1 = int.Parse(this.nudSchoolYear1.Value + "");
            int semester = int.Parse((this.cboSemester.SelectedItem as DataItems.SemesterItem).Value);
            int semester1 = int.Parse((this.cboSemester1.SelectedItem as DataItems.SemesterItem).Value);

            this.InitCourse(school_year, school_year1, semester, semester1);
            this.InitSubject(school_year, school_year1, semester, semester1);
            this.InitTeacher(school_year, school_year1, semester, semester1);
        }

        private MemoryStream GetSurveyTemplate(int SurveyID)
        {
            List<UDT.ReportTemplate> ReportTemplates = this.Access.Select<UDT.ReportTemplate>(string.Format("ref_survey_id = {0}", SurveyID));

            byte[] _buffer = Convert.FromBase64String(ReportTemplates.ElementAt(0).Template);
            MemoryStream template = new MemoryStream(_buffer);

            return template;
        }

        private void GetStatisticsHeader()
        {

        }

        private List<DataBindedSheet> GetDataBindedSheets(StudentRecord Student)
        {
            Workbook wb = new Workbook();
            MemoryStream ms = new MemoryStream();   // (Properties.Resources.EMBA_歷年成績表_樣版);
            wb.Open(ms);

            List<DataBindedSheet> DataBindedSheets = new List<DataBindedSheet>();

            DataBindedSheet DataBindedSheet = new DataBindedSheet();
            DataBindedSheet.Worksheet = wb.Worksheets["PageHeader"];
            DataBindedSheet.DataTables = new List<DataTable>();
            DataBindedSheet.DataTables.Add(Student.StudentNumber.ToDataTable("學號", "學號"));
            DataBindedSheet.DataTables.Add(Student.Name.ToDataTable("姓名", "姓名"));
            DataBindedSheet.DataTables.Add(Student.Gender.ToDataTable("性別", "性別"));

            if (!Student.Birthday.HasValue)
            {
                DataBindedSheet.DataTables.Add("".ToDataTable("出生日期", "出生日期"));
            }
            else
            {
                DateTime birthday;
                if (DateTime.TryParse(Student.Birthday.Value + "", out birthday))
                    DataBindedSheet.DataTables.Add(birthday.ToString("yyyy/MM/dd").ToDataTable("出生日期", "出生日期"));
                else
                    DataBindedSheet.DataTables.Add("".ToDataTable("出生日期", "出生日期"));
            }

            DataBindedSheet.DataTables.Add(StudentBrief2.EnrollYear.ToDataTable("入學年度", "入學年度"));
            DataBindedSheet.DataTables.Add(DepartmentGroup.Name.ToDataTable("系所組別", "系所組別"));
            DataBindedSheets.Add(DataBindedSheet);

            Dictionary<string, KeyValuePair> dicSubjectGroupGredits = new Dictionary<string, KeyValuePair>();
            if (this.dicSubjectSemesterScores.ContainsKey(Student.ID))
            {
                List<UDT.SubjectSemesterScore> SubjectSemesterScores = this.dicSubjectSemesterScores[Student.ID];
                SubjectSemesterScores.ForEach((x) =>
                {
                    if (string.IsNullOrEmpty(x.NewSubjectCode))
                    {
                        if (this.dicSubjects.ContainsKey(x.SubjectID.ToString()))
                            x.NewSubjectCode = this.dicSubjects[x.SubjectID.ToString()].NewSubjectCode;
                    }
                });
                SubjectSemesterScores = SubjectSemesterScores.OrderBy(x => x.SchoolYear.HasValue ? x.SchoolYear.Value : 0).ThenBy(x => x.Semester.HasValue ? x.Semester.Value : 0).ThenBy(x => x.NewSubjectCode).ToList();

                int credit_total = 0;
                foreach (UDT.SubjectSemesterScore SubjectSemesterScore in SubjectSemesterScores)
                {
                    DataBindedSheet = new DataBindedSheet();
                    DataBindedSheet.Worksheet = wb.Worksheets["DataSection"];
                    DataBindedSheet.DataTables = new List<DataTable>();
                    DataBindedSheet.DataTables.Add((SubjectSemesterScore.SchoolYear.HasValue ? SubjectSemesterScore.SchoolYear.Value + "" : "").ToDataTable("學年度", "學年度"));
                    DataBindedSheet.DataTables.Add((SubjectSemesterScore.Semester.HasValue ? SubjectSemesterScore.Semester.Value + "" : "").ToDataTable("學期", "學期"));
                    DataBindedSheet.DataTables.Add(SubjectSemesterScore.NewSubjectCode.ToDataTable("課號", "課號"));

                    DataBindedSheet.DataTables.Add(SubjectSemesterScore.SubjectCode.ToDataTable("課程識別碼", "課程識別碼"));
                    DataBindedSheet.DataTables.Add(SubjectSemesterScore.SubjectName.ToDataTable("課程名稱", "課程名稱"));
                    DataBindedSheet.DataTables.Add(SubjectSemesterScore.Score.ToDataTable("成績", "成績"));
                    DataBindedSheet.DataTables.Add(SubjectSemesterScore.Credit.ToDataTable("學分數", "學分數"));

                    if (dicGraduationSubjectLists.ContainsKey(SubjectSemesterScore.SubjectID))
                    {
                        string SubjectGroup = dicGraduationSubjectLists[SubjectSemesterScore.SubjectID].SubjectGroup;
                        DataBindedSheet.DataTables.Add(SubjectGroup.ToDataTable("群組別", "群組別"));
                        if (!dicSubjectGroupGredits.ContainsKey(SubjectGroup))
                            dicSubjectGroupGredits.Add(SubjectGroup, new KeyValuePair());

                        if (dicGraduationSubjectGroups.ContainsKey(SubjectGroup))
                            dicSubjectGroupGredits[SubjectGroup].Key = dicGraduationSubjectGroups[SubjectGroup].LowestCredit;
                        if (SubjectSemesterScore.IsPass)
                        {
                            if (dicSubjectGroupGredits[SubjectGroup].Value == null)
                                dicSubjectGroupGredits[SubjectGroup].Value = SubjectSemesterScore.Credit;
                            else
                                dicSubjectGroupGredits[SubjectGroup].Value += SubjectSemesterScore.Credit;
                        }
                    }
                    else
                        DataBindedSheet.DataTables.Add("".ToDataTable("群組別", "群組別"));

                    if (!string.IsNullOrEmpty(SubjectSemesterScore.OffsetCourse) || SubjectSemesterScore.IsPass)
                    {
                        DataBindedSheet.DataTables.Add("已取得學分".ToDataTable("備註", "備註"));
                    }
                    else
                        DataBindedSheet.DataTables.Add("未取得學分".ToDataTable("備註", "備註"));

                    if (SubjectSemesterScore.IsPass && string.IsNullOrEmpty(SubjectSemesterScore.OffsetCourse))
                        credit_total += SubjectSemesterScore.Credit;

                    DataBindedSheets.Add(DataBindedSheet);
                }

                int offset_credit = SubjectSemesterScores.Where(x => !string.IsNullOrWhiteSpace(x.OffsetCourse)).Sum(x => x.Credit);
                DataBindedSheet = new DataBindedSheet();
                DataBindedSheet.Worksheet = wb.Worksheets["PageFooter-Header"];
                DataBindedSheet.DataTables = new List<DataTable>();
                DataBindedSheet.DataTables.Add(credit_total.ToDataTable("修習及格學分", "修習及格學分"));
                DataBindedSheet.DataTables.Add(offset_credit.ToDataTable("抵免學分", "抵免學分"));
                DataBindedSheet.DataTables.Add((credit_total + offset_credit).ToDataTable("實得總學分", "實得總學分"));
                DataBindedSheets.Add(DataBindedSheet);

                List<UDT.GraduationSubjectGroupRequirement> GraduationSubjectGroupRequirements = dicGraduationSubjectGroups.Values.ToList();
                GraduationSubjectGroupRequirements.Sort(
                delegate(UDT.GraduationSubjectGroupRequirement x, UDT.GraduationSubjectGroupRequirement y)
                {
                    int index_x = SubjectGroups_Sort.IndexOf(x.SubjectGroup);
                    int index_y = SubjectGroups_Sort.IndexOf(y.SubjectGroup);
                    if (index_x < 0)
                        index_x = int.MaxValue;
                    if (index_y < 0)
                        index_y = int.MaxValue;

                    return index_x.CompareTo(index_y);
                });

                foreach (UDT.GraduationSubjectGroupRequirement GraduationSubjectGroupRequirement in GraduationSubjectGroupRequirements)
                {
                    string SubjectGroup = GraduationSubjectGroupRequirement.SubjectGroup;
                    DataBindedSheet = new DataBindedSheet();
                    DataBindedSheet.Worksheet = wb.Worksheets["PageFooter-DataSection"];
                    DataBindedSheet.DataTables = new List<DataTable>();
                    DataBindedSheet.DataTables.Add(SubjectGroup.ToDataTable("群組別", "群組別"));

                    int gCredit = 0;

                    if (dicSubjectGroupGredits.ContainsKey(SubjectGroup))
                        if (dicSubjectGroupGredits[SubjectGroup].Value.HasValue)
                            gCredit = dicSubjectGroupGredits[SubjectGroup].Value.Value;

                    DataBindedSheet.DataTables.Add((gCredit + "").ToDataTable("已取得學分數", "已取得學分數"));

                    int sCredit = GraduationSubjectGroupRequirement.LowestCredit;
                    if (dicSubjectGroupGredits.ContainsKey(SubjectGroup))
                    {
                        if (sCredit > gCredit)
                            DataBindedSheet.DataTables.Add((sCredit - gCredit).ToDataTable("不足學分數", "不足學分數"));
                        else
                            DataBindedSheet.DataTables.Add("0".ToDataTable("不足學分數", "不足學分數"));
                    }
                    else
                    {
                        DataBindedSheet.DataTables.Add(sCredit.ToDataTable("不足學分數", "不足學分數"));
                    }
                    DataBindedSheet.DataTables.Add(sCredit.ToDataTable("應修學分數", "應修學分數"));

                    DataBindedSheets.Add(DataBindedSheet);
                }

                DataBindedSheet = new DataBindedSheet();
                DataBindedSheet.Worksheet = wb.Worksheets["PageFooter-Footer"];
                DataBindedSheet.DataTables = new List<DataTable>();
                DataBindedSheets.Add(DataBindedSheet);
            }

            return DataBindedSheets;
        }

        private Workbook GenerateWorkbook(StudentRecord Student)
        {
            Workbook workbook = new Workbook();
            workbook.Worksheets.Cast<Worksheet>().ToList().ForEach(x => workbook.Worksheets.RemoveAt(x.Name));

            List<DataBindedSheet> TemplateSheets = this.GetDataBindedSheets(Student);

            int instanceSheetIndex = workbook.Worksheets.Add();
            workbook.Worksheets[instanceSheetIndex].Name = Student.StudentNumber + "-" + Student.Name;
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
                    dataSet = new DataSet();
                    dataSet.Tables.AddRange(sheet.DataTables.ToArray());
                    i = instanceSheet.Cells.MaxRow + 1;
                    DocumentHelper.CloneTemplate(instanceSheet, sheet.Worksheet, i);
                    DocumentHelper.GenerateSheet(dataSet, instanceSheet, i, null);
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

        private void btnSubjectSemesterScoreHistory_Click(object sender, EventArgs e)
        {
            this.circularProgress.Visible = true;
            this.circularProgress.IsRunning = true;
            this.btnPrint.Enabled = false;
            Task<Workbook> task = Task<Workbook>.Factory.StartNew(() =>
            {
                Workbook new_workbook = new Workbook();
                foreach (string key in this.dicStudents.Keys)
                {
                    Workbook wb = this.GenerateWorkbook(this.dicStudents[key]);
                    new_workbook.Combine(wb);
                    new_workbook.Worksheets.Cast<Worksheet>().ToList().ForEach((x) =>
                    {
                        if (x.Cells.MaxDataColumn == 0 && x.Cells.MaxDataRow == 0)
                            new_workbook.Worksheets.RemoveAt(x.Index);
                    });
                }
                return new_workbook;
            });

            task.ContinueWith((x) =>
            {
                this.circularProgress.Visible = false;
                this.circularProgress.IsRunning = false;
                this.btnPrint.Enabled = true;
                if (x.Exception != null)
                {
                    MessageBox.Show(x.Exception.InnerException.Message);
                    return;
                }

                System.Windows.Forms.SaveFileDialog sd = new System.Windows.Forms.SaveFileDialog();
                sd.Title = "另存新檔";
                sd.FileName = "歷年成績表" + DateTime.Now.ToString(" yyyy-MM-dd_HH_mm_ss") + ".xls";
                sd.Filter = "Excel檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
                sd.AddExtension = true;
                if (sd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        x.Result.Save(sd.FileName, FileFormatType.Excel2003);
                        System.Diagnostics.Process.Start(sd.FileName);
                    }
                    catch
                    {
                        FISCA.Presentation.Controls.MsgBox.Show("指定路徑無法存取。", "建立檔案失敗", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return;
                    }
                }
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }

    public class DataBindedSheet
    {
        public Worksheet Worksheet { get; set; }
        public List<DataTable> DataTables { get; set; }
    }

    public class KeyValuePair
    {
        public int? Key { get; set; }
        public int? Value { get; set; }
    }

}