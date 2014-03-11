using System;
using System.Xml;
using FISCA.UDT;
using System.Collections.Generic;
using FISCA.UDT;
using DevComponents.Editors;
using System.Linq;
using System.Windows.Forms;

namespace TeachingEvaluation.Forms
{
    public partial class SurveyCreator : FISCA.Presentation.Controls.BaseForm
    {
        private UDT.Survey _Survey;
        private AccessHelper Access;

        public SurveyCreator(UDT.Survey Survey)
        {
            InitializeComponent();

            this.Load += new System.EventHandler(this.Form_Load);

            this._Survey = Survey;

            Access = new AccessHelper();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            this.Text = (_Survey == null ? "新增" : "修改") + "評鑑樣版";
            this.txtName.Text = (this._Survey == null ? string.Empty : this._Survey.Name);

            if (this._Survey != null)
            {
                this.cboSurvey.Enabled = false;
                this.txtCategory.Text = this._Survey.Category;
                this.txtDescription.Text = this._Survey.Description;
                return;
            }

            List<UDT.Survey> Surveys = Access.Select<UDT.Survey>();

            ComboItem comboItem1 = new ComboItem("不進行複製");
            comboItem1.Tag = null;

            this.cboSurvey.Items.Add(comboItem1);
            foreach (UDT.Survey var in Surveys)
            {
                ComboItem item = new ComboItem(var.Name);
                item.Tag = var;
                cboSurvey.Items.Add(item);
            }

            cboSurvey.SelectedItem = comboItem1;
            txtName.Focus();
        }
        
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            try
            {
                string Err_Msg = string.Empty;

                string SurveyName = this.txtName.Text.Trim();

                if (string.IsNullOrEmpty(SurveyName))
                    Err_Msg += "請輸入評鑑樣版名稱。\n";
                
                if (!string.IsNullOrEmpty(Err_Msg))
                    throw new Exception(Err_Msg);

                //  所有的「評鑑樣版」
                List<UDT.Survey> Surveys = Access.Select<UDT.Survey>(string.Format(@"name='{0}'", SurveyName));
                //  若新增的「評鑑樣版」名稱已存在，則發出警告並跳出程式
                if ((this._Survey == null) && Surveys.Count() > 0)
                {
                    MessageBox.Show("同名之評鑑樣版已存在。");
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }
                //  若修改的「評鑑樣版」名稱已存在，且該「評鑑樣版」的「uid」非待修改「評鑑樣版」所擁有，則發出警告並跳出程式
                if ((this._Survey != null) && Surveys.Where(x => x.UID != this._Survey.UID).Count() > 0)
                {
                    MessageBox.Show("同名之評鑑樣版已存在。");
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }

                //  修改「問卷」
                if (this._Survey != null)
                {
                    this._Survey.Name = this.txtName.Text.Trim();
                    this._Survey.Category = this.txtCategory.Text.Trim();
                    this._Survey.Description = this.txtDescription.Text.Trim();
                    this._Survey.Save();
                    this.Close();
                    return;
                }

                //  待新增的「問卷」
                UDT.Survey nSurvey = new UDT.Survey();

                nSurvey.Name = SurveyName;
                nSurvey.Category = this.txtCategory.Text.Trim();
                nSurvey.Description = this.txtDescription.Text.Trim();

                nSurvey.Save();

                //  檢查是否複製「題目」
                ComboItem item = (ComboItem)this.cboSurvey.SelectedItem;
                if (item == null || item.Tag == null)
                {
                    //  不複製
                    this.Close();
                    return;
                }
                else
                {
                    //  複製「題目」、「答案選項」、及「題目標題」
                    string SurveyID = ((UDT.Survey)item.Tag).UID;
                    List<UDT.Question> oQuestions = Access.Select<UDT.Question>(string.Format(@"ref_survey_id = {0}", SurveyID));
                    List<UDT.Question> nQuestions = new List<UDT.Question>();
                    foreach (UDT.Question oQuestion in oQuestions)
                    {
                        UDT.Question nQuestion = new UDT.Question();

                        nQuestion.SurveyID = int.Parse(nSurvey.UID);
                        nQuestion.Title = oQuestion.Title;
                        nQuestion.Type = oQuestion.Type;
                        nQuestion.IsRequired = oQuestion.IsRequired;
                        nQuestion.IsCase = oQuestion.IsCase;
                        nQuestion.IsSelfAssessment = oQuestion.IsSelfAssessment;
                        nQuestion.DisplayOrder = oQuestion.DisplayOrder;

                        nQuestions.Add(nQuestion);
                    }
                    nQuestions.SaveAll();
                    nQuestions = Access.Select<UDT.Question>(string.Format(@"ref_survey_id = {0}", nSurvey.UID));
                    List<UDT.QuestionOption> oQuestionOptions = new List<UDT.QuestionOption>();
                    List<UDT.QuestionOption> nQuestionOptions = new List<UDT.QuestionOption>();
                    if (oQuestions.Count > 0)
                        oQuestionOptions = Access.Select<UDT.QuestionOption>(string.Format(@"ref_question_id in ({0})", string.Join(",", oQuestions.Select(x => x.UID))));
                    foreach (UDT.QuestionOption oQuestionOption in oQuestionOptions)
                    {
                        UDT.Question oQuestion = oQuestions.Where(x => x.UID == oQuestionOption.QuestionID.ToString()).ElementAt(0);
                        UDT.Question nQuestion = nQuestions.Where(x => x.Title == oQuestion.Title).ElementAt(0);
                        
                        UDT.QuestionOption nQuestionOption = new UDT.QuestionOption();
                        nQuestionOption.QuestionID = int.Parse(nQuestion.UID);
                        nQuestionOption.Title = oQuestionOption.Title;
                        nQuestionOption.DisplayOrder = oQuestionOption.DisplayOrder;

                        nQuestionOptions.Add(nQuestionOption);
                    }
                    nQuestionOptions.SaveAll();
                    List<UDT.QHRelation> oQHRelations = new List<UDT.QHRelation>();
                    List<UDT.QHRelation> nQHRelations = new List<UDT.QHRelation>();
                    Dictionary<string, string> dicQHRelations = new Dictionary<string, string>();
                    if (oQuestions.Count > 0)
                        oQHRelations = Access.Select<UDT.QHRelation>(string.Format(@"ref_question_id in ({0})", string.Join(",", oQuestions.Select(x => x.UID))));
                    if (oQHRelations.Count > 0)
                    {
                        foreach(UDT.QHRelation QHRelation in oQHRelations)
                        {
                            if (!dicQHRelations.ContainsKey(QHRelation.QuestionID.ToString()))
                                dicQHRelations.Add(QHRelation.QuestionID.ToString(), QHRelation.HierarchyTitle);
                        }
                    }
                    foreach (UDT.QHRelation oQHRelation in oQHRelations)
                    {
                        UDT.Question oQuestion = oQuestions.Where(x => x.UID == oQHRelation.QuestionID.ToString()).ElementAt(0);
                        UDT.Question nQuestion = nQuestions.Where(x => x.Title == oQuestion.Title).ElementAt(0);

                        if (dicQHRelations.ContainsKey(oQuestion.UID))
                        {
                            UDT.QHRelation nQHRelation = new UDT.QHRelation();

                            nQHRelation.QuestionID = int.Parse(nQuestion.UID);
                            nQHRelation.HierarchyTitle = dicQHRelations[oQuestion.UID];

                            nQHRelations.Add(nQHRelation);
                        }
                    }
                    nQHRelations.SaveAll();
                    this.Close();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
            }
            finally
            {
                //this.Close();
            }
        }
    }
}