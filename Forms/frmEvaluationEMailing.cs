using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.Editors;
using FISCA.Data;
using FISCA.Presentation.Controls;
using FISCA.UDT;
using K12.Data;
using Mandrill;
using K12.Data.Configuration;
using Aspose.Cells;

namespace TeachingEvaluation.Forms
{
    public partial class frmEvaluationEMailing : BaseForm
    {
        private AccessHelper Access;
        private QueryHelper Query;

        private bool form_loaded;

        private string SenderEmail = "";
        private string SenderPassword = "";
        private string CC = "";
        private List<string> validated_cc;
        private string MandrillAPIKey = "";
        private string from_email = "";
        private string from_name = "";
        private string guid = Guid.NewGuid().ToString();
        private string user_account = FISCA.Authentication.DSAServices.UserAccount;
        private string email_category = "";
        private DateTime time_stamp = DateTime.Now;
        private ConfigData config; 

        public frmEvaluationEMailing()
        {
            InitializeComponent();
            this.buttonX1.Popup(-1000, -1000);
            this.Load += new EventHandler(Form_Load);
            this.FormClosing += new FormClosingEventHandler(Form_Closing);
            this.validated_cc = new List<string>();
        }

        private void DataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 5 || e.Column.Index == 6 || e.Column.Index == 10 || e.Column.Index == 11)
            {
                DateTime date_time_1 = new DateTime(1, 1, 1);
                DateTime date_time_2 = new DateTime(1, 1, 1);

                DateTime.TryParse(e.CellValue1 + "", out date_time_1);
                DateTime.TryParse(e.CellValue2 + "", out date_time_2);
                e.SortResult = System.DateTime.Compare(date_time_1, date_time_2);
            }
            else if (e.Column.Index == 7 || e.Column.Index == 8)
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

        private void Form_Closing(object sender, EventArgs e)
        {
            this.form_loaded = false;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            form_loaded = false;

            config = K12.Data.School.Configuration["台大EMBA教學意見調查提醒寄件人資訊"];
            if (config != null)
            {
                this.txtSenderEMail.Text = config["SenderEMail"];
                this.txtSenderName.Text = config["SenderName"];
                this.txtCC.Text = config["CC"];

                this.from_email = config["SenderEMail"];
                this.from_name = config["SenderName"];
                string[] ccs = config["CC"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string cc in ccs)
                {
                    if (cc.Trim().Length > 0)
                        this.validated_cc.Add(cc);
                }
            }

            Access = new AccessHelper();
            Query = new QueryHelper();

            this.InitSchoolYear();
            this.InitSemester();

            List<UDT.MandrillAPIKey> MandrillAPIKeys = Access.Select<UDT.MandrillAPIKey>();
            if (MandrillAPIKeys.Count > 0)
                this.MandrillAPIKey = MandrillAPIKeys.ElementAt(0).APIKey;

            InitCourse();

            form_loaded = true;
            if (this.cboCourse.Items.Count > 0)
                this.cboCourse.SelectedIndex = 0;
            this.DGV_DataBinding();
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
            if (!this.form_loaded)
                return;

            this.InitCourse();
            this.DGV_DataBinding();
        }

