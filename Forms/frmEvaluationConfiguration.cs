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
using System.Net.Mail;
using System.Dynamic;
using System.Text.RegularExpressions;

namespace TeachingEvaluation.Forms
{
    public partial class frmEvaluationConfiguration : BaseForm
    {
        private AccessHelper Access;
        private QueryHelper Query;

        private bool QueryMode;
        private bool form_loaded;

        private Dictionary<string, List<string>> dicTeachersCases;
        private Dictionary<string, UDT.Reply> dicReplys;
        private Dictionary<string, CourseRecord> dicCourses;
        private Dictionary<string, KeyValuePair<string, string>> dicCourseInstructors;
        private Dictionary<string, List<string>> dicCases;
        private Dictionary<string, UDT.AssignedSurvey> dicAssignedSurveys;
        private Dictionary<string, UDT.Survey> dicSurveys;
        
        public frmEvaluationConfiguration(bool QueryMode = true)
        {
            InitializeComponent();

            this.dgvData.CurrentCellDirtyStateChanged += new EventHandler(dgvData_CurrentCellDirtyStateChanged);
            this.dgvData.CellEnter += new DataGridViewCellEventHandler(dgvData_CellEnter);
            this.dgvData.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgvData_EditingControlShowing);
            this.dgvData.DataError += new DataGridViewDataErrorEventHandler(dgvData_DataError);
            this.dgvData.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvData_ColumnHeaderMouseClick);
            this.dgvData.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvData_RowHeaderMouseClick);
            this.dgvData.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvData_MouseClick);

            this.Load += new EventHandler(frmTeachingEvaluation_Load);
            dgvData.SortCompare += new DataGridViewSortCompareEventHandler(
                this.DataGridView_SortCompare);

            this.dicTeachersCases = new Dictionary<string, List<string>>();
            this.dicReplys = new Dictionary<string, UDT.Reply>();
            this.dicCourses = new Dictionary<string, CourseRecord>();
            this.dicCourseInstructors = new Dictionary<string, KeyValuePair<string, string>>();
            this.dicCases = new Dictionary<string, List<string>>();
            this.dicSurveys = new Dictionary<string, UDT.Survey>();
            this.dicAssignedSurveys = new Dictionary<string, UDT.AssignedSurvey>();

            this.QueryMode = QueryMode;

            Access = new AccessHelper();
            Query = new QueryHelper();
        }

        private void dgvData_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvData.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvData_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgvData.SelectedCells.Count == 1)
            //{
            //    dgvData.BeginEdit(true);
            //}
        }

        private void frmTeachingEvaluation_Load(object sender, EventArgs e)
        {
            if (this.QueryMode)
            {
                this.btnSet.Visible = false;
                this.Text = "查詢問卷組態";
            }
            else
            {
                this.btnSet.Visible = true;
                this.Text = "設定問卷組態";
            }

            this.circularProgress.Visible = true;
            this.circularProgress.IsRunning = true;

            this.InitSchoolYear();
            this.InitSemester();
            int SchoolYear = int.Parse(this.nudSchoolYear.Value.ToString());
            int Semester = int.Parse(this.cboSemester.SelectedValue + "");

            Task task = Task.Factory.StartNew(() =>
            {
                this.PrepareData();
            });
            task.ContinueWith((x) =>
            {
                if (x.Exception != null)
                {
                    MessageBox.Show(x.Exception.InnerException.Message);
                    goto TheEnd;
                }

                this.InitCourse(this.dicCourses.Values.Where(y => (y.SchoolYear.HasValue && y.Semester.HasValue)).Where(y => (y.SchoolYear.Value == SchoolYear && y.Semester.Value == Semester)));
                this.InitMenuItem();
                this.Template.DataSource = (new List<UDT.Survey>() { new UDT.Survey() }).Union(this.dicSurveys.Values).ToList();
                this.Template.ValueMember = "UID";
                this.Template.DisplayMember = "Name";
                this.DGV_DataBinding(this.cboCourse.Items[0] as ComboItem);

            TheEnd:
                this.circularProgress.Visible = false;
                this.circularProgress.IsRunning = false;

                form_loaded = true;
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void DataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 3 || e.Column.Index == 4)
            {
                DateTime date_time_1 = new DateTime(1, 1, 1);
                DateTime date_time_2 = new DateTime(1, 1, 1);

                DateTime.TryParse(e.CellValue1 + "", out date_time_1);
                DateTime.TryParse(e.CellValue2 + "", out date_time_2);
                e.SortResult = System.DateTime.Compare(date_time_1, date_time_2);
            }
            else
                e.SortResult = System.String.Compare(e.CellValue1 + "", e.CellValue2 + "");

            // If the cells are equal, sort based on the ID column.
            //if (e.SortResult == 0 && e.Column.Name != "ID")
            //{
            //    e.SortResult = System.String.Compare(
            //        dgvData.Rows[e.RowIndex1].Cells["ID"].Value.ToString(),
            //        dgvData.Rows[e.RowIndex2].Cells["ID"].Value.ToString());
            //}
            e.Handled = true;
        }

        private void PrepareData()
        {
            //  問卷
            List<UDT.Survey> Surveys = Access.Select<UDT.Survey>();
            if (Surveys.Count > 0)
                this.dicSurveys = Surveys.ToDictionary(x => x.UID);

            //  課程
            List<CourseRecord> Courses = K12.Data.Course.SelectAll();
            if (Courses.Count > 0)
                dicCourses = Courses.ToDictionary(x => x.ID);

            //  問卷做答
            List<UDT.Reply> Replys = Access.Select<UDT.Reply>();
            if (Replys.Count > 0)
            {
                Replys.ForEach((x) =>
                {
                    string key = x.CourseID + "-" + x.TeacherID + "-" + x.SurveyID;

                    if (!this.dicReplys.ContainsKey(key))
                        this.dicReplys.Add(key, x);
                });
            }

            //  授課教師
            DataTable dataTable = Query.Select(string.Format(@"select course.id as course_id, teacher.id as teacher_id, course.course_name, teacher.teacher_name from course join $ischool.emba.course_instructor as ci on course.id=ci.ref_course_id join teacher on teacher.id=ci.ref_teacher_id where teacher.id in (SELECT  distinct teacher.id FROM course LEFT JOIN $ischool.emba.course_instructor ON $ischool.emba.course_instructor.ref_course_id = course.id LEFT JOIN teacher ON teacher.id = $ischool.emba.course_instructor.ref_teacher_id LEFT JOIN tag_teacher ON tag_teacher.ref_teacher_id = teacher.id LEFT JOIN tag ON tag.id = tag_teacher.ref_tag_id WHERE tag.category = 'Teacher' AND tag.prefix = '教師') and course.id in ({0}) order by course.course_name, teacher.teacher_name", this.dicCourses.Keys.Count == 0 ? "0" : string.Join(",", dicCourses.Keys)));
            foreach (DataRow row in dataTable.Rows)
            {
                if (!dicCourseInstructors.ContainsKey(row["course_id"] + "-" + row["teacher_id"]))
                    dicCourseInstructors.Add(row["course_id"] + "-" + row["teacher_id"], new KeyValuePair<string, string>(row["course_name"] + "", row["teacher_name"] + ""));
            }

            //  個案
            dataTable = Query.Select(string.Format(@"select course.id as course_id, teacher.id as teacher_id, cazz.uid as case_id, cazz.name as case_name, cazz.english_name as case_english_name from $ischool.emba.case_management.case as cazz join $ischool.emba.case_management.case_usage as case_usage on case_usage.ref_case_id=cazz.uid join teacher on teacher.id=case_usage.ref_teacher_id join course on course.id=case_usage.ref_course_id"));
            foreach (DataRow row in dataTable.Rows)
            {
                if (!this.dicCases.ContainsKey(row["course_id"] + "-" + row["teacher_id"]))
                    this.dicCases.Add(row["course_id"] + "-" + row["teacher_id"], new List<string>());

                string case_name = string.IsNullOrWhiteSpace(row["case_english_name"] + "") ? row["case_name"] + "" : row["case_english_name"] + "";
                this.dicCases[row["course_id"] + "-" + row["teacher_id"]].Add(case_name);
            }

            //  問卷組態
            List<UDT.AssignedSurvey> AssignedSurveys = Access.Select<UDT.AssignedSurvey>();
            foreach (UDT.AssignedSurvey AssegnedSurvey in AssignedSurveys)
            {
                if (!this.dicAssignedSurveys.ContainsKey(AssegnedSurvey.CourseID + "-" + AssegnedSurvey.TeacherID))
                    this.dicAssignedSurveys.Add(AssegnedSurvey.CourseID + "-" + AssegnedSurvey.TeacherID, AssegnedSurvey);
            }
        }

        private void dgvData_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                ComboBox comboBox = e.Control as ComboBox;
            }
        }

        private bool DeletingAssignedSurveyIsReplyed(DataGridViewRow row)
        {
            //UDT.AssignedSurvey AssignedSurvey = row.Tag as UDT.AssignedSurvey;
            //if (AssignedSurvey == null)
            //    return true;

            string course_id = row.Cells[0].Tag + "";
            string teacher_id = row.Cells[1].Tag + "";
            string Nsurvey_id = row.Cells[5].Value + "";
            UDT.Survey Survey = row.Cells[5].Tag as UDT.Survey;
            string Osurvey_id = (Survey == null ? string.Empty : Survey.UID);

            string key = course_id + "-" + teacher_id + "-" + Osurvey_id;

            if (this.dicReplys.ContainsKey(key))
            {
                if (Nsurvey_id != Osurvey_id)
                    return true;
                else
                    return false;
            }
            else
                return false;
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

        private void dgvData_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void InitMenuItem()
        {
            foreach (UDT.Survey Survey in this.dicSurveys.Values)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(Survey.Name);
                item.ShowShortcutKeys = false;
                item.Tag = Survey;
                item.Click += new System.EventHandler(this.toolStripMenuItem_Click);
                this.ToolStripMenuItem_AssignedTemplate.DropDownItems.Add(item);
            }
        }

        private void toolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dgvData.SelectedRows.Count == 0)
                return;

            this.dgvData.EndEdit();
            UDT.Survey Survey = (sender as ToolStripMenuItem).Tag as UDT.Survey;
            foreach (DataGridViewRow row in this.dgvData.SelectedRows)
            {
                row.Cells[5].Value = Survey.UID;
                row.Cells[5].Tag = Survey;
                row.Cells[6].Value = Survey.Description;
            }
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

        private void InitCourse(IEnumerable<CourseRecord> SelectedCourses)
        {
            this.cboCourse.Items.Clear();

            //if (SelectedCourses.Count() == 0)
            //    return;

            ComboItem comboItem1 = new ComboItem("全部");
            comboItem1.Tag = string.Empty;
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

            if (this.dicCourses.Count == 0)
                return;

            int SchoolYear = int.Parse(this.nudSchoolYear.Value.ToString());
            int Semester = int.Parse(this.cboSemester.SelectedValue + "");

            this.InitCourse(this.dicCourses.Values.Where(x => (x.SchoolYear.HasValue && x.Semester.HasValue)).Where(x => (x.SchoolYear.Value == SchoolYear && x.Semester.Value == Semester)));
        }

        private void cboSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.form_loaded)
                return;

            if (this.dicCourses.Count == 0)
                return;

            int SchoolYear = int.Parse(this.nudSchoolYear.Value.ToString());
            int Semester = int.Parse(this.cboSemester.SelectedValue + "");

            this.InitCourse(this.dicCourses.Values.Where(x => (x.SchoolYear.HasValue && x.Semester.HasValue)).Where(x => (x.SchoolYear.Value == SchoolYear && x.Semester.Value == Semester)));
        }

        private void cboCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.form_loaded)
                return;

            if (this.cboCourse.SelectedIndex == -1)
                return;

            ComboItem item = this.cboCourse.Items[this.cboCourse.SelectedIndex] as ComboItem;
            this.DGV_DataBinding(item);
        }

        private void DGV_DataBinding(ComboItem item)
        {
            List<string> SelectedCourseIDs = new List<string>();

            if (!string.IsNullOrEmpty(item.Tag + ""))
                this.DGV_DataBinding(new List<string> { item.Tag + "" });
            else
                this.DGV_DataBinding(this.cboCourse.Items.Cast<ComboItem>().ToList().Where(x => !string.IsNullOrEmpty(x.Tag + "")).Select(x => x.Tag + ""));
        }

        private void DGV_DataBinding(IEnumerable<string> CourseIDs)
        {
            this.dgvData.Rows.Clear();

            foreach (string key in dicCourseInstructors.Keys)
            {
                char[] delimiterChars = { '-' };
                string course_id = key.Split(delimiterChars).ElementAt(0);
                string teacher_id = key.Split(delimiterChars).ElementAt(1);

                if (!CourseIDs.Contains(course_id))
                    continue;

                List<object> sources = new List<object>();

                sources.Add(dicCourseInstructors[key].Key);
                sources.Add(dicCourseInstructors[key].Value);

                if (dicCases.ContainsKey(course_id + "-" + teacher_id))
                {
                    List<string> cases = new List<string>();
                    int i = 0;
                    dicCases[course_id + "-" + teacher_id].ForEach((x) =>
                    {
                        //  Windows 環境用 0D 0A ，Unix 用 0A ，Mac 用 0D
                        //\n 0d(13)換行
                        //\r  0a(10) 歸位
                        //  移除文字中換行符號
                        cases.Add(++i + "、" + x.Replace("\x0d", "").Replace("\x0a", ""));
                    });
                    sources.Add(string.Join("\r\n", cases));
                }
                else
                    sources.Add("");

                UDT.AssignedSurvey AssignedSurvey = null;
                if (dicAssignedSurveys.ContainsKey(key))
                    AssignedSurvey = dicAssignedSurveys[key];

                if (AssignedSurvey == null)
                    sources.Add("");
                else
                    sources.Add(AssignedSurvey.OpeningTime.ToString("yyyy/MM/dd HH:mm"));

                if (AssignedSurvey == null)
                    sources.Add("");
                else
                    sources.Add(AssignedSurvey.EndTime.ToString("yyyy/MM/dd HH:mm"));

                sources.Add("");

                if (AssignedSurvey == null)
                    sources.Add("");
                else
                    sources.Add(AssignedSurvey.Description);

                if (AssignedSurvey == null)
                    sources.Add("");
                else
                    sources.Add(AssignedSurvey.EmailTime.HasValue ? AssignedSurvey.EmailTime.Value.ToString("yyyy/MM/dd HH:mm:ss") : "");

                int idx = this.dgvData.Rows.Add(sources.ToArray());
                this.dgvData.Rows[idx].Tag = AssignedSurvey;

                if (AssignedSurvey != null)
                    if (!this.dicAssignedSurveys.ContainsKey(AssignedSurvey.UID))
                        this.dicAssignedSurveys.Add(AssignedSurvey.UID, AssignedSurvey);

                this.dgvData.Rows[idx].Cells[0].Tag = course_id;
                this.dgvData.Rows[idx].Cells[1].Tag = teacher_id;
                if (AssignedSurvey == null)
                    this.dgvData.Rows[idx].Cells[5].Tag = null;
                else
                {
                    if (this.dicSurveys.Where(y => y.Key == AssignedSurvey.SurveyID + "").Count() > 0)
                    {
                        this.dgvData.Rows[idx].Cells[5].Value = AssignedSurvey.SurveyID + "";
                        if (dicSurveys.ContainsKey(AssignedSurvey.SurveyID.ToString()))
                            this.dgvData.Rows[idx].Cells[5].Tag = dicSurveys[AssignedSurvey.SurveyID.ToString()];
                    }
                }
            }
        }

        private bool Is_Validated()
        {
            if (this.dgvData.CurrentRow != null)
                this.dgvData.CurrentRow.Selected = false;
            bool validated = true;
            this.dgvData.EndEdit();
            List<string> DeletingAssignedSurveyUIDs = new List<string>();
            foreach (DataGridViewRow row in this.dgvData.Rows)
            {
                if (row.IsNewRow)
                    continue;

                if (this.DeletingAssignedSurveyIsReplyed(row))
                {
                    row.Cells[5].ErrorText = "指定樣版已有學生做答，不得變更或移除。";
                    validated = false;
                }
                else
                    row.Cells[5].ErrorText = "";

                string cell_begin_date_time = row.Cells[3].Value + "";
                string cell_end_date_time = row.Cells[4].Value + "";
                string cell_survey_id = row.Cells[5].Value + "";
                string cell_description = row.Cells[6].Value + "";

                if (string.IsNullOrWhiteSpace(cell_begin_date_time + cell_end_date_time + cell_survey_id + cell_description))
                    continue;

                if (string.IsNullOrWhiteSpace(row.Cells[3].Value + ""))
                {
                    row.Cells[3].ErrorText = "必填。";
                    validated = false;
                }
                else
                    row.Cells[3].ErrorText = "";

                if (string.IsNullOrWhiteSpace(row.Cells[4].Value + ""))
                {
                    row.Cells[4].ErrorText = "必填。";
                    validated = false;
                }
                else
                    row.Cells[4].ErrorText = "";

                DateTime begin_date_time;
                DateTime end_date_time;

                if (!DateTime.TryParse((row.Cells[3].Value + "").Trim(), out begin_date_time))
                {
                    row.Cells[3].ErrorText = "格式錯誤。範例：2013/6/22 00:01。";
                    validated = false;
                }
                else
                    row.Cells[3].ErrorText = "";

                if (!DateTime.TryParse((row.Cells[4].Value + "").Trim(), out end_date_time))
                {
                    row.Cells[4].ErrorText = "格式錯誤。範例：2013/7/31 23:59。";
                    validated = false;
                }
                else
                    row.Cells[4].ErrorText = "";

                if (string.IsNullOrWhiteSpace(row.Cells[5].Value + ""))
                {
                    row.Cells[5].ErrorText = "必填。";
                    validated = false;
                }
                else
                {
                    if (row.Cells[5].ErrorText != "指定樣版已有學生做答，不得變更或移除。")
                        row.Cells[5].ErrorText = "";
                }
            }
            return validated;
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (!this.Is_Validated())
            {
                MessageBox.Show("請修正錯誤再儲存。");
                return;
            }
            this.btnSet.Enabled = false;
            List<UDT.AssignedSurvey> AssignedSurveys = new List<UDT.AssignedSurvey>();
            foreach (DataGridViewRow row in this.dgvData.Rows)
            {
                if (row.IsNewRow)
                    continue;

                //if (row.Cells[5].Tag == null)
                //    continue;

                string SurveyID = row.Cells[5].Value + "";
                int course_id = int.Parse(row.Cells[0].Tag + "");
                int teacher_id = int.Parse(row.Cells[1].Tag + "");

                UDT.AssignedSurvey AssignedSurvey = row.Tag as UDT.AssignedSurvey;
                if (AssignedSurvey == null)
                    AssignedSurvey = new UDT.AssignedSurvey();

                if (string.IsNullOrEmpty(SurveyID))
                    AssignedSurvey.Deleted = true;
                else
                {
                    AssignedSurvey.CourseID = course_id;
                    AssignedSurvey.TeacherID = teacher_id;
                    AssignedSurvey.SurveyID = int.Parse(SurveyID);
                    AssignedSurvey.OpeningTime = DateTime.Parse((row.Cells[3].Value + "").Trim());
                    AssignedSurvey.EndTime = DateTime.Parse((row.Cells[4].Value + "").Trim());
                    AssignedSurvey.Description = (row.Cells[6].Value + "").Trim();
                }

                AssignedSurveys.Add(AssignedSurvey);
            }
            AssignedSurveys.SaveAll();
            //  問卷組態
            AssignedSurveys = Access.Select<UDT.AssignedSurvey>();
            this.dicAssignedSurveys.Clear();
            foreach (UDT.AssignedSurvey AssegnedSurvey in AssignedSurveys)
            {
                if (!this.dicAssignedSurveys.ContainsKey(AssegnedSurvey.CourseID + "-" + AssegnedSurvey.TeacherID))
                    this.dicAssignedSurveys.Add(AssegnedSurvey.CourseID + "-" + AssegnedSurvey.TeacherID, AssegnedSurvey);
            }
            this.btnSet.Enabled = true;
            MsgBox.Show("儲存成功。");

            if (this.cboCourse.SelectedIndex == -1)
                return;

            ComboItem item = this.cboCourse.Items[this.cboCourse.SelectedIndex] as ComboItem;
            this.DGV_DataBinding(item);
        }

        private void ToolStripMenuItem_Description_MouseHover(object sender, EventArgs e)
        {
            if (this.dgvData.SelectedRows.Count == 0)
                return;

            this.toolStripTextBox_Description.Text = (this.dgvData.SelectedRows[0].Cells[6].Value + "").Trim();
            this.toolStripTextBox_Description.Tag = this.dgvData.SelectedRows[0];
        }

        private void ToolStripMenuItem_Save_Click(object sender, EventArgs e)
        {
            (this.toolStripTextBox_Description.Tag as DataGridViewRow).Cells[6].Value = this.toolStripTextBox_Description.Text.Trim();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DeleteAssignedSurvey_Click(object sender, EventArgs e)
        {
            if (this.dgvData.SelectedRows.Count == 0)
                return;

            this.dgvData.EndEdit();
            foreach (DataGridViewRow row in this.dgvData.SelectedRows)
            {
                if (row.IsNewRow)
                    continue;

                row.Cells[3].Value = string.Empty;
                row.Cells[4].Value = string.Empty;
                row.Cells[5].Value = string.Empty;
                row.Cells[6].Value = string.Empty;
            }
        }
    }
}
