using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Campus.Windows;
using FISCA.Data;
using FISCA.Permission;
using FISCA.UDT;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Dynamic;

namespace TeachingEvaluation.DetailItems
{
    [AccessControl("ischool.EMBA.DetailContent.Teacher_EvaluationProgressEnquiry", "教學評鑑", "教師>資料項目")]
    public partial class Teaching_EvaluationProgressEnquiry : DetailContentImproved
    {
        private AccessHelper Access;
        private QueryHelper Query;
        private Dictionary<string, dynamic> dicRowData;
        private int school_year;
        private int semester;
        private bool form_loaded;

        public Teaching_EvaluationProgressEnquiry()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.Form_Load);
            this.Group = "教學評鑑";
        }

        private void Form_Load(object sender, EventArgs e)
        {
            //  DataGridView 事件
            this.dgvData.DataError += new DataGridViewDataErrorEventHandler(dgvData_DataError);
            this.dgvData.CurrentCellDirtyStateChanged += new EventHandler(dgvData_CurrentCellDirtyStateChanged);
            this.dgvData.CellEnter += new DataGridViewCellEventHandler(dgvData_CellEnter);
            this.dgvData.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvData_ColumnHeaderMouseClick);
            this.dgvData.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvData_RowHeaderMouseClick);
            this.dgvData.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvData_MouseClick);

            this.WatchChange(new DataGridViewSource(this.dgvData));

            Access = new AccessHelper();
            Query = new QueryHelper();
            //  事件監聽
            //  1、班級異動
            //  2、組別異動
            //Course.AfterChange += (x, y) => ReInitialize();
            form_loaded = false;
            this.InitSchoolYear();
            this.InitSemester();
            form_loaded = true;
        }

        protected override void OnInitializeAsync()
        {

        }

        private void InitSchoolYear()
        {
            if (int.TryParse(K12.Data.School.DefaultSchoolYear, out school_year))
            {
                this.nudSchoolYear.Value = decimal.Parse(school_year.ToString());
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

        protected override void OnInitializeComplete(Exception error)
        {
            if (error != null) //有錯就直接丟出去吧。
                throw error;
        }

        protected override void OnPrimaryKeyChangedAsync()
        {
            string SQL = string.Empty;

            DataTable dataTable = new DataTable();
            dicRowData = new Dictionary<string, dynamic>();

            #region 教師授課之課程名稱
            SQL = string.Format("SELECT  distinct teacher.id as teacher_id, teacher.teacher_name, course.id as course_id, course.course_name FROM course LEFT JOIN $ischool.emba.course_instructor ON $ischool.emba.course_instructor.ref_course_id = course.id LEFT JOIN teacher ON teacher.id = $ischool.emba.course_instructor.ref_teacher_id LEFT JOIN tag_teacher ON tag_teacher.ref_teacher_id = teacher.id LEFT JOIN tag ON tag.id = tag_teacher.ref_tag_id WHERE tag.category = 'Teacher' AND tag.prefix = '教師' AND course.school_year={0} AND course.semester={1} AND teacher.id={2} order by course.id", school_year, semester, PrimaryKey);
            dataTable = Query.Select(SQL);
            foreach (DataRow row in dataTable.Rows)
            {
                if (!dicRowData.ContainsKey(row["course_id"] + ""))
                    dicRowData.Add(row["course_id"] + "", new ExpandoObject());

                dicRowData[row["course_id"] + ""].CourseName = row["course_name"] + "";
            }

            #endregion

            #region 授課教師使用個案之英文名稱
            SQL = string.Format("select teacher.id as teacher_id, course.id as course_id, cazz.uid as case_id, cazz.english_name, cazz.name as case_name from $ischool.emba.case_management.case_usage as cu join teacher on cu.ref_teacher_id=teacher.id join course on course.id=cu.ref_course_id join $ischool.emba.case_management.case as cazz on cazz.uid=cu.ref_case_id where teacher.id={0}", PrimaryKey);
            dataTable = Query.Select(SQL);
            foreach (DataRow row in dataTable.Rows)
            {
                if (!dicRowData.ContainsKey(row["course_id"] + ""))
                    continue;

                dicRowData[row["course_id"] + ""].CaseEnglishName = row["english_name"] + "";
                dicRowData[row["course_id"] + ""].CaseName = row["case_name"] + "";
            }
            #endregion

            #region 授課教師採用問卷之問卷名稱、評鑑開始時間、評鑑結束時間
            SQL = string.Format("select survey.name, survey.uid, course.id as course_id, teacher.id as teacher_id, asu.opening_time, end_time from $ischool.emba.teaching_evaluation.assigned_survey as asu join $ischool.emba.teaching_evaluation.survey as survey on survey.uid=asu.ref_survey_id join teacher on teacher.id=asu.ref_teacher_id join course on course.id=asu.ref_course_id where teacher.id={0}", PrimaryKey);
            dataTable = Query.Select(SQL);
            foreach (DataRow row in dataTable.Rows)
            {
                if (!dicRowData.ContainsKey(row["course_id"] + ""))
                    continue;

                dicRowData[row["course_id"] + ""].SurveyName = row["name"] + "";
                dicRowData[row["course_id"] + ""].BeginTime = DateTime.Parse(row["opening_time"] + "").ToString("yyyy/MM/dd HH:mm");
                dicRowData[row["course_id"] + ""].EndTime = DateTime.Parse(row["end_time"] + "").ToString("yyyy/MM/dd HH:mm");
            }
            #endregion

            #region 修課學生數
            SQL = string.Format("select course.id as course_id, count(ref_student_id) as attend_no from course join $ischool.emba.scattend_ext as se on se.ref_course_id=course.id where is_cancel=false and course.school_year={0} and course.semester={1} Group by course.id", school_year, semester);
            dataTable = Query.Select(SQL);
            foreach (DataRow row in dataTable.Rows)
            {
                if (!dicRowData.ContainsKey(row["course_id"] + ""))
                    continue;

                dicRowData[row["course_id"] + ""].CSAttendNo = row["attend_no"] + "";
            }
            #endregion

            #region 填寫問卷數
            SQL = string.Format("select count(reply.ref_student_id) as reply_no, reply.ref_course_id as course_id, reply.ref_teacher_id as teacher_id from  course join $ischool.emba.teaching_evaluation.reply as reply on course.id=reply.ref_course_id join teacher on teacher.id=reply.ref_teacher_id where reply.status=1 and course.school_year={0} and course.semester={1} and teacher.id={2} group by reply.ref_course_id, reply.ref_teacher_id", school_year, semester, PrimaryKey);
            dataTable = Query.Select(SQL);
            foreach (DataRow row in dataTable.Rows)
            {
                if (!dicRowData.ContainsKey(row["course_id"] + ""))
                    continue;

                dicRowData[row["course_id"] + ""].FeedBackNo = row["reply_no"] + "";
            }
            #endregion
        }

        protected override void OnPrimaryKeyChangedComplete(Exception error)
        {
            this.dgvData.Rows.Clear();

            if (error != null) //有錯就直接丟出去吧。
                throw error;

            ErrorTip.Clear();

            this.GridViewDataBinding();

            ResetDirtyStatus();            
        }

        private void GridViewDataBinding()
        {
            foreach (string key in this.dicRowData.Keys)
            {
                dynamic o = this.dicRowData[key];

                List<object> source = new List<object>();

                source.Add(o.CourseName);
                if (((IDictionary<String, object>)o).ContainsKey("CaseEnglishName"))
                    source.Add(o.CaseEnglishName);
                else
                    source.Add(string.Empty);
                if (((IDictionary<String, object>)o).ContainsKey("SurveyName"))
                    source.Add(o.SurveyName);
                else
                    source.Add(string.Empty);
                if (((IDictionary<String, object>)o).ContainsKey("BeginTime"))
                    source.Add(o.BeginTime);
                else
                    source.Add(string.Empty);
                if (((IDictionary<String, object>)o).ContainsKey("EndTime"))
                    source.Add(o.EndTime);
                else
                    source.Add(string.Empty);

                int intCSAttendNo = 0;
                int intFeedBackNo = 0;

                if (((IDictionary<String, object>)o).ContainsKey("CSAttendNo"))
                {
                    source.Add(o.CSAttendNo);
                    int.TryParse(o.CSAttendNo + "", out intCSAttendNo);
                }
                else
                    source.Add(string.Empty);

                if (((IDictionary<String, object>)o).ContainsKey("FeedBackNo"))
                {
                    source.Add(o.FeedBackNo);
                    int.TryParse(o.FeedBackNo + "", out intFeedBackNo);
                }
                else
                    source.Add(string.Empty);

                if (intCSAttendNo == 0 || intFeedBackNo == 0)
                    source.Add("未達半數");
                else
                {
                    if ((intCSAttendNo) <= intFeedBackNo * 2)
                        source.Add(string.Empty);
                    else
                        source.Add("未達半數");
                }
                int idx = this.dgvData.Rows.Add(source.ToArray());
            }
        }
        
        //protected override void OnCancelButtonClick(EventArgs e)
        //{
        //    base.OnCancelButtonClick(e);
        //}

        //protected override void OnSaveData()
        //{            
        //    ResetDirtyStatus();
        //}

        protected override void OnDirtyStatusChanged(ChangeEventArgs e)
        {
            if (UserAcl.Current[this.GetType()].Editable)
                SaveButtonVisible = e.Status == ValueStatus.Dirty;
            else
                this.SaveButtonVisible = false;

            CancelButtonVisible = e.Status == ValueStatus.Dirty;
        }

        private void dgvData_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void dgvData_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvData.CommitEdit(DataGridViewDataErrorContexts.Commit);

            //this.Varify(sender as DataGridView);
        }
        
        private void dgvData_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgvData.SelectedCells.Count == 1)
            {
                dgvData.BeginEdit(true);
                //if (dgvData.CurrentCell != null && dgvData.CurrentCell.GetType().ToString() == "System.Windows.Forms.DataGridViewComboBoxCell")
                //    (dgvData.EditingControl as ComboBox).DroppedDown = true;  //自動拉下清單
            }
        }

        private void dgvData_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvData.CurrentCell = null;
            dgvData.Rows[e.RowIndex].Selected = true;
        }

        private void dgvData_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvData.CurrentCell = null;
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

        private void nudSchoolYear_ValueChanged(object sender, EventArgs e)
        {
            this.school_year = int.Parse(this.nudSchoolYear.Value + "");
            if (form_loaded)
                this.OnPrimaryKeyChanged(null);
        }

        private void cboSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.semester = int.Parse((this.cboSemester.SelectedItem as DataItems.SemesterItem).Value);
            if (form_loaded)
                this.OnPrimaryKeyChanged(null);
        }
    }
}