        private void cboSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.form_loaded)
                return;

            this.InitCourse();
            this.DGV_DataBinding();
        }

        private void cboCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.form_loaded)
                return;

            this.DGV_DataBinding();
        }

        private void DGV_DataBinding()
        {
            if (!this.form_loaded)
                return;

            this.dgvData.Rows.Clear();

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
            Dictionary<string, dynamic> dicAssignedTeachers = new Dictionary<string, dynamic>();
            Dictionary<string, string> dicSCAttendNos = new Dictionary<string, string>();
            Dictionary<string, string> dicReplys = new Dictionary<string, string>();

            List<CourseRecord> Courses = new List<CourseRecord>();
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

                //  所有老師
                TeacherStatistics = Access.Select<UDT.TeacherStatistics>();
                if (TeacherStatistics.Count > 0)
                    dicTeacherStatistics = TeacherStatistics.ToDictionary(x => x.CourseID + "-" + x.TeacherID + "-" + x.SchoolYear + "-" + x.Semester);

                //  答題人數
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

                //  修課人數
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

                //  問卷填寫開始及結束時間
                if (CourseID == 0)
                    SQL = string.Format(@"select course.id as course_id, course.course_name, teacher.id as teacher_id, teacher.teacher_name, teas.opening_time, teas.end_time, teas.email_time, teas.second_email_time, teas.uid from course join $ischool.emba.teaching_evaluation.assigned_survey as teas on teas.ref_course_id=course.id join teacher on teacher.id=teas.ref_teacher_id where course.school_year={0} and course.semester={1}", school_year, semester);
                else
                    SQL = string.Format(@"select course.id as course_id, course.course_name, teacher.id as teacher_id, teacher.teacher_name, teas.opening_time, teas.end_time, teas.email_time, teas.second_email_time, teas.uid from course join $ischool.emba.teaching_evaluation.assigned_survey as teas on teas.ref_course_id=course.id join teacher on teacher.id=teas.ref_teacher_id where course.id={0}", CourseID);

                dataTable_AssignedSurveyTeacher = Query.Select(SQL);
                foreach (DataRow row in dataTable_AssignedSurveyTeacher.Rows)
                {
                    if (!dicAssignedTeachers.ContainsKey(row["course_id"] + "-" + row["teacher_id"]))
                    {
                        dynamic o = new ExpandoObject();

                        DateTime opening_time;
                        DateTime end_time;
                        DateTime email_time;
                        DateTime second_email_time;

                        if (DateTime.TryParse(row["opening_time"] + "", out opening_time))
                            o.OpeningTime = opening_time.ToString("yyyy/MM/dd HH:mm:ss");
                        else
                            o.OpeningTime = string.Empty;

                        if (DateTime.TryParse(row["end_time"] + "", out end_time))
                            o.EndTime = end_time.ToString("yyyy/MM/dd HH:mm:ss");
                        else
                            o.EndTime = string.Empty;

                        if (DateTime.TryParse(row["email_time"] + "", out email_time))
                            o.EmailTime = email_time.ToString("yyyy/MM/dd HH:mm:ss");
                        else
                            o.EmailTime = string.Empty;

                        if (DateTime.TryParse(row["second_email_time"] + "", out second_email_time))
                            o.SecondEmailTime = second_email_time.ToString("yyyy/MM/dd HH:mm:ss");
                        else
                            o.SecondEmailTime = string.Empty;

                        o.AssignedSurvey = Access.Select<UDT.AssignedSurvey>(new List<string>() { row["uid"] + "" }).ElementAt(0);

                        dicAssignedTeachers.Add(row["course_id"] + "-" + row["teacher_id"], o);
                    }
                }
                    
                //  課程及開課
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

                //  授課教師
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
                    o.OpeningTime = (string)dicAssignedTeachers[row["course_id"] + "-" + row["teacher_id"]].OpeningTime;
                    o.EndTime = (string)dicAssignedTeachers[row["course_id"] + "-" + row["teacher_id"]].EndTime;
                    o.EmailTime = (string)dicAssignedTeachers[row["course_id"] + "-" + row["teacher_id"]].EmailTime;
                    o.SecondEmailTime = (string)dicAssignedTeachers[row["course_id"] + "-" + row["teacher_id"]].SecondEmailTime;

                    if (dicSCAttendNos.ContainsKey(row["course_id"] + ""))
                        o.SCAttendNo = dicSCAttendNos[row["course_id"] + ""];
                    else
                        o.SCAttendNo = "0";

                    if (dicReplys.ContainsKey(row["course_id"] + "-" + row["teacher_id"]))
                        o.ReplyNo = dicReplys[row["course_id"] + "-" + row["teacher_id"]];
                    else
                        o.ReplyNo = "0";

                    o.AssignedSurvey = dicAssignedTeachers[row["course_id"] + "-" + row["teacher_id"]].AssignedSurvey as UDT.AssignedSurvey;

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
                    if (!dicCourses.ContainsKey(key))
                        continue;
                    if (!dicTeachers.ContainsKey(key))
                        continue;

                    List<dynamic> oTeachers = dicTeachers[key];
                    dynamic oCourse = dicCourses[key];

                    foreach (dynamic o in oTeachers)
                    {
                        List<object> sources = new List<object>();

                        sources.Add((string)oCourse.SubjectName);
                        sources.Add((string)oCourse.SubjectCode);
                        sources.Add((string)oCourse.NewSubjectCode);
                        sources.Add((string)oCourse.CourseName);
                        sources.Add((string)o.TeacherName);
                        sources.Add((string)o.OpeningTime);
                        sources.Add((string)o.EndTime);
                        sources.Add((string)o.SCAttendNo);
                        sources.Add((string)o.ReplyNo);

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

                        sources.Add((string)o.EmailTime);
                        sources.Add((string)o.SecondEmailTime);

                        int idx = this.dgvData.Rows.Add(sources.ToArray());
                        this.dgvData.Rows[idx].Cells[0].Tag = (string)oCourse.SubjectID;
                        this.dgvData.Rows[idx].Cells[3].Tag = (string)o.CourseID;
                        this.dgvData.Rows[idx].Cells[4].Tag = (string)o.TeacherID;

                        this.dgvData.Rows[idx].Tag = o.AssignedSurvey as UDT.AssignedSurvey;
                    }
                }
                TheEnd:
                this.circularProgress.IsRunning = false;
                this.circularProgress.Visible = false;
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool isValidEmail(string email)
        {
            string patternStrict = @"^(([^<>()[\]\\.,;:\s@\""]+"
                       + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                       + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                       + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                       + @"[a-zA-Z]{2,}))$";
            Regex reStrict = new Regex(patternStrict);

            bool isStrictMatch = reStrict.IsMatch(email);
            return isStrictMatch;
        }

        void frmCred_AfterInputCredential(object sender, EmailCredentialEventArgs args)
        {
            if (args.Cancel)
                return;
            this.SenderEmail = args.UserID;
            this.SenderPassword = args.Password;
            this.CC = args.CC;
            this.validated_cc.Clear();
            //處理寄件備份
            string[] ccs = this.CC.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string cc in ccs)
            {
                if (cc.Trim().Length > 0)
                    this.validated_cc.Add(cc);
            }
        }

        private void btnSendEmail_Click(object sender, EventArgs e)
        {
            this.SendEmail_Click(1);
        }

        private void btnSendSecondEmail_Click(object sender, EventArgs e)
        {
            this.SendEmail_Click(2);
        }

        private void SendEmail_Click(int no)
        {
            if (this.dgvData.SelectedRows.Count < 1)
            {
                MessageBox.Show("請選擇要發送 Email 通知的紀錄。", "提示");
                return;
            }

            if (string.IsNullOrWhiteSpace(this.txtSenderEMail.Text) || string.IsNullOrWhiteSpace(this.txtSenderName.Text))
            {
                MessageBox.Show("請輸入寄件人 Email 及名稱。");
                return;
            }

            MandrillApi mandrill = new MandrillApi(this.MandrillAPIKey, false);

            string pong = string.Empty;

            try
            {
                pong = mandrill.Ping();
            }
            catch (Exception ex)
            {
                MessageBox.Show("未正確設定「MandrillAPIKey」, 錯誤訊息：" + ex.Message);
                return;
            }
            if (!pong.ToUpper().Contains("PONG!"))
            {
                MessageBox.Show("請先設定「MandrillAPIKey」。");
                return;
            }

            ////彈出畫面指定寄件者
            //if (string.IsNullOrWhiteSpace(this.SenderEmail))
            //{
            //    Forms.Email_Credential frmCred = new Email_Credential();
            //    frmCred.AfterInputCredential += new Email_Credential.InputCredentialHandler(frmCred_AfterInputCredential);
            //    frmCred.ShowDialog();
            //}

            ////如果還是沒有指定寄件者，則離開
            //if (string.IsNullOrWhiteSpace(this.SenderEmail))
            //{
            //    MessageBox.Show("未指定寄件者。", "提示");
            //    return;
            //}
            this.guid = Guid.NewGuid().ToString();
            List<UDT.AssignedSurvey> AssignedSurveys = new List<UDT.AssignedSurvey>();
            Dictionary<UDT.AssignedSurvey, dynamic> dicEmailInfos = new Dictionary<UDT.AssignedSurvey, dynamic>();
            //string error_message = string.Empty;
            int SchoolYear = int.Parse(this.nudSchoolYear.Value.ToString());
            int Semester = int.Parse(this.cboSemester.SelectedValue + "");
            foreach (DataGridViewRow row in this.dgvData.SelectedRows)
            {
                UDT.AssignedSurvey AssignedSurvey = row.Tag as UDT.AssignedSurvey;

                if (AssignedSurvey == null)
                    continue;

                //if (AssignedSurvey == null)
                //    error_message += string.Format("開課「{0}」，授課教師「{1}」。\n", row.Cells[0].Value + "", row.Cells[1].Value + "");

                AssignedSurveys.Add(AssignedSurvey);

                dynamic o = new ExpandoObject();


                o.CourseID = AssignedSurvey.CourseID;
                o.CourseName = row.Cells[3].Value + "";
                o.TeacherID = AssignedSurvey.TeacherID;
                o.TeacherName = row.Cells[4].Value + "";
                o.SubjectID = row.Cells[0].Tag + "";
                o.SubjectName = row.Cells[0].Value + "";
                o.SchoolYear = SchoolYear;
                o.Semester = TeachingEvaluation.DataItems.SemesterItem.GetSemesterByCode(Semester.ToString()).Name;
                o.SurveyBeginTime = row.Cells[5].Value + "";
                o.SurveyEndTime = row.Cells[6].Value + "";
                o.EmailList = new Dictionary<StudentRecord, IEnumerable<string>>();

                dicEmailInfos.Add(AssignedSurvey, o);
            }
            //if (!string.IsNullOrEmpty(error_message))
            //{
            //    MessageBox.Show("下列授課教師尚未設定評鑑組態：\n\n" + error_message, "錯誤");
            //    return;
            //}

            this.from_email = this.txtSenderEMail.Text.Trim();
            this.from_name = this.txtSenderName.Text.Trim();
            List<string> email_lists = new List<string>();
            Dictionary<string, List<string>> error_email_lists = new Dictionary<string, List<string>>();
            this.circularProgress.Visible = true;
            this.circularProgress.IsRunning = true;
            this.btnSendEmail.Enabled = false;
            this.btnSendSecondEmail.Enabled = false;
            Task task = Task.Factory.StartNew(() =>
            {
                //  評鑑開課
                List<int> CourseIDs = new List<int>();
                //  修課學生
                Dictionary<string, List<string>> dicSCAttends = new Dictionary<string, List<string>>();
                //  待寄通知學生
                Dictionary<UDT.AssignedSurvey, IEnumerable<string>> dicStudentIDs = new Dictionary<UDT.AssignedSurvey, IEnumerable<string>>();
                List<StudentRecord> Students = Student.SelectAll();
                Dictionary<string, StudentRecord> dicStudents = new Dictionary<string, StudentRecord>();
                if (Students.Count > 0)
                    dicStudents = Students.ToDictionary(x => x.ID);
                foreach (UDT.AssignedSurvey AssignedSurvey in AssignedSurveys)
                {
                    CourseIDs.Add(AssignedSurvey.CourseID);
                }
                //  取得修課學生
                DataTable dataTable = Query.Select(string.Format(@"select student.id as student_id, course.id as course_id from student join $ischool.emba.scattend_ext as se on se.ref_student_id=student.id join course on course.id=se.ref_course_id where se.is_cancel=false and   course.id in ({0})", string.Join(",", CourseIDs)));
                foreach (DataRow row in dataTable.Rows)
                {
                    if (!dicSCAttends.ContainsKey(row["course_id"] + ""))
                        dicSCAttends.Add(row["course_id"] + "", new List<string>());

                    dicSCAttends[row["course_id"] + ""].Add(row["student_id"] + "");
                }
                //  取得評鑑回覆學生清單(已回覆者不需Email通知)
                List<UDT.Reply> Replys = Access.Select<UDT.Reply>("status=1");
                foreach (UDT.AssignedSurvey AssignedSurvey in dicEmailInfos.Keys)
                {
                    List<string> reply_student_ids = new List<string>();
                    reply_student_ids = Replys.Where(x => (x.CourseID == AssignedSurvey.CourseID && x.TeacherID == AssignedSurvey.TeacherID)).Select(x => x.StudentID.ToString()).ToList();

                    if (!dicSCAttends.ContainsKey(AssignedSurvey.CourseID.ToString()))
                        continue;

                    IEnumerable<string> need_mail_student_ids = new List<string>();
                    need_mail_student_ids = dicSCAttends[AssignedSurvey.CourseID.ToString()].Except(reply_student_ids);

                    dicStudentIDs.Add(AssignedSurvey, need_mail_student_ids);
                }
                //  取得需Email通知之學生的Email
                List<string> all_need_mail_student_ids = new List<string>();
                dicStudentIDs.Values.ToList().ForEach((x => all_need_mail_student_ids.AddRange(x)));

                if (all_need_mail_student_ids.Count == 0)
                    throw new Exception("所有學生皆已回覆，不需 Email 通知。");

                //  每個學生的電子郵件
                Dictionary<string, IEnumerable<string>> dicStudentEmails = new Dictionary<string, IEnumerable<string>>();
                dataTable = Query.Select(string.Format(@"Select student.id as 學生系統編號, student.student_number as 學號, student.name as 學生姓名, student.sa_login_name as 登入帳號, (xpath_string('<root>' || sb2.email_list || '</root>','email1')) as 電子郵件一, (xpath_string('<root>' || sb2.email_list || '</root>','email2')) as 電子郵件二, (xpath_string('<root>' || sb2.email_list || '</root>','email3')) as 電子郵件三, (xpath_string('<root>' || sb2.email_list || '</root>','email4')) as 電子郵件四, 
(xpath_string('<root>' || sb2.email_list || '</root>','email5')) as 電子郵件五 From student Left join $ischool.emba.student_brief2 as sb2 on sb2.ref_student_id = student.id Where student.id in ({0})", string.Join(",", all_need_mail_student_ids)));
                string error_email_alert_message = string.Empty;
                foreach (DataRow row in dataTable.Rows)
                {
                    List<string> student_emails = new List<string>();
                    if (!dicStudentEmails.ContainsKey(row["學生系統編號"] + ""))
                        dicStudentEmails.Add(row["學生系統編號"] + "", new List<string>());

                    if (!string.IsNullOrWhiteSpace(row["登入帳號"] + ""))
                    {
                        if (isValidEmail(row["登入帳號"] + ""))
                            student_emails.Add((row["登入帳號"] + "").Trim());
                        else
                            error_email_alert_message += string.Format("學號「{0}」，姓名「{1}」，電子郵件「{2}」\n", row["學號"] + "", row["學生姓名"] + "", row["登入帳號"] + "");
                    }

                    if (!string.IsNullOrWhiteSpace(row["電子郵件一"] + ""))
                    {
                        if (isValidEmail(row["電子郵件一"] + ""))
                            student_emails.Add((row["電子郵件一"] + "").Trim());
                        else
                            error_email_alert_message += string.Format("學號「{0}」，姓名「{1}」，電子郵件「{2}」\n", row["學號"] + "", row["學生姓名"] + "", row["電子郵件一"] + "");
                    }

                    if (!string.IsNullOrWhiteSpace(row["電子郵件二"] + ""))
                    {
                        if (isValidEmail(row["電子郵件二"] + ""))
                            student_emails.Add((row["電子郵件二"] + "").Trim());
                        else
                            error_email_alert_message += string.Format("學號「{0}」，姓名「{1}」，電子郵件「{2}」\n", row["學號"] + "", row["學生姓名"] + "", row["電子郵件二"] + "");
                    }

                    if (!string.IsNullOrWhiteSpace(row["電子郵件三"] + ""))
                    {
                        if (isValidEmail(row["電子郵件三"] + ""))
                            student_emails.Add((row["電子郵件三"] + "").Trim());
                        else
                            error_email_alert_message += string.Format("學號「{0}」，姓名「{1}」，電子郵件「{2}」\n", row["學號"] + "", row["學生姓名"] + "", row["電子郵件三"] + "");
                    }

                    if (!string.IsNullOrWhiteSpace(row["電子郵件四"] + ""))
                    {
                        if (isValidEmail(row["電子郵件四"] + ""))
                            student_emails.Add((row["電子郵件四"] + "").Trim());
                        else
                            error_email_alert_message += string.Format("學號「{0}」，姓名「{1}」，電子郵件「{2}」\n", row["學號"] + "", row["學生姓名"] + "", row["電子郵件四"] + "");
                    }

                    if (!string.IsNullOrWhiteSpace(row["電子郵件五"] + ""))
                    {
                        if (isValidEmail(row["電子郵件五"] + ""))
                            student_emails.Add((row["電子郵件五"] + "").Trim());
                        else
                            error_email_alert_message += string.Format("學號「{0}」，姓名「{1}」，電子郵件「{2}」\n", row["學號"] + "", row["學生姓名"] + "", row["電子郵件五"] + "");
                    }
                    dicStudentEmails[row["學生系統編號"] + ""] = student_emails.Distinct();
                }
                if (!string.IsNullOrEmpty(error_email_alert_message))
                    throw new Exception("下列學生及其電子郵件格式有誤，請先修正：\n\n" + error_email_alert_message);

                foreach (UDT.AssignedSurvey AssignedSurvey in dicEmailInfos.Keys)
                {
                    dynamic o = dicEmailInfos[AssignedSurvey];
                    Dictionary<StudentRecord, IEnumerable<string>> dicEmailLists = new Dictionary<StudentRecord, IEnumerable<string>>();
                    IEnumerable<string> need_email_student_ids = dicStudentIDs[AssignedSurvey];

                    foreach (string student_id in need_email_student_ids)
                    {
                        if (dicStudentEmails.ContainsKey(student_id))
                            dicEmailLists.Add(dicStudents[student_id], dicStudentEmails[student_id]);
                    }
                    o.EmailList = dicEmailLists;
                }
                this.SendMail(dicEmailInfos, no, mandrill);
            });
            task.ContinueWith((x) =>
            {
                this.circularProgress.Visible = false;
                this.circularProgress.IsRunning = false;

                this.btnSendEmail.Enabled = true;
                this.btnSendSecondEmail.Enabled = true;
                
                if (x.Exception != null)
                {
                    MessageBox.Show(x.Exception.InnerException.Message);
                    return;
                }
                this.UpdateEmailingTimes(no);
                MessageBox.Show("完成發送 Email 提醒通知，將開啟 Log 記錄。");
                (new frmEMailingLog(this.guid)).ShowDialog();
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void SendMail(Dictionary<UDT.AssignedSurvey, dynamic> dicEmailInfos, int no, MandrillApi mandrill)
        {
            List<UDT.MandrillSendLog> MandrillSendLogs = new List<UDT.MandrillSendLog>();
            DateTime time_stamp = DateTime.Now;
            string user_account = FISCA.Authentication.DSAServices.UserAccount;

            try
            {
                UDT.CSConfiguration conf = new UDT.CSConfiguration();
                UDT.CSConfiguration conf_subject = new UDT.CSConfiguration();                    
                    
                if (no == 1)
                {
                    this.email_category = "教學評鑑意見調查EMAIL提醒";
                    conf = UDT.CSConfiguration.GetTemplate(UDT.CSConfiguration.TemplateName.教學評鑑意見調查EMAIL提醒);
                    conf_subject = UDT.CSConfiguration.GetTemplate(UDT.CSConfiguration.TemplateName.教學評鑑意見調查EMAIL提醒.ToString() + "_subject");
                }
                else
                {
                    this.email_category = "教學評鑑意見調查EMAIL再次提醒";
                    conf = UDT.CSConfiguration.GetTemplate(UDT.CSConfiguration.TemplateName.教學評鑑意見調查EMAIL再次提醒);
                    conf_subject = UDT.CSConfiguration.GetTemplate(UDT.CSConfiguration.TemplateName.教學評鑑意見調查EMAIL再次提醒.ToString() + "_subject");
                }

                string content = conf.Content;
                string subject = conf_subject.Content;
                foreach (UDT.AssignedSurvey AssignedSurvey in dicEmailInfos.Keys)
                {
                    dynamic o = dicEmailInfos[AssignedSurvey];

                    string subject_name = o.SubjectName;
                    string course_name = o.CourseName;
                    string teacher_name = o.TeacherName;
                    string school_year = o.SchoolYear + "";
                    string semester = o.Semester; 

                    string begin_time = string.Empty;
                    string end_time = string.Empty;

                    DateTime begin_time_date_time;
                    DateTime end_time_date_time;

                    if (DateTime.TryParse(o.SurveyBeginTime, out begin_time_date_time))
                        begin_time = begin_time_date_time.ToString("yyyy/MM/dd HH:mm:ss");
                    if (DateTime.TryParse(o.SurveyEndTime, out end_time_date_time))
                        end_time = end_time_date_time.ToString("yyyy/MM/dd HH:mm:ss");

                    Dictionary<StudentRecord, IEnumerable<string>> dicStudentEmails = o.EmailList as Dictionary<StudentRecord, IEnumerable<string>>;
                    string email_subject = string.Empty;
                    string email_body = string.Empty;
                    foreach (StudentRecord Student in dicStudentEmails.Keys)
                    {
                        string student_name = Student.Name;
                        //學生姓名  課程  開課  授課教師    學年度 學期  問卷填寫開始時間    問卷填寫結束時間
                        email_subject = subject.Replace("[[學生姓名]]", student_name).Replace("[[課程]]", subject_name).Replace("[[開課]]", course_name).Replace("[[授課教師]]", teacher_name).Replace("[[學年度]]", school_year).Replace("[[學期]]", semester).Replace("[[問卷填寫開始時間]]", begin_time).Replace("[[問卷填寫結束時間]]", end_time); 
                        email_body = content.Replace("[[學生姓名]]", student_name).Replace("[[課程]]", subject_name).Replace("[[開課]]", course_name).Replace("[[授課教師]]", teacher_name).Replace("[[學年度]]", school_year).Replace("[[學期]]", semester).Replace("[[問卷填寫開始時間]]", begin_time).Replace("[[問卷填寫結束時間]]", end_time);
                        if (dicStudentEmails[Student].Count() > 0)
                            this.SendSingleMail(email_subject, email_body, dicStudentEmails[Student], mandrill, MandrillSendLogs, o, Student, false);
                    }
                    email_subject = subject.Replace("[[學生姓名]]", string.Empty).Replace("[[課程]]", subject_name).Replace("[[開課]]", course_name).Replace("[[授課教師]]", teacher_name).Replace("[[學年度]]", school_year).Replace("[[學期]]", semester).Replace("[[問卷填寫開始時間]]", begin_time).Replace("[[問卷填寫結束時間]]", end_time) + "【副本】";
                    email_body = content.Replace("[[學生姓名]]", string.Empty).Replace("[[課程]]", subject_name).Replace("[[開課]]", course_name).Replace("[[授課教師]]", teacher_name).Replace("[[學年度]]", school_year).Replace("[[學期]]", semester).Replace("[[問卷填寫開始時間]]", begin_time).Replace("[[問卷填寫結束時間]]", end_time);
                    if (this.validated_cc.Count > 0)
                        this.SendSingleMail(email_subject, email_body, this.validated_cc, mandrill, MandrillSendLogs, o, null, true);
                    //更新 email 時間
                    //if (updatedRecs.Count > 0)
                    //{
                    //    (new AccessHelper()).SaveAll(updatedRecs);
                    //}
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                MandrillSendLogs.SaveAll();
            }
        }

        private void SendSingleMail(string email_subject, string email_body, IEnumerable<string> email_list, MandrillApi mandrill, List<UDT.MandrillSendLog> MandrillSendLogs, dynamic o, StudentRecord Student, bool IsCC)
        {
            if (email_list.Count() == 0)
                return;

            string current_email = string.Empty;
            try
            {
                //  gmail 改為 mandrill

                //System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                //System.Net.NetworkCredential cred = new System.Net.NetworkCredential(this.SenderEmail, this.SenderPassword);

                //mail.Subject = email_subject;
                //mail.From = new System.Net.Mail.MailAddress(this.SenderEmail);

                //foreach (string email_address in email_list)
                //    mail.To.Add(email_address);

                //mail.IsBodyHtml = true;
                //mail.Body = email_body;

                //System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                //smtp.UseDefaultCredentials = false;
                //smtp.EnableSsl = true;
                //smtp.Credentials = cred;
                //smtp.Port = 587;
                //smtp.Send(mail);

                //mandrill = new MandrillApi(this.MandrillAPIKey, false);


                List<EmailAddress> EmailAddresss = new List<EmailAddress>();
                foreach (string mail_to in email_list)
                {
                    current_email = mail_to;

                    EmailMessage message = new EmailMessage();
                    message.auto_text = false;
                    message.from_email = this.from_email;
                    message.from_name = this.from_name;
                    EmailAddress mt = new EmailAddress();

                    mt.email = mail_to;
                    mt.name = string.Empty;
                    message.to = new List<EmailAddress>(){ mt };

                    message.track_clicks = false;
                    message.track_opens = false;
                    message.html = email_body;
                    message.important = false;
                    message.merge = false;
                    message.preserve_recipients = false;
                    message.subject = email_subject;

                    List<EmailResult> results = mandrill.SendMessageSync(message);

                    //  Log Email Result
                    foreach (EmailResult result in results)
                    {
                        UDT.MandrillSendLog MandrillSendLog = new UDT.MandrillSendLog();

                        if (Student != null)
                        {
                            MandrillSendLog.StudentID = int.Parse(Student.ID);
                            MandrillSendLog.StudentNumber = Student.StudentNumber;
                            MandrillSendLog.StudentName = Student.Name;
                        }

                        MandrillSendLog.CourseID = int.Parse(o.CourseID + "");
                        MandrillSendLog.CourseName = o.CourseName + "";
                        MandrillSendLog.SubjectID = int.Parse(o.SubjectID + "");
                        MandrillSendLog.SubjectName = o.SubjectName + "";
                        MandrillSendLog.TeacherID = int.Parse(o.TeacherID + "");
                        MandrillSendLog.TeacherName = o.TeacherName + "";
                        MandrillSendLog.SchoolYear = o.SchoolYear + "";
                        MandrillSendLog.Semester = o.Semester + "";
                        MandrillSendLog.SurveyBeginTime = o.SurveyBeginTime + "";
                        MandrillSendLog.SurveyEndTime = o.SurveyEndTime + "";
                        MandrillSendLog.IsCC = IsCC;

                        MandrillSendLog.CCEmailAddress = string.Empty;
                        MandrillSendLog.CurrentUserAccount = this.user_account;
                        MandrillSendLog.Device = "desktop";
                        MandrillSendLog.EmailCategory = this.email_category;
                        MandrillSendLog.GUID = this.guid;
                        MandrillSendLog.RecipientEmailAddress = mail_to;// string.Join(",", email_list);
                        MandrillSendLog.RejectReason = result.RejectReason;
                        MandrillSendLog.ResultEmail = result.Email;
                        MandrillSendLog.ResultID = result.Id;
                        MandrillSendLog.SenderEmailAddress = this.from_email;
                        MandrillSendLog.SenderName = this.from_name;
                        MandrillSendLog.TimeStamp = this.time_stamp;
                        MandrillSendLog.Source = string.Empty;

                        MandrillSendLogs.Add(MandrillSendLog);
                    }

                    //EmailAddresss.Add(mt);
                }
            }
            //  Log Email Error
            catch (Exception ex)
            {
                MandrillException error = (MandrillException)ex;
                UDT.MandrillSendLog MandrillSendLog = new UDT.MandrillSendLog();

                if (Student != null)
                {
                    MandrillSendLog.StudentID = int.Parse(Student.ID);
                    MandrillSendLog.StudentNumber = Student.StudentNumber;
                    MandrillSendLog.StudentName = Student.Name;
                }

                MandrillSendLog.CourseID = int.Parse(o.CourseID + "");
                MandrillSendLog.CourseName = o.CourseName + "";
                MandrillSendLog.SubjectID = int.Parse(o.SubjectID + "");
                MandrillSendLog.SubjectName = o.SubjectName + "";
                MandrillSendLog.TeacherID = int.Parse(o.TeacherID + "");
                MandrillSendLog.TeacherName = o.TeacherName + "";
                MandrillSendLog.SchoolYear = o.SchoolYear + "";
                MandrillSendLog.Semester = o.Semester + "";
                MandrillSendLog.SurveyBeginTime = o.SurveyBeginTime + "";
                MandrillSendLog.SurveyEndTime = o.SurveyEndTime + "";
                MandrillSendLog.IsCC = IsCC;
                
                MandrillSendLog.CCEmailAddress = string.Empty;
                MandrillSendLog.CurrentUserAccount = this.user_account;
                MandrillSendLog.Device = "desktop";
                MandrillSendLog.EmailCategory = this.email_category;
                MandrillSendLog.GUID = this.guid;
                MandrillSendLog.RecipientEmailAddress = current_email;// string.Join(",", email_list);
                MandrillSendLog.ErrorCode = error.Error.code;
                MandrillSendLog.ErrorMessage = error.Error.message;
                MandrillSendLog.ErrorName = error.Error.name;
                MandrillSendLog.ErrorStatus = error.Error.status;
                MandrillSendLog.SenderName = this.from_name;
                MandrillSendLog.SenderEmailAddress = this.from_email;
                MandrillSendLog.TimeStamp = this.time_stamp;
                MandrillSendLog.Source = error.ToString();

                MandrillSendLogs.Add(MandrillSendLog);
            }
        }

        private void UpdateEmailingTimes(int no)
        {
            if (no == 1)
                UpdateEmailingTimes();
            else
                UpdateSecondEmailingTimes();
        }

        private void UpdateEmailingTimes()
        {
            DateTime just_now = DateTime.Now;
            foreach (DataGridViewRow row in this.dgvData.SelectedRows)
            {
                UDT.AssignedSurvey AssignedSurvey = row.Tag as UDT.AssignedSurvey;

                row.Cells[10].Value = just_now.ToString("yyyy/MM/dd HH:mm:ss");
                AssignedSurvey.EmailTime = just_now;

                AssignedSurvey.Save();
            }
        }
    
        private void UpdateSecondEmailingTimes()
        {
            foreach (DataGridViewRow row in this.dgvData.SelectedRows)
            {
                UDT.AssignedSurvey AssignedSurvey = row.Tag as UDT.AssignedSurvey;

                DateTime just_now = DateTime.Now;

                row.Cells[11].Value = just_now.ToString("yyyy/MM/dd HH:mm:ss");
                AssignedSurvey.SecondEmailTime = just_now;

                AssignedSurvey.Save();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool isOk = true;

            //Validate SenderEMail
            if (string.IsNullOrWhiteSpace(this.txtSenderEMail.Text))
            {
                this.errorProvider1.SetError(this.txtSenderEMail.TextBox, "必填。");
                isOk = false;
            }
            else
            {
                if (!this.isValidEmail(this.txtSenderEMail.Text.Trim()))
                {
                    this.errorProvider1.SetError(this.txtSenderEMail.TextBox, string.Format("不正確的電子郵件格式。"));
                    isOk = false;
                }
                else
                    this.errorProvider1.SetError(this.txtSenderEMail.TextBox, "");
            }

            //Validate SenderName
            if (string.IsNullOrWhiteSpace(this.txtSenderName.Text))
            {
                this.errorProvider1.SetError(this.txtSenderName.TextBox, "必填。");
                isOk = false;
            }
            else
            {
                this.errorProvider1.SetError(this.txtSenderName.TextBox, "");
            }

            //  驗證副本是否符合電子郵件格式
            string[] ccs = this.txtCC.Text.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> err_emails = new List<string>();
            foreach (string cc in ccs)
            {
                if (cc.Trim().Length > 0)
                {
                    if (!this.isValidEmail(cc))
                        err_emails.Add(cc);
                }
            }
            if (err_emails.Count > 0)
            {
                this.errorProvider1.SetError(this.txtCC.TextBox, string.Format("副本『{0}』是不正確的電子郵件格式。", string.Join(",", err_emails)));
                isOk = false;
            }
            else
                this.errorProvider1.SetError(this.txtCC.TextBox, "");

            if (isOk)
            {
                this.from_email = this.txtSenderEMail.Text.Trim();
                this.from_name = this.txtSenderName.Text.Trim();
                this.validated_cc = ccs.ToList();

                if (config != null)
                {
                    config["SenderEMail"] = this.txtSenderEMail.Text.Trim();
                    config["SenderName"] = this.txtSenderName.Text.Trim();
                    config["CC"] = this.txtCC.Text.Trim();

                    config.Save();
                }
                this.lblMessage.Text = "儲存成功";
                Timer timer = new Timer();
                timer.Enabled = true;
                timer.Interval = 3000;
                timer.Tick += (x, y) => {
                    this.lblMessage.Text = string.Empty;
                    timer.Enabled = false;
                };
                buttonX1.Expanded = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.txtSenderEMail.Text = this.from_email;
            this.txtSenderName.Text = this.from_name;
            this.txtCC.Text = string.Join(",", this.validated_cc);
            this.buttonX1.Expanded = false;
        }

        //private void btnSend_Click(object sender, EventArgs e)
        //{
        //    MandrillApi mandrill = new MandrillApi(this.MandrillAPIKey, false);

        //    string pong = mandrill.Ping();

        //    if (!pong.ToUpper().Contains("PONG!"))
        //    {
        //        MessageBox.Show("無效的 MandrillAPIKey。");
        //        return;
        //    }

        //    EmailMessage message = new EmailMessage();
        //    message.auto_text = false;
        //    message.from_email = "paul.wang@ischool.com.tw";
        //    message.from_name = "EMBA辦公室";

        //    string[] mail_tos = this.txtAccount.Text.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        //    List<EmailAddress> EmailAddresss = new List<EmailAddress>();
        //    foreach (string mail_to in mail_tos)
        //    {
        //        EmailAddress mt = new EmailAddress();

        //        mt.email = mail_to;
        //        mt.name = string.Empty;

        //        EmailAddresss.Add(mt);
        //    }
        //    message.to = EmailAddresss;

        //    message.track_clicks = true;
        //    message.track_opens = true;
        //    message.html = webBrowser1.Document.Body.InnerHtml;
        //    message.important = true;
        //    message.merge = false;
        //    message.preserve_recipients = true;
        //    message.subject = this.txtSubject.Text.Trim();

        //    List<EmailResult> results = mandrill.SendMessageSync(message);
        //}

    }
}
