using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using System.Dynamic;
using FISCA.UDT;
using DevComponents.Editors;
using System.Xml.Linq;

namespace TeachingEvaluation.Forms
{
    public partial class frmStatistics_SingleForm : BaseForm
    {
        private UDT.StatisticsGroup StatisticsGroup;
        private UDT.Survey Survey;
        private ErrorProvider ErrorProvider;
        private AccessHelper Access;

        public frmStatistics_SingleForm(UDT.StatisticsGroup StatisticsGroup, UDT.Survey Survey)
        {
            InitializeComponent();
            this.StatisticsGroup = StatisticsGroup;
            this.Survey = Survey;
            Access = new AccessHelper();
            this.ErrorProvider = new ErrorProvider();
            this.Load += new EventHandler(frmStatistics_SingleForm_Load);
        }

        private void frmStatistics_SingleForm_Load(object sender, EventArgs e)
        {
            this.InitSurvey();

            if (this.StatisticsGroup == null)
            {
                this.Text = "新增統計群組";
                this.cboSurvey.SelectedIndex = 0;
                this.txtStatisticsGroup.Text = string.Empty;
            }
            else
            {
                this.Text = "修改統計群組";
                UDT.Survey Survey = new UDT.Survey();
                this.txtStatisticsGroup.Text = this.StatisticsGroup.Name;
                foreach (ComboItem item in this.cboSurvey.Items)
                {
                    if (item.Tag == null)
                        continue;
                    if ((item.Tag as UDT.Survey).UID == this.StatisticsGroup.SurveyID.ToString())
                    {
                        this.cboSurvey.SelectedItem = item;
                        Survey = item.Tag as UDT.Survey;
                    }
                }
                this.cboSurvey.Enabled = false;
                this.InitQuestion(Survey.UID);
                this.SetQuestionChecked(this.StatisticsGroup.DisplayOrderList);
            }
        }

        private void SetQuestionChecked(string DisplayOrderList)
        {
            /// <Questions>
            ///     <Question QuestionID="123" DisplayOrder="1">本課程的內容和學習目標十分明確</Question>
            ///     <Question QuestionID="456" DisplayOrder="2">本課程上課內容充實，且符合教學大綱</Question>
            /// </Questions>

            XDocument xDocument = XDocument.Parse(DisplayOrderList, LoadOptions.None);
            List<XElement> xElements = xDocument.Descendants("Question").ToList();
            if (xElements.Count() == 0)
                return;

            Dictionary<string, string> dicOptions = new Dictionary<string, string>();
            xElements.ForEach(x => dicOptions.Add(x.Attribute("QuestionID").Value, x.Value));
            foreach (ListViewItem item in this.lstQuestions.Items)
            {
                UDT.Question Question = item.Tag as UDT.Question;

                if (dicOptions.ContainsKey(Question.UID))
                    item.Checked = true;
                else
                    item.Checked = false;
            }
        }

        private void InitQuestion(string SurveyID)
        {
            List<UDT.Question> Questions = Access.Select<UDT.Question>(string.Format("ref_survey_id={0}", SurveyID));

            this.lstQuestions.Clear();
            foreach (UDT.Question Question in Questions)
            {
                if (Question.Type == "問答題")
                    continue;
                if (Question.IsSelfAssessment)
                    continue;
                ListViewItem item = new ListViewItem(Question.DisplayOrder + "、" + Question.Title);

                item.Tag = Question;

                this.lstQuestions.Items.Add(item);
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
            if (string.IsNullOrWhiteSpace(this.cboSurvey.Text))
            {
                this.ErrorProvider.SetError(this.cboSurvey, "必填。");
                validated = false;
            }
            if (string.IsNullOrWhiteSpace(this.txtStatisticsGroup.Text))
            {
                this.ErrorProvider.SetError(this.txtStatisticsGroup, "必填。");
                validated = false;
            }

            if (this.lstQuestions.CheckedItems.Count == 0)
            {
                this.ErrorProvider.SetError(this.lstQuestions, "必選。");
                validated = false;
            }
            //  檢查相同教學評鑑樣版之群組名稱是否已存在？
            ComboItem item = (ComboItem)this.cboSurvey.SelectedItem;
            string SurveyID = ((UDT.Survey)item.Tag).UID;
            List<UDT.StatisticsGroup> StatisticsGroups = Access.Select<UDT.StatisticsGroup>(string.Format("ref_survey_id ={0}", SurveyID));
            foreach (UDT.StatisticsGroup StatisticsGroup in StatisticsGroups)
            {
                if (this.StatisticsGroup == null)
                {
                    if (StatisticsGroup.Name.Trim().ToLower() == this.txtStatisticsGroup.Text.Trim().ToLower())
                    {
                        this.ErrorProvider.SetError(this.txtStatisticsGroup, "群組名稱已存在。");
                        validated = false;
                    }
                }
                else
                {
                    if ((StatisticsGroup.Name.Trim().ToLower() == this.txtStatisticsGroup.Text.Trim().ToLower()) && (StatisticsGroup.UID != this.StatisticsGroup.UID))
                    {
                        this.ErrorProvider.SetError(this.txtStatisticsGroup, "群組名稱已存在。");
                        validated = false;
                    }
                }
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
            if (this.StatisticsGroup == null)
                this.StatisticsGroup = new UDT.StatisticsGroup();

            this.StatisticsGroup.Name = this.txtStatisticsGroup.Text.Trim();
            
            ComboItem item = (ComboItem)this.cboSurvey.SelectedItem;
            string SurveyID = ((UDT.Survey)item.Tag).UID;
            this.StatisticsGroup.SurveyID = int.Parse(SurveyID);

            StringBuilder sb = new StringBuilder("<Questions>");
            /// <Questions>
            ///     <Question QuestionID="123" DisplayOrder="1">本課程的內容和學習目標十分明確</Question>
            ///     <Question QuestionID="456" DisplayOrder="2">本課程上課內容充實，且符合教學大綱</Question>
            /// </Questions>
            foreach (ListViewItem Item in this.lstQuestions.Items)
            {
                if (Item.Checked)
                {
                    UDT.Question Question = Item.Tag as UDT.Question;
                    sb.AppendFormat(@"<Question QuestionID='{0}' DisplayOrder='{1}'>{2}</Question>", Question.UID, Question.DisplayOrder, Question.Title);
                }
            }

            this.StatisticsGroup.DisplayOrderList = sb.Append("</Questions>").ToString();

            this.StatisticsGroup.Save();
            this.Close();
        }

        private void cboSurvey_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lstQuestions.Clear();

            if (this.cboSurvey.SelectedIndex == -1)
                return;

            UDT.Survey Survey = (this.cboSurvey.Items[this.cboSurvey.SelectedIndex] as ComboItem).Tag as UDT.Survey;
            if (Survey == null)
                return;
            this.InitQuestion(Survey.UID);
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            this.lstQuestions.Items.Cast<ListViewItem>().ToList().ForEach(x => x.Checked = this.chkSelectAll.Checked);
        }
    }
}