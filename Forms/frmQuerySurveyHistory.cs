using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using FISCA.UDT;
using DevComponents.Editors;
using FISCA.Data;
using System.Threading.Tasks;
using System.Dynamic;
using Aspose.Cells;

namespace TeachingEvaluation.Forms
{
    public partial class frmQuerySurveyHistory : BaseForm
    {
        private AccessHelper Access;
        private QueryHelper Query;

        private DataTable dataTable_SurveyHistory;
        private Dictionary<string, SurveyHistory> dicAchievingInfos;
        private Dictionary<string, decimal> dicAchievingRates;
        private Dictionary<string, Course> dicCourses;

        private bool form_loaded;

        public frmQuerySurveyHistory()
        {
            InitializeComponent();

            Access = new AccessHelper();
            Query = new QueryHelper();

            dataTable_SurveyHistory = new DataTable();
            dicAchievingInfos = new Dictionary<string, SurveyHistory>();
            dicAchievingRates = new Dictionary<string, decimal>();
            dicCourses = new Dictionary<string, Course>();

            this.dgvDataSurveyHistory.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvData_MouseClick);
            this.dgvDataAchievingRate.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvData_MouseClick);            

            this.Load += new EventHandler(Form_Load);
        }

        private void dgvData_MouseClick(object sender, MouseEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            DataGridView.HitTestInfo hit = dgv.HitTest(e.X, e.Y);

            if (hit.Type == DataGridViewHitTestType.TopLeftHeader)
            {
                dgv.SelectAll();
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            form_loaded = false;

            this.InitSchoolYearAndSemester();
            this.InitDataSource(new List<Action>(){ this.DGV_DataBinding, this.InitCourseDataSource });

            form_loaded = true;
        }

        private void InitCourseDataSource()
        {
            int school_year = int.Parse(this.nudSchoolYear.Value + "");
            int semester = int.Parse((this.cboSemester.SelectedItem as TeachingEvaluation.DataItems.SemesterItem).Value);

            int school_year1 = int.Parse(this.nudSchoolYear1.Value + "");
            int semester1 = int.Parse((this.cboSemester1.SelectedItem as TeachingEvaluation.DataItems.SemesterItem).Value);

            string oCourseName = this.cboCourse.Text.Trim();
            this.cboCourse.Items.Clear();
            this.cboCourse.Items.Add(string.Empty);
            List<string> Courses = new List<string>();
            foreach (Course course in this.dicCourses.Values)
            {
                if (course.SchoolYear < school_year || course.SchoolYear > school_year1)
                    continue;
                if (course.SchoolYear == school_year && course.Semester < semester)
                    continue;
                if (course.SchoolYear == school_year1 && course.Semester > semester1)
                    continue;

                if (!Courses.Contains(course.CourseName))
                {
                    Courses.Add(course.CourseName);
                    this.cboCourse.Items.Add(course.CourseName);
                }
            }
            if (Courses.Contains(oCourseName))
                this.cboCourse.Text = oCourseName;

            this.cboCourse.AutoCompleteMode = AutoCompleteMode.Suggest;
            this.cboCourse.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void InitDataSource(List<Action> Callbacks = null)
        {
            this.circularProgress.Visible = true;
            this.circularProgress.IsRunning = true;
            this.nudSchoolYear.Enabled = false;
            this.cboSemester.Enabled = false;
            this.nudSchoolYear1.Enabled = false;
            this.cboSemester1.Enabled = false;
            this.txtFilterSnum.Enabled = false;
            this.cboCourse.Enabled = false;
            this.Confirm.Enabled = false;

            Task task = Task.Factory.StartNew(() =>
            {
                #region  填答記錄
                this.dataTable_SurveyHistory = Query.Select(string.Format(@"select survey_info.student_id, survey_info.student_name, survey_info.student_number, survey_info.school_year, survey_info.semester, survey_info.course_id, survey_info.course_name, survey_info.teacher_id, survey_info.teacher_name, reply_info.ref_student_id, survey_info.end_time from 
(select course.school_year, course.semester, course.id as course_id, course.course_name, asurvey.ref_teacher_id as teacher_id, teacher.teacher_name, end_time, student.id as student_id, student_number, student.name as student_name from $ischool.emba.scattend_ext as se 
join course on course.id=se.ref_course_id 
join student on student.id=se.ref_student_id
join $ischool.emba.teaching_evaluation.assigned_survey as asurvey on asurvey.ref_course_id=course.id
join teacher on teacher.id=asurvey.ref_teacher_id) as survey_info
left join 
(select ref_student_id, ref_course_id, ref_teacher_id from $ischool.emba.teaching_evaluation.reply as reply where reply.status=1) as reply_info
on reply_info.ref_student_id=survey_info.student_id and reply_info.ref_course_id=survey_info.course_id and survey_info.teacher_id=reply_info.ref_teacher_id
order by school_year, semester, survey_info.student_id, survey_info.course_id"));
                #endregion

                #region 問卷達標百分比
                DataTable dataTable_AchievingRate = Query.Select(string.Format(@"select school_year, semester, rate from $ischool.emba.teaching_evaluation.achieving_rate"));
                foreach (DataRow row in dataTable_AchievingRate.Rows)
                {
                    string key = row["school_year"] + "-" + row["semester"];
                    if (!dicAchievingRates.ContainsKey(key))
                        dicAchievingRates.Add(key, decimal.Parse(row["rate"] + ""));
                }

                #endregion

                #region 問卷填答率

                foreach (DataRow row in dataTable_SurveyHistory.Rows)
                {
                    if (!this.dicCourses.ContainsKey(row["school_year"] + "-" + row["semester"] + "-" + row["course_name"]))
                    {
                        Course course = new Course();
                        course.SchoolYear = int.Parse(row["school_year"] + "");
                        course.Semester = int.Parse(row["semester"] + "");
                        course.CourseID = row["course_id"] + "";
                        course.CourseName = row["course_name"] + "";
                        this.dicCourses.Add(row["school_year"] + "-" + row["semester"] + "-" + row["course_name"], course);
                    }
                    string key = row["student_id"] + "-" + row["school_year"] + "-" + row["semester"];

                    if (!this.dicAchievingInfos.ContainsKey(key))
                        this.dicAchievingInfos.Add(key, new SurveyHistory());

                    this.dicAchievingInfos[key].StudentID = row["student_id"] + "";
                    this.dicAchievingInfos[key].StudentName = row["student_name"] + "";
                    this.dicAchievingInfos[key].StudentNumber = row["student_number"] + "";
                    this.dicAchievingInfos[key].SchoolYear = int.Parse(row["school_year"] + "");
                    this.dicAchievingInfos[key].Semester = int.Parse(row["semester"] + "");

                    if (!string.IsNullOrEmpty(row["ref_student_id"] + ""))
                    {
                        this.dicAchievingInfos[key].AnswerCount += 1;
                        if (!this.dicAchievingInfos[key].ReplyCourses.Contains(row["course_name"] + ""))
                            this.dicAchievingInfos[key].ReplyCourses.Add(row["course_name"] + "");

                        this.dicAchievingInfos[key].NoReplyCourses.Remove(row["course_name"] + "");
                    }
                    else
                    {
                        if (!this.dicAchievingInfos[key].NoReplyCourses.Contains(row["course_name"] + "") && !this.dicAchievingInfos[key].ReplyCourses.Contains(row["course_name"] + ""))
                            this.dicAchievingInfos[key].NoReplyCourses.Add(row["course_name"] + "");
                    }

                    this.dicAchievingInfos[key].SurveyCount += 1;
                }
                foreach (SurveyHistory sh in this.dicAchievingInfos.Values)
                {
                    string key = sh.SchoolYear + "-" + sh.Semester;
                    sh.Rate = Math.Ceiling(sh.AnswerCount * 100 / sh.SurveyCount);

                    if (dicAchievingRates.ContainsKey(key))
                    {
                        decimal rate = dicAchievingRates[key];

                        if (sh.Rate >= dicAchievingRates[key])
                        {
                            if (sh.NoReplyCourses.Count == 0)
                                sh.IsQualify = true;
                            else
                                sh.IsQualify = false;
                        }
                        else
                            sh.IsQualify = false;
                    }
                    else
                        sh.IsQualify = false;
                }

                #endregion
            });
            task.ContinueWith((x) =>
            {
                if (x.Exception != null)
                {
                    MessageBox.Show(x.Exception.InnerException.Message);
                    goto TheEnd;
                }

            TheEnd:
                this.circularProgress.IsRunning = false;
                this.circularProgress.Visible = false;
                this.nudSchoolYear.Enabled = true;
                this.cboSemester.Enabled = true;
                this.nudSchoolYear1.Enabled = true;
                this.cboSemester1.Enabled = true;
                this.txtFilterSnum.Enabled = true;
                this.cboCourse.Enabled = true;
                this.Confirm.Enabled = true;
                if (x.Exception == null && Callbacks != null)
                    Callbacks.ForEach(y=>y.Invoke());
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void InitSchoolYearAndSemester()
        {
            this.cboSemester.DataSource = TeachingEvaluation.DataItems.SemesterItem.GetSemesterList();
            this.cboSemester.ValueMember = "Value";
            this.cboSemester.DisplayMember = "Name";

            this.cboSemester1.DataSource = TeachingEvaluation.DataItems.SemesterItem.GetSemesterList();
            this.cboSemester1.ValueMember = "Value";
            this.cboSemester1.DisplayMember = "Name";

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

            this.cboSemester.SelectedValue = K12.Data.School.DefaultSemester;
            this.cboSemester1.SelectedValue = K12.Data.School.DefaultSemester;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void nudSchoolYear_ValueChanged(object sender, EventArgs e)
        {
            if (!this.form_loaded)
                return;

            this.InitCourseDataSource();
            this.DGV_DataBinding();
        }

        private void cboSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.form_loaded)
                return;

            this.InitCourseDataSource();
            this.DGV_DataBinding();
        }

        private void DGV_DataBinding()
        {
            if (!this.form_loaded)
                return;

            int school_year = int.Parse(this.nudSchoolYear.Value + "");
            int semester = int.Parse((this.cboSemester.SelectedItem as TeachingEvaluation.DataItems.SemesterItem).Value);

            int school_year1 = int.Parse(this.nudSchoolYear1.Value + "");
            int semester1 = int.Parse((this.cboSemester1.SelectedItem as TeachingEvaluation.DataItems.SemesterItem).Value);

            this.circularProgress.Visible = true;
            this.circularProgress.IsRunning = true;

            string strFilter_SNum = this.txtFilterSnum.Text.Trim();
            string strFilter_Course = this.cboCourse.Text.Trim();

            //Task<List<object>> task = Task<List<object>>.Factory.StartNew(() =>
            //{
                DataTable dataTable_Filter_SurveyHistory = this.dataTable_SurveyHistory.Clone();
                List<SurveyHistory> SurveyHistory = new List<Forms.SurveyHistory>();

                foreach (DataRow dataRow in this.dataTable_SurveyHistory.Rows)
                {
                    int rSchoolYear = int.Parse(dataRow["school_year"] + "");
                    int rSemester = int.Parse(dataRow["semester"] + "");
                    string student_number = (dataRow["student_number"] + "").ToLower();
                    string student_name = (dataRow["student_name"] + "").ToLower();
                    string course_name = (dataRow["course_name"] + "").ToLower();

                    if (rSchoolYear < school_year || rSchoolYear > school_year1)
                        continue;
                    if (rSchoolYear == school_year && rSemester < semester)
                        continue;
                    if (rSchoolYear == school_year1 && rSemester > semester1)
                        continue;

                    if (!(string.IsNullOrWhiteSpace(strFilter_SNum)))
                    {
                        if (!student_number.Contains(strFilter_SNum.ToLower()) && !student_name.Contains(strFilter_SNum.ToLower()))
                            continue;
                    }

                    if (!(string.IsNullOrWhiteSpace(strFilter_Course)))
                    {
                        if (!course_name.Contains(strFilter_Course.ToLower()))
                            continue;
                    }

                    dataTable_Filter_SurveyHistory.Rows.Add(dataRow.ItemArray);
                }
                foreach (SurveyHistory sh in this.dicAchievingInfos.Values)
                {
                    if (sh.SchoolYear < school_year || sh.SchoolYear > school_year1)
                        continue;
                    if (sh.SchoolYear == school_year && sh.Semester < semester)
                        continue;
                    if (sh.SchoolYear == school_year1 && sh.Semester > semester1)
                        continue;

                    if (!(string.IsNullOrWhiteSpace(strFilter_SNum)))
                    {
                        if (!(sh.StudentNumber).ToLower().Contains(strFilter_SNum.ToLower()) && !(sh.StudentName).ToLower().Contains(strFilter_SNum.ToLower()))
                            continue;
                    }
                    SurveyHistory.Add(sh);
                }
            //    return new List<object>() { dataTable_Filter_SurveyHistory, SurveyHistory };
            //});

            //task.ContinueWith((x) =>
            //{
                this.dgvDataSurveyHistory.Rows.Clear();
                this.dgvDataAchievingRate.Rows.Clear();

                //if (x.Exception != null)
                //{
                //    MessageBox.Show(x.Exception.InnerException.Message);
                //    goto TheEnd;
                //}
                //DataTable dataTable_Filter_SurveyHistory = x.Result.ElementAt(0) as DataTable;
                //List<SurveyHistory> SurveyHistory = x.Result.ElementAt(1) as List<SurveyHistory>;
                foreach (DataRow row in dataTable_Filter_SurveyHistory.Rows)
                {
                    List<object> source = new List<object>();

                    source.Add(row["student_number"] + "");
                    source.Add(row["student_name"] + "");
                    source.Add(row["school_year"] + "");
                    source.Add(row["semester"] + "");
                    source.Add(row["course_name"] + "");
                    source.Add(row["teacher_name"] + "");
                    if (string.IsNullOrEmpty(row["ref_student_id"] + ""))
                        source.Add("否");
                    else
                        source.Add("是");

                    int idx = this.dgvDataSurveyHistory.Rows.Add(source.ToArray());
                    //Application.DoEvents();
                    this.dgvDataSurveyHistory.Rows[idx].Tag = row["student_id"] + "";
                }
                foreach (SurveyHistory sh in SurveyHistory)
                {
                    List<object> source = new List<object>();

                    source.Add(sh.StudentNumber);
                    source.Add(sh.StudentName);
                    source.Add(sh.SchoolYear);
                    source.Add(sh.Semester);
                    source.Add(sh.AnswerCount);
                    source.Add(sh.SurveyCount);
                    source.Add(sh.Rate);
                    source.Add(sh.IsQualify ? "是" : "否");
                    source.Add(string.Join("、", sh.NoReplyCourses));
                    source.Add(string.Join("、", sh.ReplyCourses));

                    int idx = this.dgvDataAchievingRate.Rows.Add(source.ToArray());
                    //Application.DoEvents();
                    this.dgvDataAchievingRate.Rows[idx].Tag = sh.StudentID;
                }
                
                TheEnd:
                this.circularProgress.IsRunning = false;
                this.circularProgress.Visible = false;
                this.dgvDataSurveyHistory.CurrentCell = null;
                this.dgvDataAchievingRate.CurrentCell = null;
            //}, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            Workbook wb = new Workbook();                
            
            this.circularProgress.Visible = true;
            this.circularProgress.IsRunning = true;
            this.Confirm.Enabled = false;

            List<DataGridViewRow> DataGridViewRows_SurveyHistory = this.dgvDataSurveyHistory.Rows.Cast<DataGridViewRow>().ToList();
            List<DataGridViewRow> DataGridViewRows_AchievingRate = this.dgvDataAchievingRate.Rows.Cast<DataGridViewRow>().ToList();
            
            Task task = Task.Factory.StartNew(() =>
            {
                wb.Worksheets.Cast<Worksheet>().ToList().ForEach(x => wb.Worksheets.RemoveAt(x.Index));

                wb.Worksheets.Add();
                wb.Worksheets[0].Name = "填答記錄";

                if (DataGridViewRows_SurveyHistory.Count > 0)
                {
                    for (int i = 0; i < DataGridViewRows_SurveyHistory.ElementAt(0).Cells.Count; i++)
                    {
                        wb.Worksheets[0].Cells[0, i].PutValue(DataGridViewRows_SurveyHistory.ElementAt(0).Cells[i].OwningColumn.HeaderText);
                    }
                }

                for (int i = 0; i < DataGridViewRows_SurveyHistory.Count; i++)
                {
                    for (int j = 0; j < DataGridViewRows_SurveyHistory.ElementAt(0).Cells.Count; j++)
                    {
                        wb.Worksheets[0].Cells[i + 1, j].PutValue(DataGridViewRows_SurveyHistory.ElementAt(i).Cells[j].Value + "");
                    }
                }

                wb.Worksheets.Add();
                wb.Worksheets[1].Name = "填答率";

                if (DataGridViewRows_AchievingRate.Count > 0)
                {
                    for (int i = 0; i < DataGridViewRows_AchievingRate.ElementAt(0).Cells.Count; i++)
                    {
                        wb.Worksheets[1].Cells[0, i].PutValue(DataGridViewRows_AchievingRate.ElementAt(0).Cells[i].OwningColumn.HeaderText);
                    }
                }

                for (int i = 0; i < DataGridViewRows_AchievingRate.Count; i++)
                {
                    for (int j = 0; j < DataGridViewRows_AchievingRate.ElementAt(0).Cells.Count; j++)
                    {
                        wb.Worksheets[1].Cells[i + 1, j].PutValue(DataGridViewRows_AchievingRate.ElementAt(i).Cells[j].Value + "");
                    }
                }

                wb.Worksheets[0].AutoFitColumns();
                wb.Worksheets[1].AutoFitColumns();
            });
            task.ContinueWith((x) =>
            {
                if (x.Exception != null)
                {
                    MessageBox.Show(x.Exception.InnerException.Message);
                    goto TheEnd;
                }
                SaveFileDialog sd = new SaveFileDialog();
                sd.Filter = "Excel 檔案|*.xls;";
                sd.FileName = "填答記錄-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-");
                sd.AddExtension = true;
                if (sd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        wb.Save(sd.FileName);
                        System.Diagnostics.Process.Start(sd.FileName);
                    }
                    catch (Exception ex)
                    {
                        FISCA.Presentation.Controls.MsgBox.Show(ex.Message);
                    }
                }
            TheEnd:
                this.circularProgress.IsRunning = false;
                this.circularProgress.Visible = false;
                this.Confirm.Enabled = true;
                    
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void txtSNum_TextChanged(object sender, EventArgs e)
        {
            if (!form_loaded)
                return;

            this.DGV_DataBinding();
        }

        private void txtFilterCourseName_TextChanged(object sender, EventArgs e)
        {
        }

        private void cboCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.DGV_DataBinding();
        }

        private void cboCourse_TextChanged(object sender, EventArgs e)
        {
            this.DGV_DataBinding();
        }

        private void txtFilterSnum_KeyUp(object sender, KeyEventArgs e)
        {

        }
    }

    public class Course
    {
        public string CourseID { set; get; }
        public string CourseName { set; get; }
        public int SchoolYear { set; get; }
        public int Semester { set; get; }

        public Course() { }
    }

    public class SurveyHistory
    {
        public string StudentID { set; get; }
        public string StudentNumber { set; get; }
        public string StudentName { set; get; }
        public int SchoolYear { set; get; }
        public int Semester { set; get; }
        public decimal AnswerCount { set; get; }
        public decimal SurveyCount { set; get; }
        public decimal Rate { set; get; }
        public bool IsQualify { set; get; }
        public List<string> ReplyCourses { set; get; }
        public List<string> NoReplyCourses { set; get; }

        public SurveyHistory() 
        {
            this.ReplyCourses = new List<string>();
            this.NoReplyCourses = new List<string>();
        }
    }
}