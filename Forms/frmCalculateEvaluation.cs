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
using FISCA.Data;
using System.Dynamic;
using System.Xml.Linq;
using Agent.MVC.Viewer;

namespace TeachingEvaluation.Forms
{
    public partial class frmCalculateEvaluation : BaseForm
    {
        private bool form_loaded;
        private AccessHelper Access;
        private QueryHelper Query;
        private ComboBox cmb;

        public frmCalculateEvaluation()
        {
            InitializeComponent();

            this.Load += new EventHandler(frmCalculateValuation_Load);
            this.dgvData.CellBeginEdit += new DataGridViewCellCancelEventHandler(dgvData_CellBeginEdit);
            this.dgvData.CellEndEdit += new DataGridViewCellEventHandler(dgvData_CellEndEdit);
            this.dgvData.CellFormatting += new DataGridViewCellFormattingEventHandler(dgvData_CellFormatting);
            dgvData.CurrentCellDirtyStateChanged += new EventHandler(dgvData_CurrentCellDirtyStateChanged);
            //dgvData.CellEnter += new DataGridViewCellEventHandler(dgvData_CellEnter);
            //this.dgvData.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvData_ColumnHeaderMouseClick);
            //this.dgvData.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvData_RowHeaderMouseClick);
            //this.dgvData.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvData_MouseClick);

            this.dgvData.DataError += new DataGridViewDataErrorEventHandler(dgvData_DataError);
        }

        private void frmCalculateValuation_Load(object sender, EventArgs e)
        {
            this.form_loaded = false;

            Access = new FISCA.UDT.AccessHelper();
            Query = new FISCA.Data.QueryHelper();

            this.InitStatisticsGroup();

            cmb = new ColorComboBox();
            cmb.Visible = false;
            cmb.DrawMode = DrawMode.OwnerDrawFixed;
            this.dgvData.Controls.Add(cmb);

            this.form_loaded = true;
        }

