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
using K12.Data;
using DevComponents.Editors;

namespace TeachingEvaluation.Forms
{
    public partial class frmAssignedSurvy_SingleForm : BaseForm
    {
        private UDT.AssignedSurvey AssignedSurvey;
        private ErrorProvider ErrorProvider;
        private AccessHelper Access;

        private CourseRecord Course;
        private TeacherRecord Teacher;
        private string CaseName;

        public frmAssignedSurvy_SingleForm(UDT.AssignedSurvey AssignedSurvey, CourseRecord Course, TeacherRecord Teacher, string CaseName)
        {
            InitializeComponent();
            this.AssignedSurvey = AssignedSurvey;
            this.Course = Course;
            this.Teacher = Teacher;
            this.CaseName = CaseName;
            Access = new AccessHelper();
            this.ErrorProvider = new ErrorProvider();
            this.Load += new EventHandler(frmAssignedSurvy_SingleForm_Load);
        }

        private void frmAssignedSurvy_SingleForm_Load(object sender, EventArgs e)
        {
            this.InitSurvey();

            if (this.AssignedSurvey == null)
            {
                this.txtCourseName.Text = this.Course.Name;
                this.txtCourseName.Tag = this.Course;
                this.txtTeacherName.Text = this.Teacher.Name;
                this.txtTeacherName.Tag = this.Teacher;
                this.txtCaseName.Text = this.CaseName;
                this.txtBeginTime.Text = string.Empty;
                this.txtEndTime.Text = string.Empty;
                this.cboSurvey.SelectedIndex = 0;
                this.txtTeachingEvaluationDescription.Text = string.Empty;
                this.Text = "新增問卷組態";
            }
            else
            {
                UDT.Survey Survey = new UDT.Survey();
                this.txtCourseName.Text = this.Course.Name;
                this.txtCourseName.Tag = this.Course;
                this.txtTeacherName.Text = this.Teacher.Name;
                this.txtTeacherName.Tag = this.Teacher;
                this.txtCaseName.Text = this.CaseName;
                this.txtBeginTime.Text = this.AssignedSurvey.OpeningTime.ToString("yyyy/MM/dd HH:mm");
                this.txtEndTime.Text = this.AssignedSurvey.EndTime.ToString("yyyy/MM/dd HH:mm");
                foreach (ComboItem item in this.cboSurvey.Items)
                {
                    if (item.Tag == null)
                        continue;
                    if ((item.Tag as UDT.Survey).UID == this.AssignedSurvey.SurveyID.ToString())
                    {
                        this.cboSurvey.SelectedItem = item;
                        Survey = item.Tag as UDT.Survey;
                    }
                }
                this.txtTeachingEvaluationDescription.Text = (string.IsNullOrEmpty(this.AssignedSurvey.Description) ? Survey.Description : this.AssignedSurvey.Description);
                this.Text = "修改問卷組態";
            }
        }

        private void InitSurvey()
        {
            List<UDT.Survey> Surveys = Access.Select<UDT.Survey>();

            ComboItem comboItem1 = new ComboItem("");
            comboItem1.Tag = null;

            this.cboSurvey.Items.Add(comboItem1);
            foreach (UDT.Survey var in Surveys)
            {
                ComboItem item = new ComboItem(var.Name);
                item.Tag = var;
                cboSurvey.Items.Add(item);
            }

            cboSurvey.SelectedItem = comboItem1;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private bool Validated()
        {
            this.ErrorProvider.Clear();
            bool validated = true;

            this.ErrorProvider.Clear();
            if (string.IsNullOrWhiteSpace(this.txtBeginTime.Text))
            {
                this.ErrorProvider.SetError(this.txtBeginTime, "必填。");
                validated = false;
            }
            if (string.IsNullOrWhiteSpace(this.txtEndTime.Text))
            {
                this.ErrorProvider.SetError(this.txtEndTime, "必填。");
                validated = false;
            }
            if (string.IsNullOrWhiteSpace(this.cboSurvey.Text))
            {
                this.ErrorProvider.SetError(this.cboSurvey, "必填。");
                validated = false;
            }

            DateTime begin_date_time;
            DateTime end_date_time;

            if (!DateTime.TryParse(this.txtBeginTime.Text.Trim(), out begin_date_time))
            {
                this.ErrorProvider.SetError(this.txtBeginTime, "「問卷填寫開始時間」格式錯誤。範例：2013/6/22 00:01。");
                validated = false;
            }

            if (!DateTime.TryParse(this.txtEndTime.Text.Trim(), out end_date_time))
            {
                this.ErrorProvider.SetError(this.txtEndTime, "「問卷填寫結束時間」格式錯誤。範例：2013/7/31 23:59。");
                validated = false;
            }

            return validated;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            if (!this.Validated())
            {
                MessageBox.Show("請修正錯誤再儲存。");
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }
            if (this.AssignedSurvey == null)
                this.AssignedSurvey = new UDT.AssignedSurvey();

            List<UDT.AssignedSurvey> AssignedSurveys = Access.Select<UDT.AssignedSurvey>();

            this.AssignedSurvey.CourseID = int.Parse((this.txtCourseName.Tag as CourseRecord).ID);
            this.AssignedSurvey.TeacherID = int.Parse((this.txtTeacherName.Tag as TeacherRecord).ID);
            
            ComboItem item = (ComboItem)this.cboSurvey.SelectedItem;
            string SurveyID = ((UDT.Survey)item.Tag).UID;
            this.AssignedSurvey.SurveyID = int.Parse(SurveyID);

            this.AssignedSurvey.OpeningTime = DateTime.Parse(this.txtBeginTime.Text.Trim());
            this.AssignedSurvey.EndTime = DateTime.Parse(this.txtEndTime.Text.Trim());
            this.AssignedSurvey.Description = this.txtTeachingEvaluationDescription.Text.Trim();

            this.AssignedSurvey.Save();
            this.Close();
        }
    }
}
