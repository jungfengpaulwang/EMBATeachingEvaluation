using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aspose.Cells;
using FISCA.Data;
using FISCA.Presentation.Controls;
using FISCA.UDT;
using K12.Data;
using System.Xml.Linq;
using System.Text;
using ReportHelper;
using System.IO;
using System.Drawing;
using System.Web;

namespace TeachingEvaluation.Export
{
    public partial class ExportEvaluation_ComplexQueries : BaseForm
    {
        private AccessHelper Access;
        private QueryHelper Query;

        private List<CourseRecord> Courses;
        private List<dynamic> Teachers;
        private List<dynamic> Subjects;
        private List<UDT.TeacherStatistics> Statistics;

        private bool form_loaded;

        /// <summary>
        /// 報表歸檔方式
        /// 1：一門課程一個檔案
        /// 2：一門開課一個檔案
        /// 3：一位教師一個檔案
        /// </summary>
        public int FileType { get; set; }

        /// <summary>
        /// 使用者選取之教學意見統計結果
        /// </summary>
        public List<UDT.TeacherStatistics> SelectedStatistics { get; set; }

        public ExportEvaluation_ComplexQueries()
        {
            InitializeComponent();

            this.InitSchoolYear();
            this.InitSemester();

            this.Courses = new List<CourseRecord>();
            this.Statistics = new List<UDT.TeacherStatistics>();
            this.SelectedStatistics = new List<UDT.TeacherStatistics>();

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
            this.DialogResult = System.Windows.Forms.DialogResult.None;
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

            this.FileType = 3;
            if (this.radioSubject.Checked) this.FileType = 1;
            if (this.radioCourse.Checked) this.FileType = 2;

            this.SelectedStatistics.Clear();
            this.dgvData.SelectedRows.Cast<DataGridViewRow>().ToList().ForEach(x => this.SelectedStatistics.Add(x.Tag as UDT.TeacherStatistics));

            //  驗證評鑑樣版是否對應教學意見調查表樣版
            Task<Dictionary<string, Workbook>> task = Task<Dictionary<string, Workbook>>.Factory.StartNew(() =>
            {
                DataTable dataTable = Query.Select(@"select aSurvey.ref_course_id, aSurvey.ref_teacher_id, survey.name as survey_name, rt.ref_survey_id as survey_id from $ischool.emba.teaching_evaluation.survey as survey 
join $ischool.emba.teaching_evaluation.assigned_survey as aSurvey on survey.uid=aSurvey.ref_survey_id
left join $ischool.emba.teaching_evaluation.report_template as rt on aSurvey.ref_survey_id=rt.ref_survey_id");
                if (dataTable.Rows.Count == 0)
                    throw new Exception("請先設定「教學意見表樣版」。");
                Dictionary<string, Dictionary<string, string>> dicReportTemplates = new Dictionary<string, Dictionary<string, string>>();
                dataTable.Rows.Cast<DataRow>().ToList().ForEach(x =>
                {
                    string key = x["ref_course_id"] + "-" + x["ref_teacher_id"];
                    string survey_id = x["survey_id"] + "";
                    string survey_name = x["survey_name"] + "";

                    if (!dicReportTemplates.ContainsKey(key))
                        dicReportTemplates.Add(key, new Dictionary<string, string>());

                    dicReportTemplates[key].Add(survey_id, survey_name);
                });

                List<string> Empty_Survey = new List<string>();
                this.SelectedStatistics.ForEach((x) =>
                {
                    string key = x.CourseID + "-" + x.TeacherID;
                    if (String.IsNullOrEmpty(dicReportTemplates[key].ElementAt(0).Key))
                        Empty_Survey.Add(dicReportTemplates[key].ElementAt(0).Value);
                });
                if (Empty_Survey.Count > 0)
                {
                    throw new Exception(string.Format("請先設定下列評鑑之教學意見表樣版：\n\n{0}", string.Join("\n", Empty_Survey.Distinct())));
                }
                
                //  統計群組、背景顏色
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

                    if (!dicEvaluationBackgroundColor[x.SurveyID.ToString()].ContainsKey(HttpUtility.HtmlDecode(x.Name)))
                        dicEvaluationBackgroundColor[x.SurveyID.ToString()].Add(HttpUtility.HtmlDecode(x.Name), Color.FromName(x.EvaluationBgColor));
                });

                List<UDT.QHRelation> QHRelations = Access.Select<UDT.QHRelation>();
                List<UDT.Hierarchy> Hierarchies = Access.Select<UDT.Hierarchy>();
                Dictionary<string, Workbook> dicSurveyIDs = new Dictionary<string, Workbook>();
                Dictionary<string, List<Workbook>> dicWorkbooks = new Dictionary<string, List<Workbook>>();

                this.SelectedStatistics.ForEach((x) =>
                {
                    XDocument xDocument = XDocument.Parse(x.StatisticsList, LoadOptions.None);
                    XElement xStatistics = xDocument.Element("Statistics");
                    string SurveyID = xStatistics.Attribute("SurveyID").Value;

                    if (!dicSurveyIDs.ContainsKey(SurveyID))
                        dicSurveyIDs.Add(SurveyID, this.GetSurveyTemplate(SurveyID));
                    ExcelDocumentMaker excelDocumentMaker = new ExcelDocumentMaker(dicEvaluationBackgroundColor, dicQuestionBackgroundColor, QHRelations, Hierarchies, x, dicSurveyIDs[SurveyID]);
                    Workbook wb = excelDocumentMaker.Produce();
                    if (wb != null)
                    {
                        string key = string.Empty;
                        if (this.FileType == 1)
                            key = excelDocumentMaker.SubjectName + "-課程教學評鑑統計表";
                        if (this.FileType == 2)
                            key = excelDocumentMaker.CourseName + "-開課教學評鑑統計表";
                        if (this.FileType == 3)
                            key = excelDocumentMaker.TeacherName + "-授課教師教學評鑑統計表";

                        if (!dicWorkbooks.ContainsKey(key))
                            dicWorkbooks.Add(key, new List<Workbook>());

                        dicWorkbooks[key].Add(wb);
                    }
                });

                Dictionary<string, Workbook> dicFiles = new Dictionary<string, Workbook>();
                try
                {
                    foreach (string key in dicWorkbooks.Keys)
                    {
                        Workbook new_workbook = new Workbook();
                        foreach (Workbook wb in dicWorkbooks[key])
                        {
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
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                if (dicFiles.Count == 0)
                    throw new Exception("無檔案產生。");

                return dicFiles;
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

                foreach (string fileName in x.Result.Keys)
                {
                    try
                    {
                        //  檔案名稱不能有下列字元<>:"/\|?*
                        string new_fileName = fileName.Replace("：", "꞉").Replace(":", "꞉").Replace("/", "⁄").Replace("／", "⁄").Replace(@"\", "∖").Replace("＼", "∖").Replace("?", "_").Replace("？", "_").Replace("*", "✻").Replace("＊", "✻").Replace("<", "〈").Replace("＜", "〈").Replace(">", "〉").Replace("＞", "〉").Replace("\"", "''").Replace("”", "''").Replace("|", "ㅣ").Replace("｜", "ㅣ");
                        x.Result[fileName].Save(Path.Combine(filePath, new_fileName + ".xls"), FileFormatType.Excel2003);
                        System.Diagnostics.Process.Start(filePath);
                    }
                    catch
                    {
                        MessageBox.Show("指定路徑無法存取。", "建立檔案失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private Workbook GetSurveyTemplate(string SurveyID)
        {
            List<UDT.ReportTemplate> ReportTemplates = this.Access.Select<UDT.ReportTemplate>(string.Format("ref_survey_id = {0}", SurveyID));

            byte[] _buffer = Convert.FromBase64String(ReportTemplates.ElementAt(0).Template);
            MemoryStream template = new MemoryStream(_buffer);

            Workbook wb = new Workbook();
            wb.Open(template);

            return wb;
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
    }
}