        private void dgvData_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        void dgvData_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvData.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvData_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex > 2 && dgvData.SelectedCells.Count == 1)
            {
                dgvData.BeginEdit(true);
            }
        }

        private void dgvData_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //dgvData.CurrentCell = null;
            //dgvData.Rows[e.RowIndex].Selected = true;
        }

        private void dgvData_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //dgvData.CurrentCell = null;
            //dgvData.Columns[e.ColumnIndex].Selected = true;
        }

        //private void dgvData_MouseClick(object sender, MouseEventArgs e)
        //{
        //    MouseEventArgs args = (MouseEventArgs)e;
        //    DataGridView dgv = (DataGridView)sender;
        //    DataGridView.HitTestInfo hit = dgv.HitTest(args.X, args.Y);

        //    if (hit.Type == DataGridViewHitTestType.TopLeftHeader)
        //    {
        //        dgvData.CurrentCell = null;
        //        dgvData.SelectAll();
        //    }
        //}

        //private void cmb_DrawItem(object sender, DrawItemEventArgs e)
        //{
        //    e.DrawBackground();
        //    ComboBox cmb = (ComboBox)sender;
        //    Color color = (Color)cmb.Items[e.Index];
        //    e.Graphics.FillRectangle(new SolidBrush(color), e.Bounds);
        //    e.Graphics.DrawString(color.Name, e.Font, new SolidBrush(e.ForeColor), e.Bounds);
        //}

        private void dgvData_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex > 2)
            {
                Rectangle ret = this.dgvData.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                this.cmb.Location = ret.Location;
                this.cmb.Width = ret.Width;
                this.cmb.DroppedDown = true;
                if (this.dgvData.CurrentCell.Value != null)
                {
                    this.cmb.SelectedItem = (Color)this.dgvData.CurrentCell.Value;
                }
                this.cmb.Visible = true;
            }
            else
            {
                this.cmb.Visible = false;
            }
        }

        private void dgvData_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            this.dgvData.CurrentCell.Value = this.cmb.SelectedItem;
            this.cmb.Visible = false;
        }

        private void dgvData_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex >2 && e.RowIndex != this.dgvData.NewRowIndex && e.Value != null)
            {
                e.CellStyle.BackColor = (Color)e.Value;
            }
        }

        private void InitStatisticsGroup()
        {
            this.dgvData.Rows.Clear();

            List<UDT.StatisticsGroup> StatisticsGroups = Access.Select<UDT.StatisticsGroup>();
            List<UDT.Survey> Surveys = Access.Select<UDT.Survey>();
            Dictionary<string, UDT.Survey> dicSurveys = new Dictionary<string,UDT.Survey>();
            if (Surveys.Count > 0)
                dicSurveys = Surveys.ToDictionary(x=>x.UID);

            if (StatisticsGroups.Count == 0)
                return;

            foreach (UDT.StatisticsGroup StatisticsGroup in StatisticsGroups)
            {
                if (!dicSurveys.ContainsKey(StatisticsGroup.SurveyID.ToString()))
                    continue;

                List<object> source = new List<object>();

                source.Add(dicSurveys[StatisticsGroup.SurveyID.ToString()].Name);
                source.Add(StatisticsGroup.Name);

                /// <Questions>
                ///     <Question QuestionID="123" DisplayOrder="1">本課程的內容和學習目標十分明確</Question>
                ///     <Question QuestionID="456" DisplayOrder="2">本課程上課內容充實，且符合教學大綱</Question>
                /// </Questions>

                XDocument xDocument = XDocument.Parse(StatisticsGroup.DisplayOrderList, LoadOptions.None);
                List<XElement> xElements = xDocument.Descendants("Question").ToList();
                if (xElements.Count() == 0)
                    source.Add("");
                else
                    source.Add(string.Join("、", xElements.Select(x => x.Attribute("DisplayOrder").Value)));

                if (!string.IsNullOrEmpty(StatisticsGroup.QuestionBgColor))
                    source.Add(Color.FromName(StatisticsGroup.QuestionBgColor));
                else
                    source.Add(null);

                if (!string.IsNullOrEmpty(StatisticsGroup.EvaluationBgColor))
                    source.Add(Color.FromName(StatisticsGroup.EvaluationBgColor));
                else
                    source.Add(null);

                int idx = this.dgvData.Rows.Add(source.ToArray());
                this.dgvData.Rows[idx].Tag = StatisticsGroup;
                this.dgvData.Rows[idx].Cells[0].Tag = dicSurveys[StatisticsGroup.SurveyID.ToString()];
            }
            this.dgvData.CurrentCell = null;
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmStatistics_SingleForm form = new frmStatistics_SingleForm(null, null);

            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.InitStatisticsGroup();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (this.dgvData.SelectedRows.Count == 0)
            {
                MessageBox.Show("請選擇待修改項目。");
                return;
            }
            if (this.dgvData.SelectedRows.Count > 1)
            {
                MessageBox.Show("僅能單筆刪除。");
                return;
            }
            UDT.Survey Survey = this.dgvData.SelectedRows[0].Cells[0].Tag as UDT.Survey;
            UDT.StatisticsGroup StatisticsGroup = this.dgvData.SelectedRows[0].Tag as UDT.StatisticsGroup;
            frmStatistics_SingleForm form = new frmStatistics_SingleForm(StatisticsGroup, Survey);

            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.InitStatisticsGroup();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<UDT.StatisticsGroup> StatisticsGroups = new List<UDT.StatisticsGroup>();

            if (MessageBox.Show("確定刪除？", "警告", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                return;
            foreach (DataGridViewRow DataGridViewRow in this.dgvData.SelectedRows)
            {
                UDT.StatisticsGroup StatisticsGroup = DataGridViewRow.Tag as UDT.StatisticsGroup;
                StatisticsGroup.Deleted = true;
                StatisticsGroups.Add(StatisticsGroup);
            }
            StatisticsGroups.SaveAll();
            this.InitStatisticsGroup();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<UDT.StatisticsGroup> StatisticsGroups = new List<UDT.StatisticsGroup>();
            foreach (DataGridViewRow DataGridViewRow in this.dgvData.Rows)
            {
                UDT.StatisticsGroup StatisticsGroup = DataGridViewRow.Tag as UDT.StatisticsGroup;

                if (!string.IsNullOrEmpty(DataGridViewRow.Cells[3].Value + ""))
                    StatisticsGroup.QuestionBgColor = ((Color)DataGridViewRow.Cells[3].Value).Name;
                if (!string.IsNullOrEmpty(DataGridViewRow.Cells[4].Value + ""))
                    StatisticsGroup.EvaluationBgColor = ((Color)DataGridViewRow.Cells[4].Value).Name;

                StatisticsGroups.Add(StatisticsGroup);
            }
            StatisticsGroups.SaveAll();
            MessageBox.Show("儲存成功。");
        }
    }
}