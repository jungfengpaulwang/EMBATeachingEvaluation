using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Linq;

namespace TeachingEvaluation.PrivateControl
{
    partial class PupopDetailPane : Office2007Form
    {
        FISCA.UDT.AccessHelper Access;
        FISCA.Data.QueryHelper Query;

        public PupopDetailPane()
        {
            InitializeComponent();

            //detailPane1.PreferenceRequire = () =>
            //{
            //    return Preference.GetChild("DetailPane");
            //};
            this.Load += new EventHandler(PupopDetailPane_Load);
        }

        private void PupopDetailPane_Load(object sender, EventArgs e)
        {
            detailPane1.SetLayout(LayoutXml);
            detailPane1.btnPreview.Click += new EventHandler(btnPreview_Click);
            Access = new FISCA.UDT.AccessHelper();
            Query = new FISCA.Data.QueryHelper();
            this.lstSurvey.DisplayMember = "Name";
            this.lstSurvey.ValueMember = "UID";
            this.InitSurvey();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (this.lstSurvey.SelectedItem == null)
            {
                MessageBox.Show("請先選取問卷。");
                return;
            }
            List<UDT.Survey> Surveys = Access.Select<UDT.Survey>(new List<string> { ((dynamic)this.lstSurvey.SelectedItem).UID });
            (new Forms.frmHTML_Preview(Surveys.ElementAt(0))).ShowDialog();
        }

        public string PrimaryKey
        {
            get { return this.detailPane1.PrimaryKey; }
            set
            {
                detailPane1.PrimaryKey = value;
            }
        }
        //public void SetDescriptionPane(SurveyManagement.PrivateControl.DescriptionPane pane)
        //{
        //    //detailPane1.SetDescriptionPane(pane);
        //}

        public void SetDescription(string description)
        {
            detailPane1.SetDescription(description);
            //this.Text = description;
        }

        //public XPreference Preference
        //{
        //    get { return PreferenceRequire(); }
        //}

        //internal Func<XPreference> PreferenceRequire;

        //public IPreferenceProvider PreferenceProvider
        //{
        //    get{ return detailPane1.PreferenceProvider; }
        //    set { detailPane1.PreferenceProvider = value; }
        //}

        public void AddDetailItem(PrivateControl.DetailContent content)
        {
            detailPane1.AddDetailItem(content);
        }

        /// <summary>
        /// 由 NavContentPresentation.PopupDetailPane 指定其值。
        /// </summary>
        public System.Xml.Linq.XElement LayoutXml { get; set; }

        private void lstSurvey_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lstSurvey.SelectedItem == null)
                return;

            this.SuspendLayout();
            this.DoubleBuffered = true;
            this.detailPane1.Clear();
            this.detailPane1.PrimaryKey = ((dynamic)this.lstSurvey.SelectedItem).UID;

            this.SetDescription(((dynamic)this.lstSurvey.SelectedItem).Name); 
            
            InitQuestion();
            this.ResumeLayout(false);
        }

        private void lstSurvey_Resize(object sender, EventArgs e)
        {
            
        }

        private void PupopDetailPane_Resize(object sender, EventArgs e)
        {
            if (this.lstSurvey.SelectedItem == null)
                return;
            
            this.SetDescription(((dynamic)this.lstSurvey.SelectedItem).Name); 
        }

        private void chkAdd_Click(object sender, EventArgs e)
        {
            if ((new Forms.SurveyCreator(null)).ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.InitSurvey();
            }
        }

        private void InitSurvey()
        {
            List<UDT.Survey> Surveys = Access.Select<UDT.Survey>();
            this.lstSurvey.Items.Clear();
            foreach (UDT.Survey survey in Surveys)
            {
                this.lstSurvey.Items.Add(new { Name = survey.Name + "(" + survey.Category + ")", UID = survey.UID });
            }
        }

        private void InitQuestion()
        {
            IEnumerable<UDT.Question> Questions = Access.Select<UDT.Question>(string.Format("ref_survey_id = {0}", this.detailPane1.PrimaryKey));
            if (Questions.Count() > 0)
                Questions = Questions.OrderBy(x => x.DisplayOrder);

            foreach (UDT.Question Question in Questions)
            {
                PrivateControl.DetailContent DetailContent;
                if (Question.Type == "單選題")
                    DetailContent = new QuestionTemplate.SingleChoice();
                else if (Question.Type == "問答題")
                    DetailContent = new QuestionTemplate.Essay();
                else if (Question.Type == "複選題")
                    DetailContent = new QuestionTemplate.MultiChoice();
                else
                    throw new Exception("Type：" + Question.Type + "，尚未支援。");

                DetailContent.PrimaryKey = this.PrimaryKey;
                DetailContent.Record = Question;
                this.AddDetailItem(DetailContent);
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (this.lstSurvey.SelectedItem == null)
            {
                MessageBox.Show("請先選取問卷樣版。");
                return;
            }
            //  檢查待刪項目是否已有人做答
            IEnumerable<UDT.Reply> Replys = Access.Select<UDT.Reply>(string.Format("ref_survey_id = {0}", ((dynamic)this.lstSurvey.SelectedItem).UID));            
            List<UDT.Question> Questions = Access.Select<UDT.Question>(string.Format("ref_survey_id in ({0})", ((dynamic)this.lstSurvey.SelectedItem).UID));
            //  若有則發出警告
            if (Replys.Count() > 0)
            {
                string Error_Message = "此問卷已有學生做答，不得刪除。\n";
                //List<K12.Data.ClassRecord> Clazz = K12.Data.Class.SelectByIDs(Replys.Select(x => x.ClassID.ToString()).Distinct());
                //Error_Message += "做答班級：" + string.Join(",", Clazz.Select(x => x.Name)) + "\n";
                MessageBox.Show(Error_Message);
                return;
            }
            else
            {
                if (MessageBox.Show("確定刪除？", "警告", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                    return;
                //  刪問題選項
                List<UDT.QuestionOption> QuestionOptions = new List<UDT.QuestionOption>();
                if (Questions.Count > 0)
                    QuestionOptions = Access.Select<UDT.QuestionOption>(string.Format("ref_question_id in ({0})", string.Join(",", Questions.Select(x => x.UID))));
                QuestionOptions.ForEach(x => x.Deleted = true);
                QuestionOptions.SaveAll();
                //  刪問題
                Questions.ForEach(x => x.Deleted = true);
                Questions.SaveAll();
                //  刪問卷
                List<UDT.Survey> Surveys = Access.Select<UDT.Survey>(new List<string> { ((dynamic)this.lstSurvey.SelectedItem).UID });
                Surveys.ForEach(x => x.Deleted = true);
                Surveys.SaveAll();
            }
            this.InitSurvey();
            this.detailPane1.Clear();
        }

        private void Update_Click(object sender, EventArgs e)
        {
            if (this.lstSurvey.SelectedItem == null)
            {
                MessageBox.Show("請先選取問卷。");
                return;
            }
            List<UDT.Survey> Surveys = Access.Select<UDT.Survey>(new List<string> { ((dynamic)this.lstSurvey.SelectedItem).UID });
            if ((new Forms.SurveyCreator(Surveys.ElementAt(0))).ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.InitSurvey();
                this.SetDescription(Surveys.ElementAt(0).Name);
            }
        }
    }
}
