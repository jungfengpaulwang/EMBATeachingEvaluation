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

namespace TeachingEvaluation.Forms
{
    public partial class frmHierarchyMangagement : BaseForm
    {
        private AccessHelper Access;
        private ErrorProvider errorProvider1;

        public frmHierarchyMangagement()
        {
            InitializeComponent();

            this.Access = new AccessHelper();
            this.errorProvider1 = new ErrorProvider();

            this.Load += new EventHandler(Form_Load);
        }

        private void Form_Load(object sender, EventArgs e)
        {
            this.dgvData.DataError += new DataGridViewDataErrorEventHandler(dgvData_DataError);
            this.dgvData.CurrentCellDirtyStateChanged += new EventHandler(dgvData_CurrentCellDirtyStateChanged);
            this.dgvData.CellEnter += new DataGridViewCellEventHandler(dgvData_CellEnter);
            this.dgvData.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvData_ColumnHeaderMouseClick);
            this.dgvData.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvData_RowHeaderMouseClick);
            this.dgvData.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvData_MouseClick);

            this.DataBind();
        }

        private void DataBind()
        {
            this.dgvData.Rows.Clear();

            List<UDT.Hierarchy> udts = this.Access.Select<UDT.Hierarchy>();

            udts.ForEach((x) =>
            {
                List<object> sources = new List<object>();

                sources.Add(x.Title);
                sources.Add(x.DisplayOrder);

                int idx = this.dgvData.Rows.Add(sources.ToArray());
                this.dgvData.Rows[idx].Cells[0].Tag = x.Title;
            });
        }

        private void dgvData_MouseClick(object sender, MouseEventArgs e)
        {
            MouseEventArgs args = (MouseEventArgs)e;
            DataGridView dgv = (DataGridView)sender;
            DataGridView.HitTestInfo hit = dgv.HitTest(args.X, args.Y);

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

        private void dgvData_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvData.CurrentCell = null;
            dgvData.Rows[e.RowIndex].Selected = true;
        }

        private void dgvData_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvData.CurrentCell = null;
            dgvData.Columns[e.ColumnIndex].Selected = true;
        }

        private void dgvData_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvData.BeginEdit(true);
        }

        private void dgvData_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvData.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private bool Is_Validate()
        {
            bool valid = true;
            this.errorProvider1.Clear();

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
                            row.Cells[0].ErrorText = "重複。";
                    }
                    if (row.Cells[1].Value != null)
                    {
                        if (this.dgvData.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[1].Value + "" == row.Cells[1].Value + "").Count() > 1)
                            row.Cells[1].ErrorText = "重複。";
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
                                    cell.ErrorText = "限填整數。";
                            }
                        }
                    }
                }
            }
            return valid;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (!Is_Validate())
            {
                MessageBox.Show("請先修正錯誤。");
                return;
            }

            string msg = string.Empty;
            List<UDT.QHRelation> QHRelations = Access.Select<UDT.QHRelation>();
            List<UDT.QHRelation> QHRelations_New = new List<UDT.QHRelation>();
            Dictionary<string, List<UDT.QHRelation>> dicQHRelations = new Dictionary<string, List<UDT.QHRelation>>();
            if (QHRelations.Count > 0)
            {
                QHRelations.ForEach((x) =>
                {
                    if (!dicQHRelations.ContainsKey(x.HierarchyTitle.Trim()))
                        dicQHRelations.Add(x.HierarchyTitle.Trim(), new List<UDT.QHRelation>());

                    dicQHRelations[x.HierarchyTitle.Trim()].Add(x);
                });
            }
            this.Save.Enabled = false;
            List<UDT.Hierarchy> udts = this.Access.Select<UDT.Hierarchy>();
            udts.ForEach(x => x.Deleted = true);
            udts.SaveAll();
            udts.Clear();
            this.dgvData.Rows.Cast<DataGridViewRow>().ToList().ForEach((x) =>
            {
                if (!x.IsNewRow)
                {
                    UDT.Hierarchy udt = new UDT.Hierarchy();

                    udt.Title = (x.Cells[0].Value + "").Trim();
                    udt.DisplayOrder = int.Parse(x.Cells[1].Value + "");

                    udts.Add(udt);

                    if ((x.Cells[0].Tag + "").Trim() != (x.Cells[0].Value + "").Trim())
                    {
                        if (dicQHRelations.ContainsKey((x.Cells[0].Tag + "").Trim()))
                        {
                            foreach (UDT.QHRelation QHRelation in dicQHRelations[(x.Cells[0].Tag + "").Trim()])
                            {
                                QHRelation.HierarchyTitle = (x.Cells[0].Value + "").Trim();
                                QHRelations_New.Add(QHRelation);
                            }
                        }
                    }
                }
            });
            udts.SaveAll(); 
            QHRelations_New.SaveAll();
            this.DataBind();
            this.Save.Enabled = true;
            MessageBox.Show("儲存成功。");
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
