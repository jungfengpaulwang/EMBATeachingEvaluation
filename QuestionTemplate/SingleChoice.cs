using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using CateringService_Survey.PrivateControl;
using Campus.Windows;
using System.Xml.Linq;
using System.Threading.Tasks;
using DevComponents.Editors;

namespace TeachingEvaluation.QuestionTemplate
{
    public partial class SingleChoice : TeachingEvaluation.PrivateControl.DetailContent
    {
        private FISCA.UDT.AccessHelper Access;

        public SingleChoice()
        {
            InitializeComponent(); 

            DataListener = new Campus.Windows.ChangeListen();
            DataListener.Add(new TextBoxSource(this.txtTitle));
            DataListener.Add(new DataGridViewSource(this.dgvData));
            DataListener.Add(new NumericUpDownSource(this.nudDisplayOrder));
            DataListener.Add(new CheckBoxXSource(this.chkRequired));
            DataListener.Add(new CheckBoxXSource(this.chkCase));
            DataListener.Add(new CheckBoxXSource(this.chkSelfAssessment));
            DataListener.Add(new CheckBoxXSource(this.chkNoneCalculated));
            DataListener.Add(new ComboBoxSource(this.cboHierarchy, ComboBoxSource.ListenAttribute.Text));
            DataListener.StatusChanged += new EventHandler<ChangeEventArgs>(Listener_StatusChanged);
            DataListener.ResumeListen();

            this.dgvData.DataError += new DataGridViewDataErrorEventHandler(dgvData_DataError);
            this.dgvData.CurrentCellDirtyStateChanged += new EventHandler(dgvData_CurrentCellDirtyStateChanged);
            this.dgvData.CellEnter += new DataGridViewCellEventHandler(dgvData_CellEnter);
            this.dgvData.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvData_ColumnHeaderMouseClick);
            this.dgvData.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvData_RowHeaderMouseClick);
            this.dgvData.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvData_MouseClick);

            this.RecordDetails = new List<UDT.QuestionOption>();
            Access = new FISCA.UDT.AccessHelper();

            this.Group = Guid.NewGuid().ToString();
            this.Type = "單選題";
            this.Caption = this.Type;

            this.Load += new System.EventHandler(this.SingleChoice_Load);
        }

        private void SingleChoice_Load(object sender, EventArgs e)        {        }

        public override void DataBind()
        {
            this.DataListener.SuspendListen();
            //  題目層次標題
            this.cboHierarchy.Items.Clear();
            this.cboHierarchy.Items.AddRange(this.HierarchyTitleSource.ToArray());
            for (int i = 0; i < this.cboHierarchy.Items.Count; i++)
            {
                if ((this.cboHierarchy.Items[i] + "") == this.HierarchyTitle)
                    this.cboHierarchy.Text = (this.cboHierarchy.Items[i] + "");
            }
            //  題目
            this.txtTitle.Text = this.Record.Title;
            this.chkRequired.Checked = this.Record.IsRequired;
            this.chkCase.Checked = this.Record.IsCase;
            this.chkSelfAssessment.Checked = this.Record.IsSelfAssessment;
            this.chkNoneCalculated.Checked = this.Record.IsNoneCalculated;
            if (this.Record.RecordStatus == FISCA.UDT.RecordStatus.Insert)
                this.nudDisplayOrder.Value = this.DisplayOrder.Value;
            else
                this.nudDisplayOrder.Value = decimal.Parse(this.Record.DisplayOrder.ToString());

            //  題目答案選項
            this.dgvData.EndEdit();
            this.dgvData.Rows.Clear();
            foreach (UDT.QuestionOption QuestionOption in this.RecordDetails)
            {
                List<object> source = new List<object>();

                source.Add(QuestionOption.Title);
                source.Add(QuestionOption.DisplayOrder);

                int index = this.dgvData.Rows.Add(source.ToArray());
                this.dgvData.Rows[index].Tag = QuestionOption;
            }
            this.dgvData.CurrentCell = null;
            
            this.Title = "題號" + this.nudDisplayOrder.Value + "：" + this.txtTitle.Text.Trim();
            //  傳送自己給右上角「項目」，設定選單及其文字。
            Event.DeliverDetailContent.RaisePassDetailContentEvent(this, new Event.DeliverDetailContentEventArgs(this));
            this.ResetOverrideButton();   
        }

        private void dgvData_MouseClick(object sender, MouseEventArgs e)
        {
            MouseEventArgs args = (MouseEventArgs)e;
            DataGridView dgv = (DataGridView)sender;
            DataGridView.HitTestInfo hit = dgv.HitTest(args.X, args.Y);

            if (hit.Type == DataGridViewHitTestType.TopLeftHeader)
            {
                dgvData.EndEdit();
                dgvData.CurrentCell = null;
                dgvData.SelectAll();
            }
        }

        private void dgvData_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void dgvData_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvData.EndEdit();
            dgvData.CurrentCell = null;
            dgvData.Rows[e.RowIndex].Selected = true;
        }

        private void dgvData_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvData.EndEdit();
            dgvData.CurrentCell = null;
            dgvData.Columns[e.ColumnIndex].Selected = true;
        }

        private void dgvData_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //dgvData.BeginEdit(true);
        }

        private void dgvData_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvData.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void Listener_StatusChanged(object sender, ChangeEventArgs e)
        {
            SaveButtonVisible = e.Status == ValueStatus.Dirty;
            CancelButtonVisible = SaveButtonVisible;
        }

        private void ResetOverrideButton()
        {
            SaveButtonVisible = false;
            CancelButtonVisible = false;

            DataListener.Reset();
            DataListener.ResumeListen();
        }

        private bool Is_Validate()
        {
            bool valid = true;
            this.errorProvider1.Clear();
            if (string.IsNullOrWhiteSpace(this.txtTitle.Text))
            {
                valid = false;
                this.errorProvider1.SetError(this.txtTitle, "必填。");
            }
            else
                this.errorProvider1.SetError(this.txtTitle, "");

            if (this.dgvData.Rows.Count == 0 || (this.dgvData.Rows.Count == 1 && this.dgvData.Rows[0].IsNewRow))
            {
                valid = false;
                this.errorProvider1.SetError(this.dgvData, "必填。");
            }
            else
                this.errorProvider1.SetError(this.dgvData, "");

            if (this.dgvData.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in this.dgvData.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    row.Cells[0].ErrorText = "";
                    row.Cells[1].ErrorText = "";
                    if (row.Cells[0].Value != null)
                    {
                        if (this.dgvData.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[0].Value + "" == row.Cells[0].Value + "").Count() > 1)
                        {
                            valid = false;
                            row.Cells[0].ErrorText = "重複。";
                        }
                    }
                    if (row.Cells[1].Value != null)
                    {
                        if (this.dgvData.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[1].Value + "" == row.Cells[1].Value + "").Count() > 1)
                        {
                            valid = false;
                            row.Cells[1].ErrorText = "重複。";
                        }
                    }

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.ColumnIndex == 0)
                        {
                            if (cell.Value == null || string.IsNullOrWhiteSpace(cell.Value + ""))
                            {
                                valid = false;
                                cell.ErrorText = "必填。";
                            }
                        }
                        if (cell.ColumnIndex == 1)
                        {
                            if (cell.Value == null || string.IsNullOrWhiteSpace(cell.Value + ""))
                            {
                                valid = false;
                                cell.ErrorText = "必填。";
                            }
                            else
                            {
                                int display_order = 0;
                                if (!int.TryParse(cell.Value.ToString(), out display_order))
                                {
                                    valid = false;
                                    cell.ErrorText = "限填整數。";
                                }
                            }
                        }
                    }
                }
            }
            return valid;
        }

        protected override void OnSaveButtonClick(EventArgs e)
        {
            dgvData.EndEdit();
            if (!this.Is_Validate())
            {
                MessageBox.Show("請先修正錯誤再儲存。");
                return;
            }

            this.Loading = true;
            string strHierarchyTitle = this.cboHierarchy.Text;
            int intDisplayOrder = int.Parse(this.nudDisplayOrder.Value.ToString());
            bool bRequired = this.chkRequired.Checked;
            bool bSelfAssessment = this.chkSelfAssessment.Checked;
            bool bCase = this.chkCase.Checked;
            bool bNoneCalculated = this.chkNoneCalculated.Checked;
            string strTitle = this.txtTitle.Text.Trim();
            List<UDT.QuestionOption> Qos = new List<UDT.QuestionOption>();
            foreach (DataGridViewRow row in this.dgvData.Rows)
            {
                if (row.IsNewRow)
                    continue;

                UDT.QuestionOption Qo = new UDT.QuestionOption();

                Qo.Title = (row.Cells[0].Value + "").Trim();
                Qo.DisplayOrder = int.Parse((row.Cells[1].Value + "").Trim());

                Qos.Add(Qo);
            }

            Task task = Task.Factory.StartNew(() =>
            {
                //  檢查題號與題目是否已存在？
                List<UDT.Question> oQuestions = Access.Select<UDT.Question>(string.Format("ref_survey_id ={0}", this.SurveyID));
                Dictionary<string, UDT.Question> dicQuestionIDs = new Dictionary<string, UDT.Question>();
                Dictionary<string, string> dicQuestionTitles = new Dictionary<string, string>();

                Dictionary<string, List<UDT.Question>> dicQHRelations = new Dictionary<string, List<UDT.Question>>();
                List<UDT.QHRelation> QHRelations = Access.Select<UDT.QHRelation>();
                List<string> aQs = new List<string>();

                oQuestions.ForEach((x) =>
                {
                    if (!dicQuestionIDs.ContainsKey(x.UID))
                        dicQuestionIDs.Add(x.UID, x);

                    if (!dicQuestionTitles.ContainsKey(x.UID))
                        dicQuestionTitles.Add(x.UID, x.Title.Trim().ToUpper());
                });

                dicQHRelations.Add(string.Empty, new List<UDT.Question>());
                foreach(UDT.QHRelation x in QHRelations)
                {
                    if (!dicQuestionIDs.ContainsKey(x.QuestionID.ToString()))
                        continue;

                    if (dicQuestionIDs[x.QuestionID.ToString()].SurveyID != this.SurveyID)
                        continue;

                    if (!dicQHRelations.ContainsKey(x.HierarchyTitle))
                        dicQHRelations.Add(x.HierarchyTitle, new List<UDT.Question>());

                    dicQHRelations[x.HierarchyTitle].Add(dicQuestionIDs[x.QuestionID.ToString()]);
                    aQs.Add(x.QuestionID.ToString());
                }

                oQuestions.ForEach((x) =>
                {
                    if (!aQs.Contains(x.UID))
                        dicQHRelations[string.Empty].Add(x);
                });
                List<UDT.Question> oQs = new List<UDT.Question>();
                if (dicQHRelations.ContainsKey(strHierarchyTitle))
                    oQs = dicQHRelations[strHierarchyTitle];
                if (this.Record.RecordStatus == FISCA.UDT.RecordStatus.Insert)
                {
                    if (oQs.Where(x => x.DisplayOrder == intDisplayOrder).Count() > 0)
                    {
                        throw new Exception("相同題號已存在。");
                    }
                }
                else
                {
                    if (oQs.Where(x => (x.DisplayOrder == intDisplayOrder && x.UID != this.Record.UID)).Count() > 0)
                    {
                        throw new Exception("相同題號已存在。");
                    }
                }
                if (this.Record.RecordStatus == FISCA.UDT.RecordStatus.Insert)
                {
                    if (oQuestions.Where(x=>x.Title.ToUpper() == strTitle.ToUpper()).Count() > 0)
                        throw new Exception("相同題目已存在。");
                }
                else
                {
                    if (oQuestions.Where(x=>(x.Title.ToUpper() == strTitle.ToUpper() && x.UID != this.Record.UID)).Count() > 0)
                        throw new Exception("相同題目已存在。");
                }

                this.Record.Title = strTitle;
                this.Record.Type = this.Type;
                this.Record.IsRequired = bRequired;
                this.Record.IsCase = bCase;
                this.Record.IsSelfAssessment = bSelfAssessment;
                this.Record.IsNoneCalculated = bNoneCalculated;
                this.Record.SurveyID = this.SurveyID;
                this.Record.DisplayOrder = intDisplayOrder;
                this.HierarchyTitle = strHierarchyTitle;

                this.Record.Save();

                List<UDT.QHRelation> nQHRelations = Access.Select<UDT.QHRelation>("ref_question_id=" + this.Record.UID);
                nQHRelations.ForEach(x => x.Deleted = true);
                nQHRelations.SaveAll();
                nQHRelations.Clear();
                if (!string.IsNullOrEmpty(strHierarchyTitle))
                {
                    UDT.QHRelation QHRelation = new UDT.QHRelation();

                    QHRelation.QuestionID = int.Parse(this.Record.UID);
                    QHRelation.HierarchyTitle = strHierarchyTitle;

                    nQHRelations.Add(QHRelation);
                }
                nQHRelations.SaveAll();
                List<UDT.QuestionOption> oQos = Access.Select<UDT.QuestionOption>("ref_question_id=" + this.Record.UID);
                oQos.ForEach(x => x.Deleted = true);
                oQos.SaveAll();
                foreach (UDT.QuestionOption QuestionOption in Qos)
                {
                    QuestionOption.QuestionID = int.Parse(this.Record.UID);
                }
                Qos.SaveAll();

                this.RecordDetails = Access.Select<UDT.QuestionOption>("ref_question_id=" + this.Record.UID);
            });
            task.ContinueWith((x) =>
            {
                if (x.Exception != null)
                {
                    MessageBox.Show(x.Exception.InnerException.Message);
                    this.Loading = false;
                    return;
                }

                this.Loading = false;
                this.DataBind();
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext()); 
        }

        protected override void OnCancelButtonClick(EventArgs e)
        {
            this.dgvData.EndEdit();
            this.DataBind();
        }

        protected override void OnCopyButtonClick(EventArgs e)
        {
            PrivateControl.DetailContent detail_content = QuestionTemplate.TemplatePool.GetTemplate<SingleChoice>();
     
            //  複製題目
            detail_content.Record = new UDT.Question();
            //detail_content.Group = Guid.NewGuid().ToString();
            detail_content.PrimaryKey = this.SurveyID.ToString();
            detail_content.Type = this.Type;
            detail_content.DisplayOrder = null;
            detail_content.HierarchyTitle = this.cboHierarchy.Text;
            detail_content.HierarchyTitleSource = this.HierarchyTitleSource;
            detail_content.Record.SurveyID = this.SurveyID;
            detail_content.SurveyID = this.SurveyID;

            List<UDT.QuestionOption> Qos = new List<UDT.QuestionOption>();
            foreach (DataGridViewRow row in this.dgvData.Rows)
            {
                if (row.IsNewRow)
                    continue;

                UDT.QuestionOption QuestionOption = row.Tag as UDT.QuestionOption;

                if (QuestionOption == null)
                    QuestionOption = new UDT.QuestionOption();

                int display_order = 0;
                int.TryParse((row.Cells[1].Value + "").Trim(), out display_order);
                QuestionOption.DisplayOrder = display_order;
                QuestionOption.Title = (row.Cells[0].Value + "").Trim();
                    
                Qos.Add(QuestionOption);
            }
            detail_content.RecordDetails = Qos;

            detail_content.Record.IsRequired = this.chkRequired.Checked;
            detail_content.Record.IsCase = this.chkCase.Checked;
            detail_content.Record.IsSelfAssessment = this.chkSelfAssessment.Checked;
            detail_content.Record.IsNoneCalculated = this.chkNoneCalculated.Checked;
            detail_content.Record.Title = string.Empty;

            Event.DeliverDetailContent.RaiseCopyDetailContentEvent(this, new Event.DeliverDetailContentEventArgs(detail_content));

            detail_content.DataBind();
        }

        protected override void OnDeleteButtonClick(EventArgs e)
        {
            this.Loading = true;
            Task task = Task.Factory.StartNew(() =>
            {
                if (this.Record.RecordStatus != FISCA.UDT.RecordStatus.Insert)
                {
                    //  檢查待刪項目是否已有人做答
                    IEnumerable<UDT.Reply> Replys = Access.Select<UDT.Reply>(string.Format("ref_survey_id = {0}", this.SurveyID));
                    //  若有則發出警告
                    if (Replys.Count() > 0)
                    {
                        List<string> QuestionIDs = new List<string>();
                        foreach (UDT.Reply Reply in Replys)
                        {
                            ///  問卷做答
                            ///  <Answers>
                            ///     <Question QuestionID="456">
                            ///         <Answer CaseID=””>很滿意</Answer>
                            ///         <Answer CaseID=”123”>非常好</Answer>
                            ///     </Question>
                            ///  </Answers>    
                            XDocument xDocument = XDocument.Parse("<root>" + Reply.Answer + "</root>", LoadOptions.None);
                            foreach (XElement xElement in xDocument.Descendants("Question"))
                                QuestionIDs.Add(xElement.Attribute("QuestionID").Value);
                        }
                        if (QuestionIDs.Contains(this.Record.UID))
                        {
                            throw new Exception("本題已有學生做答，不得刪除。");
                            //List<K12.Data.ClassRecord> Clazz = K12.Data.Class.SelectByIDs(Replys.Select(x => x.ClassID.ToString()).Distinct());
                            //Error_Message += "做答班級：" + string.Join("、", Clazz.Select(x => x.Name)) + "\n";
                        }
                        else
                        {
                            //  刪問題選項
                            if (this.Record.RecordStatus != FISCA.UDT.RecordStatus.Insert)
                            {
                                List<UDT.QuestionOption> QuestionOptions = Access.Select<UDT.QuestionOption>(string.Format("ref_question_id = {0}", this.Record.UID));
                                QuestionOptions.ForEach(x => x.Deleted = true);
                                QuestionOptions.SaveAll();
                                //  刪問題
                                this.Record.Deleted = true;
                                this.Record.Save();
                            }
                        }
                    }
                    else
                    {
                        //  刪問題選項
                        if (this.Record.RecordStatus != FISCA.UDT.RecordStatus.Insert)
                        {
                            List<UDT.QuestionOption> QuestionOptions = Access.Select<UDT.QuestionOption>(string.Format("ref_question_id = {0}", this.Record.UID));
                            QuestionOptions.ForEach(x => x.Deleted = true);
                            QuestionOptions.SaveAll();
                            //  刪問題
                            this.Record.Deleted = true;
                            this.Record.Save();
                        }
                    }
                }
            });
            task.ContinueWith((x) =>
            {
                if (x.Exception != null)
                {
                    MessageBox.Show(x.Exception.InnerException.Message);
                    this.Loading = false;
                    return;
                } 
                this.Loading = false;
                Event.DeliverDetailContent.RaiseDeleteDetailContentEvent(this, new Event.DeliverDetailContentEventArgs(this));
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext()); 
        }

        private void nudDisplayOrder_ValueChanged(object sender, EventArgs e)
        {
            this.DisplayOrder = this.nudDisplayOrder.Value;
        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            this.Title = "題號" + this.nudDisplayOrder.Value + "：" + this.txtTitle.Text.Trim();
            //  傳送自己給右上角「項目」，設定選單及其文字。
            Event.DeliverDetailContent.RaisePassDetailContentEvent(this, new Event.DeliverDetailContentEventArgs(this));
        }
    }
}
