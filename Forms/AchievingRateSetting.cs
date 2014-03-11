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
using System.Threading.Tasks;
using TeachingEvaluation.DataItems;

namespace TeachingEvaluation.Forms
{
    public partial class AchievingRateSetting : BaseForm
    {
        private AccessHelper Access;
        private List<UDT.AchievingRate> AchievingRates;

        public AchievingRateSetting()
        {
            InitializeComponent(); 
            
            this.dgvData.DataError += new DataGridViewDataErrorEventHandler(dgvData_DataError);
            this.dgvData.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvData_ColumnHeaderMouseClick);
            this.dgvData.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvData_RowHeaderMouseClick);
            this.dgvData.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvData_MouseClick);
            this.dgvData.CurrentCellDirtyStateChanged += new EventHandler(dgvData_CurrentCellDirtyStateChanged);
            this.dgvData.CellEnter += new DataGridViewCellEventHandler(dgvData_CellEnter);

            Access = new AccessHelper();

            this.Load += new EventHandler(Form_Load);
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
                if (dgvData.CurrentCell != null && dgvData.CurrentCell.GetType().ToString() == "System.Windows.Forms.DataGridViewComboBoxCell")
                    (dgvData.EditingControl as ComboBox).DroppedDown = true;  //自動拉下清單
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

        private void Form_Load(object sender, EventArgs e)
        {
            this.Semester.DataSource = SemesterItem.GetSemesterList();
            this.Semester.ValueMember = "Value";
            this.Semester.DisplayMember = "Name";
            this.Refresh();
        }

        private void Refresh()
        {
            this.circularProgress.Visible = true;
            this.circularProgress.IsRunning = true;
            this.btnSave.Enabled = false;
            this.dgvData.Rows.Clear();

            Task task = Task.Factory.StartNew(() =>
            {
                AchievingRates = Access.Select<UDT.AchievingRate>();
            });
            task.ContinueWith((x) =>
            {
                if (x.Exception != null)
                {
                    MessageBox.Show(x.Exception.InnerException.Message);
                    goto TheEnd;
                }
                foreach (UDT.AchievingRate achievingRate in this.AchievingRates.OrderBy(y => y.SchoolYear).ThenBy(y => y.Semester))
                {
                    List<object> sources = new List<object>();

                    sources.Add(achievingRate.SchoolYear);
                    sources.Add(achievingRate.Semester.ToString());
                    sources.Add(achievingRate.Rate);

                    this.dgvData.Rows.Add(sources.ToArray());
                }

            TheEnd:
                this.circularProgress.Visible = false;
                this.circularProgress.IsRunning = false;
                this.btnSave.Enabled = true;
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool Validated()
        {
            this.dgvData.EndEdit();
            bool is_valid = true;
            Dictionary<string, List<DataGridViewRow>> dicCells = new Dictionary<string, List<DataGridViewRow>>();

            foreach (DataGridViewRow row in this.dgvData.Rows)
            {
                if (row.IsNewRow)
                    continue;

                row.ErrorText = string.Empty;
                row.Cells[0].ErrorText = string.Empty;
                row.Cells[1].ErrorText = string.Empty;
                row.Cells[2].ErrorText = string.Empty;

                if (string.IsNullOrWhiteSpace(row.Cells[0].Value + ""))
                {
                    row.Cells[0].ErrorText = "必填。";
                    is_valid = false;
                }
                if (string.IsNullOrWhiteSpace(row.Cells[2].Value + ""))
                {
                    row.Cells[2].ErrorText = "必填。";
                    is_valid = false;
                }
                else
                {
                    decimal rate = 0;
                    if (!decimal.TryParse(row.Cells[2].Value + "", out rate))
                    {
                        row.Cells[2].ErrorText = "限填數字。";
                        is_valid = false;
                    }
                    else if (rate < 0 || rate > 100)
                    {
                        row.Cells[2].ErrorText = "合理範圍「0~100」。";
                        is_valid = false;
                    }
                }
                if (!string.IsNullOrWhiteSpace(row.Cells[0].Value + "") && !string.IsNullOrWhiteSpace(row.Cells[2].Value + ""))
                {
                    string key = (row.Cells[0].Value + "") + "-" + (row.Cells[1].Value + "");
                    if (!dicCells.ContainsKey(key))
                        dicCells.Add(key, new List<DataGridViewRow>());

                    dicCells[key].Add(row);
                }
            }
            foreach (string key in dicCells.Keys)
            {
                if (dicCells[key].Count > 1)
                {
                    is_valid = false;
                    foreach(DataGridViewRow row in dicCells[key])
                    {
                        row.Cells[0].ErrorText = "學年期的組合重覆。";
                        row.Cells[1].ErrorText = "學年期的組合重覆。";
                    }
                }
            }

            return is_valid;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.btnSave.Enabled = false;

            if (!this.Validated())
            {
                MessageBox.Show("請先修正錯誤再儲存。");
                this.btnSave.Enabled = true;
                return;
            }

            this.circularProgress.Visible = true;
            this.circularProgress.IsRunning = true;

            List<UDT.AchievingRate> oAchievingRates = new List<UDT.AchievingRate>();
            foreach (DataGridViewRow row in this.dgvData.Rows)
            {
                if (row.IsNewRow)
                    continue;

                UDT.AchievingRate achievingRate = new UDT.AchievingRate();

                achievingRate.SchoolYear = int.Parse(row.Cells["SchoolYear"].Value + "");
                achievingRate.Semester = int.Parse(row.Cells["Semester"].Value + "");
                achievingRate.Rate = decimal.Parse(row.Cells["AchievingRate"].Value + "");
                achievingRate.TimeStamp = DateTime.Now;
                achievingRate.UpdateAccount = "";

                oAchievingRates.Add(achievingRate);
            }
            Task task = Task.Factory.StartNew(() =>
            {
                this.AchievingRates.ForEach(x => x.Deleted = true);
                this.AchievingRates.SaveAll();
                this.AchievingRates.Clear();
                oAchievingRates.SaveAll();
            });
            task.ContinueWith((x)=>
            {
                if (x.Exception != null)
                {
                    MessageBox.Show(x.Exception.InnerException.Message);
                    goto TheEnd;
                }
                else
                    MessageBox.Show("儲存成功。");
            TheEnd:
                this.circularProgress.Visible = false;
                this.circularProgress.IsRunning = false;
                this.btnSave.Enabled = true;
                this.Refresh();
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

        }
    }
}