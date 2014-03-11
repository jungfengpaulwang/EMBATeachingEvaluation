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
using K12.Data;
using System.Dynamic;
using System.Xml.Linq;

namespace TeachingEvaluation.Forms
{
    public partial class frmEvaluationCalc : BaseForm
    {
        private AccessHelper Access;
        private QueryHelper Query;

        private bool form_loaded;

        private Dictionary<string, List<string>> dicTeachersCases;

        public frmEvaluationCalc()
        {
            InitializeComponent();
            this.dicTeachersCases = new Dictionary<string, List<string>>();
            this.Load += new EventHandler(frmTeachingEvaluation_Load);
            this.dgvData.SortCompare += new DataGridViewSortCompareEventHandler(DataGridView_SortCompare);
        }

        private void frmTeachingEvaluation_Load(object sender, EventArgs e)
        {
            form_loaded = false;

            Access = new AccessHelper();
            Query = new QueryHelper();

            this.InitSchoolYear();
            this.InitSemester();

            form_loaded = true;

            InitCourse();
        }

        private void DataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 5 || e.Column.Index == 4 || e.Column.Index == 9)
            {
                DateTime date_time_1 = new DateTime(1, 1, 1);
                DateTime date_time_2 = new DateTime(1, 1, 1);

                DateTime.TryParse(e.CellValue1 + "", out date_time_1);
                DateTime.TryParse(e.CellValue2 + "", out date_time_2);
                e.SortResult = System.DateTime.Compare(date_time_1, date_time_2);
            }
            else if (e.Column.Index == 6 || e.Column.Index == 7)
            {
                int value1 = 0;
                int value2 = 0;

                int.TryParse(e.CellValue1 + "", out value1);
                int.TryParse(e.CellValue2 + "", out value2);
                e.SortResult = value1 - value2;
            }
            else
                e.SortResult = System.String.Compare(e.CellValue1 + "", e.CellValue2 + "");
            e.Handled = true;
        }

        private void InitSchoolYear()
        {
            int DefaultSchoolYear;
            if (int.TryParse(K12.Data.School.DefaultSchoolYear, out DefaultSchoolYear))
            {
                this.nudSchoolYear.Value = decimal.Parse(DefaultSchoolYear.ToString());
            }
            else
            {
                this.nudSchoolYear.Value = decimal.Parse((DateTime.Today.Year - 1911).ToString());
            }
        }

        private void InitSemester()
        {
            this.cboSemester.DataSource = DataItems.SemesterItem.GetSemesterList();
            this.cboSemester.ValueMember = "Value";
            this.cboSemester.DisplayMember = "Name";

            this.cboSemester.SelectedValue = K12.Data.School.DefaultSemester;
        }

        private void InitCourse()
        {
            if (!this.form_loaded)
                return;

            this.cboCourse.Items.Clear();

            int school_year = int.Parse(this.nudSchoolYear.Value + "");
            int semester = int.Parse((this.cboSemester.SelectedItem as DataItems.SemesterItem).Value);
            List<K12.Data.CourseRecord> SelectedCourses = K12.Data.Course.SelectBySchoolYearAndSemester(school_year, semester);

            if (SelectedCourses.Count == 0)
                return;

            ComboItem comboItem1 = new ComboItem("全部");
            comboItem1.Tag = null;
            this.cboCourse.Items.Add(comboItem1);

            //  所有課程
            foreach (K12.Data.CourseRecord course in SelectedCourses)
            {
                ComboItem item = new ComboItem(course.Name);
                item.Tag = course.ID;
                this.cboCourse.Items.Add(item);
            }

            this.cboCourse.SelectedItem = comboItem1;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void nudSchoolYear_ValueChanged(object sender, EventArgs e)
        {
            this.InitCourse();
            this.DGV_DataBinding();
        }

        private void cboSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.InitCourse();
            this.DGV_DataBinding();
        }

        private void cboCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DGV_DataBinding();
        }

        private void DGV_DataBinding()
        {
            if (!this.form_loaded)
                return;

            this.dgvData.Rows.Clear();
            this.dicTeachersCases.Clear();

            if (this.cboCourse.SelectedIndex == -1)
                return;
            //  課程、課程識別碼、課號、授課教師、問卷填寫期間、填寫比例、備份日期
            ComboItem combo_course = this.cboCourse.SelectedItem as ComboItem;

            int CourseID = 0;
            int.TryParse((combo_course).Tag + "", out CourseID);

            int school_year = int.Parse(this.nudSchoolYear.Value + "");
            int semester = int.Parse((this.cboSemester.SelectedItem as DataItems.SemesterItem).Value);

            DataTable dataTable_Course;
            DataTable dataTable_Teacher;
            DataTable dataTable_AssignedSurveyTeacher;
            DataTable dataTable_SCAttendExt;
            DataTable dataTable_Reply;

            Dictionary<string, List<dynamic>> dicTeachers = new Dictionary<string, List<dynamic>>();
            Dictionary<string, dynamic> dicCourses = new Dictionary<string, dynamic>();
            Dictionary<string, KeyValuePair<string, string>> dicAssignedTeachers = new Dictionary<string, KeyValuePair<string, string>>();
            Dictionary<string, string> dicSCAttendNos = new Dictionary<string, string>();
            Dictionary<string, string> dicReplys = new Dictionary<string, string>();

            List<CourseRecord> Courses = new List<CourseRecord>();
            List<TeacherRecord> Teachers;
            Dictionary<string, List<string>> dicCases = new Dictionary<string, List<string>>();
            Dictionary<string, UDT.AssignedSurvey> dicAssignedSurveys = new Dictionary<string, UDT.AssignedSurvey>();    
            List<DataGridViewRow> DataGridViewRows = new List<DataGridViewRow>();
            List<UDT.Survey> Surveys = new List<UDT.Survey>();
            Dictionary<string, UDT.Survey> dicSurveys = new Dictionary<string, UDT.Survey>();

            List<UDT.TeacherStatistics> TeacherStatistics = new List<UDT.TeacherStatistics>();
            Dictionary<string, UDT.TeacherStatistics> dicTeacherStatistics = new Dictionary<string, UDT.TeacherStatistics>();
            
            this.circularProgress.Visible = true;
            this.circularProgress.IsRunning = true;
            Task task = Task.Factory.StartNew(()=>
            {
                string SQL = string.Empty;

                TeacherStatistics = Access.Select<UDT.TeacherStatistics>();
                if (TeacherStatistics.Count > 0)
                    dicTeacherStatistics = TeacherStatistics.ToDictionary(x => x.CourseID + "-" + x.TeacherID + "-" + x.SchoolYear + "-" + x.Semester);

                if (CourseID == 0)
                    SQL = string.Format(@"select count(reply.ref_student_id) as reply_no, reply.ref_course_id as course_id, reply.ref_teacher_id as teacher_id from  course join $ischool.emba.teaching_evaluation.reply as reply on course.id=reply.ref_course_id
where reply.status=1 and course.school_year={0} and course.semester={1}
group by reply.ref_course_id, reply.ref_teacher_id", school_year, semester);
                else
                    SQL = string.Format(@"select count(reply.ref_student_id) as reply_no, reply.ref_course_id as course_id, reply.ref_teacher_id as teacher_id from  course join $ischool.emba.teaching_evaluation.reply as reply on course.id=reply.ref_course_id where reply.status=1 and course.id={0} Group by reply.ref_course_id, reply.ref_teacher_id", CourseID);

                dataTable_Reply = Query.Select(SQL);
                foreach (DataRow row in dataTable_Reply.Rows)
                {
                    if (!dicReplys.ContainsKey(row["course_id"] + "-" + row["teacher_id"]))
                        dicReplys.Add(row["course_id"] + "-" + row["teacher_id"], row["reply_no"] + "");
                }

                if (CourseID == 0)
                    SQL = string.Format(@"select course.id as course_id, count(ref_student_id) as attend_no from course join $ischool.emba.scattend_ext as se on se.ref_course_id=course.id where is_cancel=false and course.school_year={0} and course.semester={1} Group by course.id", school_year, semester);
                else
                    SQL = string.Format(@"select course.id as course_id, count(ref_student_id) as attend_no from course join $ischool.emba.scattend_ext as se on se.ref_course_id=course.id where is_cancel=false and course.id={0} Group by course.id", CourseID);

                dataTable_SCAttendExt = Query.Select(SQL);
                foreach (DataRow row in dataTable_SCAttendExt.Rows)
                {
                    if (!dicSCAttendNos.ContainsKey(row["course_id"] + ""))
                        dicSCAttendNos.Add(row["course_id"] + "", row["attend_no"] + "");
                }

                if (CourseID == 0)
                    SQL = string.Format(@"select course.id as course_id, course.course_name, teacher.id as teacher_id, teacher.teacher_name, teas.opening_time, teas.end_time from course join $ischool.emba.teaching_evaluation.assigned_survey as teas on teas.ref_course_id=course.id
left join teacher on teacher.id=teas.ref_teacher_id where course.school_year={0} and course.semester={1}", school_year, semester);
                else
                    SQL = string.Format(@"select course.id as course_id, course.course_name, teacher.id as teacher_id, teacher.teacher_name, teas.opening_time, teas.end_time from course join $ischool.emba.teaching_evaluation.assigned_survey as teas on teas.ref_course_id=course.id left join teacher on teacher.id=teas.ref_teacher_id where course.id={0}", CourseID);

                dataTable_AssignedSurveyTeacher = Query.Select(SQL);
                foreach (DataRow row in dataTable_AssignedSurveyTeacher.Rows)
                {
                    if (!dicAssignedTeachers.ContainsKey(row["course_id"] + "-" + row["teacher_id"]))
                    {
                        DateTime opening_time;
                        DateTime end_time;

                        DateTime.TryParse(row["opening_time"] + "", out opening_time);
                        DateTime.TryParse(row["end_time"] + "", out end_time);

                        dicAssignedTeachers.Add(row["course_id"] + "-" + row["teacher_id"], new KeyValuePair<string, string>(opening_time.ToString("yyyy/MM/dd HH:mm"), end_time.ToString("yyyy/MM/dd HH:mm")));
                    }
                }
                    
                if (CourseID == 0)
                    SQL = string.Format(@"select course.id as course_id, course.course_name, subject.subject_code, subject.new_subject_code, ce.class_name, subject.name as subject_name, subject.uid as subject_id from course join $ischool.emba.course_ext as ce on ce.ref_course_id=course.id join $ischool.emba.subject as subject on subject.uid=ce.ref_subject_id where course.school_year={0} and course.semester={1}", school_year, semester);
                else
                    SQL = string.Format(@"select course.id as course_id, course.course_name, subject.subject_code, subject.new_subject_code, ce.class_name, subject.name as subject_name, subject.uid as subject_id from course join $ischool.emba.course_ext as ce on ce.ref_course_id=course.id join $ischool.emba.subject as subject on subject.uid=ce.ref_subject_id where course.id={0}", CourseID);

                dataTable_Course = Query.Select(SQL);
                foreach (DataRow row in dataTable_Course.Rows)
                {
                    if (!dicCourses.ContainsKey(row["course_id"] + ""))
                    {
                        dynamic o = new ExpandoObject();

                        o.CourseID = row["course_id"] + "";
                        o.CourseName = row["course_name"] + "";
                        o.SubjectCode = row["subject_code"] + "";
                        o.NewSubjectCode = row["new_subject_code"] + "";
                        o.SubjectName = row["subject_name"] + "";
                        o.ClassName = row["class_name"] + "";
                        o.SubjectID = row["subject_id"] + "";

                        dicCourses.Add(row["course_id"] + "", o);
                    }
                }

                if (CourseID == 0)
                    SQL = string.Format(@"SELECT  distinct teacher.id as teacher_id, teacher.teacher_name, course.id as course_id FROM course LEFT JOIN $ischool.emba.course_instructor ON $ischool.emba.course_instructor.ref_course_id = course.id LEFT JOIN teacher ON teacher.id = $ischool.emba.course_instructor.ref_teacher_id LEFT JOIN tag_teacher ON tag_teacher.ref_teacher_id = teacher.id LEFT JOIN tag ON tag.id = tag_teacher.ref_tag_id WHERE tag.category = 'Teacher' AND tag.prefix = '教師' AND course.school_year={0} AND course.semester={1} order by course.id", school_year, semester);
                else
                    SQL = string.Format(@"SELECT  distinct teacher.id as teacher_id, teacher.teacher_name, course.id as course_id FROM course LEFT JOIN $ischool.emba.course_instructor ON $ischool.emba.course_instructor.ref_course_id = course.id LEFT JOIN teacher ON teacher.id = $ischool.emba.course_instructor.ref_teacher_id LEFT JOIN tag_teacher ON tag_teacher.ref_teacher_id = teacher.id LEFT JOIN tag ON tag.id = tag_teacher.ref_tag_id WHERE tag.category = 'Teacher' AND tag.prefix = '教師' AND course.id={0} order by course.id", CourseID);

                dataTable_Teacher = Query.Select(SQL);
                foreach (DataRow row in dataTable_Teacher.Rows)
                {
                    if (!dicAssignedTeachers.ContainsKey(row["course_id"] + "-" + row["teacher_id"]))
                        continue;

                    if (!dicTeachers.ContainsKey(row["course_id"] + ""))
                        dicTeachers.Add(row["course_id"] + "", new List<dynamic>());

                    dynamic o = new ExpandoObject();

                    o.CourseID = row["course_id"] + "";
                    o.TeacherID = row["teacher_id"] + "";
                    o.TeacherName = row["teacher_name"] + "";
                    o.OpeningTime = dicAssignedTeachers[row["course_id"] + "-" + row["teacher_id"]].Key;
                    o.EndTime = dicAssignedTeachers[row["course_id"] + "-" + row["teacher_id"]].Value;

                    if (dicSCAttendNos.ContainsKey(row["course_id"] + ""))
                        o.SCAttendNo = dicSCAttendNos[row["course_id"] + ""];
                    else
                        o.SCAttendNo = "0";

                    if (dicReplys.ContainsKey(row["course_id"] + "-" + row["teacher_id"]))
                        o.ReplyNo = dicReplys[row["course_id"] + "-" + row["teacher_id"]];
                    else
                        o.ReplyNo = "0";

                    dicTeachers[row["course_id"] + ""].Add(o);
                }
            });

            task.ContinueWith((x) =>
            {
                this.dgvData.Rows.Clear();
                if (x.Exception != null)
                {
                    MessageBox.Show(x.Exception.InnerException.Message);
                    goto TheEnd;
                }
                foreach (string key in dicTeachers.Keys)
                {
                    List<dynamic> oTeachers = dicTeachers[key];
                    dynamic oCourse = dicCourses[key];

                    foreach (dynamic o in oTeachers)
                    {
                        List<object> sources = new List<object>();
                        
                        sources.Add(oCourse.CourseName);
                        sources.Add(oCourse.SubjectCode);
                        sources.Add(oCourse.NewSubjectCode);
                        sources.Add(o.TeacherName);
                        sources.Add(o.OpeningTime);
                        sources.Add(o.EndTime);
                        sources.Add(o.SCAttendNo);
                        sources.Add(o.ReplyNo);

                        int intCSAttendNo = int.Parse(o.SCAttendNo);
                        int intFeedBackNo = int.Parse(o.ReplyNo);

                        if (intCSAttendNo == 0 || intCSAttendNo == 0)
                            sources.Add("未達半數");
                        else
                        {
                            if (intCSAttendNo <= intFeedBackNo * 2)
                                sources.Add(string.Empty);
                            else
                                sources.Add("未達半數");
                        }

                        string key_statistics = oCourse.CourseID + "-" + o.TeacherID + "-" + school_year + "-" + semester;
                        if (dicTeacherStatistics.ContainsKey(key_statistics))
                            sources.Add(dicTeacherStatistics[key_statistics].TimeStamp.ToString("yyyy/MM/dd HH:mm:ss"));
                        else
                            sources.Add(string.Empty);

                        int idx = this.dgvData.Rows.Add(sources.ToArray());
                        this.dgvData.Rows[idx].Cells[0].Tag = oCourse.CourseID;
                        this.dgvData.Rows[idx].Cells[3].Tag = o.TeacherID;
                        this.dgvData.Rows[idx].Cells[1].Tag = oCourse.SubjectID;

                        this.dgvData.Rows[idx].Tag = oCourse;
                    }
                }
                TheEnd:
                this.circularProgress.IsRunning = false;
                this.circularProgress.Visible = false;
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            this.circularProgress.IsRunning = true;
            this.circularProgress.Visible = true;
            this.btnSet.Enabled = false;

            int school_year = int.Parse(this.nudSchoolYear.Value + "");
            int semester = int.Parse((this.cboSemester.SelectedItem as DataItems.SemesterItem).Value);

            List<dynamic> backup_lists = new List<dynamic>();
            foreach (DataGridViewRow row in this.dgvData.Rows)
            {
                if (!row.Selected)
                    continue;

                dynamic o = new ExpandoObject();
                dynamic oo = row.Tag as dynamic;

                o.CourseName = row.Cells[0].Value + "";
                o.CourseID = row.Cells[0].Tag + "";
                o.TeacherID = row.Cells[3].Tag + "";
                o.TeacherName = row.Cells[3].Value + "";
                o.SubjectCode = row.Cells[1].Value + "";
                o.NewSubjectCode = row.Cells[2].Value + "";
                o.TotalNo = row.Cells[6].Value + "";
                o.FillNo = row.Cells[7].Value + "";
                o.SubjectName = oo.SubjectName;
                o.ClassName = oo.ClassName;
                o.Status = row.Cells[8].Value + "";
                o.SubjectID = row.Cells[1].Tag + "";

                DateTime survey_date_begin = DateTime.Today;
                DateTime survey_date_end = DateTime.Today;
                List<string> survey_dates = new List<string>();

                if (DateTime.TryParse(row.Cells[4].Value + "", out survey_date_begin))
                    survey_dates.Add(survey_date_begin.ToShortDateString());
                if (DateTime.TryParse(row.Cells[5].Value + "", out survey_date_end))
                    survey_dates.Add(survey_date_end.ToShortDateString());

                survey_dates = survey_dates.Distinct().ToList();

                o.SurveyDate = string.Join("~", survey_dates);

                backup_lists.Add(o);
            }
            Task task = Task.Factory.StartNew(()=>
            {
                List<UDT.Reply> Replys = Access.Select<UDT.Reply>("status = 1");
                List<UDT.TeacherStatistics> TeacherStatistics = Access.Select<UDT.TeacherStatistics>();
                Dictionary<string, UDT.TeacherStatistics> dicTeacherStatistics = new Dictionary<string, UDT.TeacherStatistics>();
                if (TeacherStatistics.Count > 0)
                    dicTeacherStatistics = TeacherStatistics.ToDictionary(x => x.CourseID + "-" + x.TeacherID + "-" + x.SchoolYear + "-" + x.Semester);

                List<UDT.TeacherStatistics> nTeacherStatistics = new List<UDT.TeacherStatistics>();
                foreach (dynamic o in backup_lists)
                {
                    //if (o.Status == "無效")
                    //    continue;

                    BusinessLogic.TeacherStatistics cTeacherStatistics = new BusinessLogic.TeacherStatistics(o.CourseID, o.TeacherID);
                    bool can_save = false;
                    if (cTeacherStatistics.Survey == null)
                        continue;

                    cTeacherStatistics.SchoolYear = school_year.ToString();
                    cTeacherStatistics.Semester = semester.ToString();
                    cTeacherStatistics.CourseName = o.CourseName;
                    cTeacherStatistics.SubjectName = o.SubjectName;
                    cTeacherStatistics.ClassName = o.ClassName;
                    cTeacherStatistics.TeacherName = o.TeacherName;
                    cTeacherStatistics.SubjectCode = o.SubjectCode;
                    cTeacherStatistics.NewSubjectCode = o.NewSubjectCode;
                    cTeacherStatistics.FeedBackCount = o.FillNo;
                    cTeacherStatistics.CSAttendCount = o.TotalNo;
                    cTeacherStatistics.CourseID = o.CourseID;
                    cTeacherStatistics.SubjectID = o.SubjectID;
                    cTeacherStatistics.TeacherID = o.TeacherID;
                    cTeacherStatistics.SurveyDate = o.SurveyDate;

                    foreach (UDT.Reply Reply in Replys)
                    {
                        if (Reply.SurveyID.ToString() != cTeacherStatistics.Survey.uSurvey.UID || Reply.CourseID.ToString() != o.CourseID || Reply.TeacherID.ToString() != o.TeacherID)
                            continue;

                        cTeacherStatistics.SetAnswer(Reply.Answer);
                        can_save = true;
                    }

                    string Statistics_List = cTeacherStatistics.ToString();
                    string key = o.CourseID + "-" + o.TeacherID;
                    UDT.TeacherStatistics oTeacherStatistic = new UDT.TeacherStatistics();
                    if (dicTeacherStatistics.ContainsKey(key + "-" + school_year + "-" + semester))
                        oTeacherStatistic = dicTeacherStatistics[key + "-" + school_year + "-" + semester];

                    oTeacherStatistic.CourseID = int.Parse(o.CourseID);
                    oTeacherStatistic.TeacherID = int.Parse(o.TeacherID);
                    oTeacherStatistic.SchoolYear = school_year;
                    oTeacherStatistic.Semester = semester;
                    oTeacherStatistic.StatisticsList = Statistics_List;
                    oTeacherStatistic.TimeStamp = DateTime.Now;

                    if (can_save)
                        nTeacherStatistics.Add(oTeacherStatistic);
                }
                nTeacherStatistics.SaveAll();
            });

            task.ContinueWith((y) =>
            {
                if (y.Exception != null)
                {
                    MessageBox.Show(y.Exception.InnerException.Message);
                    goto TheEnd;
                }

                this.DGV_DataBinding();
                MessageBox.Show("計算完畢。");
                TheEnd:
                this.circularProgress.IsRunning = false;
                this.circularProgress.Visible = false;
                this.btnSet.Enabled = true;
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
