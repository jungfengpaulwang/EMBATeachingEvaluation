using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace TeachingEvaluation.PrivateControl
{
    partial class PupopDetailPane : Office2007Form
    {
        FISCA.UDT.AccessHelper Access;
        FISCA.Data.QueryHelper Query;

        public PupopDetailPane()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            this.Load += new EventHandler(PupopDetailPane_Load);
        }

        private void PupopDetailPane_Load(object sender, EventArgs e)
        {
            this.detailPane1.panProgress.Height = 0;
            detailPane1.btnPreview.Click += new EventHandler(btnPreview_Click);
            Access = new FISCA.UDT.AccessHelper();
            Query = new FISCA.Data.QueryHelper();
            this.lstSurvey.DisplayMember = "Name";
            this.lstSurvey.ValueMember = "UID";
            this.Init();
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

        public void SetDescription(string description)
        {
            detailPane1.SetDescription(description);
        }

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

            this.lstSurvey.Enabled = false;
            this.detailPane1.DetailItemContainers.Visible = false;
            this.detailPane1.Clear();
            this.detailPane1.panProgress.Height = 200;
            this.detailPane1.PrimaryKey = ((dynamic)this.lstSurvey.SelectedItem).UID;
            this.SetDescription(((dynamic)this.lstSurvey.SelectedItem).Name);
            this.detailPane1.DetailItemContainers.SuspendLayout();

            QuestionTemplate.TemplatePool.CleanTemplates();
            InitQuestion(); 
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
                this.Init();
            }
        }

        private void Init()
        {
            this.lstSurvey.Items.Clear();
            List<UDT.Survey> Surveys = new List<UDT.Survey>();
            Task task = Task.Factory.StartNew(() =>
            {
                Surveys = Access.Select<UDT.Survey>();
            });
            task.ContinueWith((x)=>
            {
                foreach (UDT.Survey survey in Surveys)
                {
                    int idx = this.lstSurvey.Items.Add(new { Name = survey.Name + (string.IsNullOrWhiteSpace(survey.Category) ? "" : "(" + survey.Category + ")"), UID = survey.UID });                  
                }
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext()); 
        }

        private void InitQuestion()
        {
            Dictionary<int, List<UDT.QuestionOption>> dicQuestionOptions = new Dictionary<int, List<UDT.QuestionOption>>();
            Dictionary<UDT.Hierarchy, List<UDT.Question>> dicHierarchies = new Dictionary<UDT.Hierarchy, List<UDT.Question>>();
            Dictionary<UDT.Hierarchy, List<UDT.Question>> dicHierarchies_New = new Dictionary<UDT.Hierarchy, List<UDT.Question>>();

            Task task = Task.Factory.StartNew(() =>
            {
                List<int> exits_question_ids = new List<int>();
                List<UDT.Question> Questions = Access.Select<UDT.Question>(string.Format("ref_survey_id = {0}", this.detailPane1.PrimaryKey));
                List<UDT.QuestionOption> QuestionOptions = new List<UDT.QuestionOption>();
                Dictionary<string, UDT.Question> dicQuestions = new Dictionary<string, UDT.Question>();
                if (Questions.Count > 0)
                {
                    QuestionOptions = Access.Select<UDT.QuestionOption>(string.Format("ref_question_id in ({0})", string.Join(",", Questions.Select(x => x.UID))));
                    Questions = Questions.OrderBy(x => x.DisplayOrder).ToList();
                    dicQuestions = Questions.ToDictionary(x => x.UID);
                }
                if (QuestionOptions.Count > 0)
                    QuestionOptions = QuestionOptions.OrderBy(x => x.QuestionID).ThenBy(x => x.DisplayOrder).ToList();

                List<UDT.QHRelation> QHRelations = Access.Select<UDT.QHRelation>();
                List<UDT.Hierarchy> Hierarchies = Access.Select<UDT.Hierarchy>();
                UDT.Hierarchy uHierarchy = new UDT.Hierarchy();
                dicHierarchies.Add(uHierarchy, new List<UDT.Question>());
                if (Hierarchies.Count > 0)
                {
                    Hierarchies = Hierarchies.OrderBy(x => x.DisplayOrder).ToList();
                    Hierarchies.ForEach((x) =>
                    {
                        if (!dicHierarchies.ContainsKey(x))
                            dicHierarchies.Add(x, new List<UDT.Question>());
                    });
                }
                if (QHRelations.Count > 0)
                {
                    QHRelations.ForEach((x) =>
                    {
                        foreach (UDT.Hierarchy Hierarchy in dicHierarchies.Keys)
                        {
                            if (x.HierarchyTitle == Hierarchy.Title && dicQuestions.ContainsKey(x.QuestionID.ToString()))
                            {
                                dicHierarchies[Hierarchy].Add(dicQuestions[x.QuestionID.ToString()]);
                                exits_question_ids.Add(x.QuestionID);
                            }
                        }
                    });
                }
                foreach (UDT.QuestionOption QuestionOption in QuestionOptions)
                {
                    if (!dicQuestionOptions.ContainsKey(QuestionOption.QuestionID))
                        dicQuestionOptions.Add(QuestionOption.QuestionID, new List<UDT.QuestionOption>());

                    dicQuestionOptions[QuestionOption.QuestionID].Add(QuestionOption);
                }
                foreach (UDT.Question Q in Questions)
                {
                    if (!exits_question_ids.Contains(int.Parse(Q.UID)))
                    {
                        exits_question_ids.Add(int.Parse(Q.UID));
                        dicHierarchies[uHierarchy].Add(Q);
                    }
                }
                foreach (UDT.Hierarchy Hierarchy in dicHierarchies.Keys)
                {
                    dicHierarchies_New.Add(Hierarchy, dicHierarchies[Hierarchy].OrderBy(y => y.DisplayOrder).ToList());
                }
            });
            task.ContinueWith((x) =>
            {
                foreach (UDT.Hierarchy Hierarchy in dicHierarchies_New.Keys)
                {
                    foreach (UDT.Question Question in dicHierarchies_New[Hierarchy])
                    {
                        PrivateControl.DetailContent DetailContent = new DetailContent();
                        if (Question.Type == "單選題")
                            DetailContent = QuestionTemplate.TemplatePool.GetTemplate<QuestionTemplate.SingleChoice>();
                        else if (Question.Type == "問答題")
                            DetailContent = QuestionTemplate.TemplatePool.GetTemplate<QuestionTemplate.Essay>();
                        else if (Question.Type == "複選題")
                            DetailContent = QuestionTemplate.TemplatePool.GetTemplate<QuestionTemplate.MultiChoice>();
                        else
                            throw new Exception("Type：" + Question.Type + "，尚未支援。");

                        DetailContent.Record = Question;
                        DetailContent.PrimaryKey = this.PrimaryKey;
                        DetailContent.SurveyID = int.Parse(this.PrimaryKey);
                        DetailContent.HierarchyTitle = Hierarchy.Title;

                        DetailContent.HierarchyTitleSource = ((new List<string>() { "" }).Union(dicHierarchies_New.Keys.Select(y => y.Title + ""))).Distinct().ToList();

                        if (dicQuestionOptions.ContainsKey(int.Parse(Question.UID)))
                            DetailContent.RecordDetails = dicQuestionOptions[int.Parse(Question.UID)];
                        else
                            DetailContent.RecordDetails = new List<UDT.QuestionOption>();

                        this.AddDetailItem(DetailContent);
                        DetailContent.DataBind();
                    }
                }
                this.detailPane1.PrimaryKey = this.PrimaryKey;
                this.detailPane1.panProgress.Height = 0;
                this.detailPane1.DetailItemContainers.ResumeLayout(false);
                this.detailPane1.DetailItemContainers.Visible = true;
                this.lstSurvey.Enabled = true;
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext()); 
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
            this.Init();
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
                this.Init();
                this.SetDescription(Surveys.ElementAt(0).Name);
            }
        }
    }
}
