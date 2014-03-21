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
using System.IO;
using System.Threading.Tasks;

namespace TeachingEvaluation.Forms
{
    public partial class TemplateManagement : BaseForm
    {
        private AccessHelper Access;
        private QueryHelper Query;
        private bool downloading;

        public TemplateManagement()
        {
            InitializeComponent();

            Access = new AccessHelper();
            Query = new QueryHelper();

            this.Load += new EventHandler(TemplateManagement_Load);
        }

        private void TemplateManagement_Load(object sender, EventArgs e)
        {
            this.BindReceiptTemplate();
        }

        private void BindReceiptTemplate()
        {
            this.CircleProgress.Visible = true;
            this.CircleProgress.IsRunning = true;
            this.Upload.Enabled = false;
            this.Update.Enabled = false;
            this.Delete.Enabled = false;

            this.dgvData.Rows.Clear();

            DataTable dataTable = new DataTable();

            Task<DataTable> task = Task < DataTable>.Factory.StartNew(() =>
            {
                dataTable = Query.Select("select survey.uid, survey.name from $ischool.emba.teaching_evaluation.survey as survey join $ischool.emba.teaching_evaluation.report_template as template on survey.uid=template.ref_survey_id order by survey.uid;");
                return dataTable;
            });
            task.ContinueWith((x) =>
            {
                if (x.Exception != null)
                {
                    MessageBox.Show(x.Exception.InnerException.Message);
                    goto TheEnd;
                }
                foreach (DataRow row in x.Result.Rows)
                {
                    List<object> source = new List<object>();

                    source.Add(row["name"] + "");
                    source.Add("檢視");

                    int idx = this.dgvData.Rows.Add(source.ToArray());
                    this.dgvData.Rows[idx].Tag = row["uid"] + "";
                }
            TheEnd:
                this.CircleProgress.Visible = false;
                this.CircleProgress.IsRunning = false;
                this.Upload.Enabled = true;
                this.Update.Enabled = true;
                this.Delete.Enabled = true;

            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Upload_Click(object sender, EventArgs e)
        {
            Forms.ImportReportDocumentTemplate form = new ImportReportDocumentTemplate();
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.BindReceiptTemplate();
        }

        private void Update_Click(object sender, EventArgs e)
        {
            if (this.dgvData.SelectedRows.Count > 1)
            {
                MessageBox.Show("僅允許單筆修改。");
                return;
            }
            this.Delete.Enabled = false;
            this.Update.Enabled = false;
            this.Upload.Enabled = false;
            this.CircleProgress.Visible = true;
            this.CircleProgress.IsRunning = true;
            string SurveyID = this.dgvData.CurrentRow.Tag + "";
            Task<UDT.ReportTemplate> task = Task<UDT.ReportTemplate>.Factory.StartNew(() =>
            {
                List<UDT.ReportTemplate> templates = Access.Select<UDT.ReportTemplate>(string.Format("ref_survey_id = {0}", SurveyID));
                return templates.ElementAt(0);
            });
            task.ContinueWith((x) =>
            {
                if (x.Exception != null)
                {
                    MessageBox.Show(x.Exception.InnerException.Message);
                    goto TheEnd;
                }
                UDT.ReportTemplate template = x.Result;
                string TemplateName = this.dgvData.CurrentRow.Cells[0].Value + "";
                Forms.ImportReportDocumentTemplate form = new ImportReportDocumentTemplate(template, TemplateName);
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.BindReceiptTemplate();
                }
            TheEnd:
                this.Delete.Enabled = true;
                this.Update.Enabled = true;
                this.Upload.Enabled = true;
                this.CircleProgress.Visible = false;
                this.CircleProgress.IsRunning = false;
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext()); 
        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == this.dgvData.NewRowIndex)
                return;

            if (this.downloading)
                return;

            if (e.ColumnIndex == 1)
            {
                this.downloading = true;
                this.Delete.Enabled = false;
                this.Update.Enabled = false;
                this.Upload.Enabled = false;
                this.CircleProgress.Visible = true;
                this.CircleProgress.IsRunning = true;
                string SurveyID = this.dgvData.CurrentRow.Tag + "";
                Task<UDT.ReportTemplate> task = Task<UDT.ReportTemplate>.Factory.StartNew(() =>
                {
                    List<UDT.ReportTemplate> templates = Access.Select<UDT.ReportTemplate>(string.Format("ref_survey_id = {0}", SurveyID));
                    return templates.ElementAt(0);
                });
                task.ContinueWith((x) =>
                {
                    if (x.Exception != null)
                    {
                        MessageBox.Show(x.Exception.InnerException.Message);
                        goto TheEnd;
                    }
                    try
                    {
                        DownloadDocument(x.Result.Template, this.dgvData.CurrentRow.Cells[0].Value + "");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                TheEnd:
                    this.downloading = false;
                    this.Delete.Enabled = true;
                    this.Update.Enabled = true;
                    this.Upload.Enabled = true;
                    this.CircleProgress.Visible = false;
                    this.CircleProgress.IsRunning = false;
                }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void DownloadDocument(string base64string, string TemplateName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(base64string))
                    throw new Exception("無樣版檔。");

                byte[] _buffer = Convert.FromBase64String(base64string);
                MemoryStream template = new MemoryStream(_buffer);

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "另存新檔";
                sfd.FileName = string.Format("{0}-教學意見表樣版.xls", TemplateName);
                sfd.Filter = "相容於 Excel 2003 檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        FileStream fs = new FileStream(sfd.FileName, FileMode.Create);

                        fs.Write(_buffer, 0, _buffer.Length);
                        fs.Close();
                        System.Diagnostics.Process.Start(sfd.FileName);
                    }
                    catch
                    {
                        throw new Exception("指定路徑無法存取。");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (this.dgvData.SelectedRows.Count == 0)
            {
                MessageBox.Show("請先選取待刪除樣版。");
                return;
            }
            if (this.dgvData.SelectedRows.Count > 1)
            {
                MessageBox.Show("僅允許單筆刪除。");
                return;
            }

            if (MessageBox.Show("確定刪除？", "提醒", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                return;

            this.Delete.Enabled = false;
            this.Update.Enabled = false;
            this.Upload.Enabled = false;
            this.CircleProgress.Visible = true;
            this.CircleProgress.IsRunning = true;

            string SurveyID = this.dgvData.CurrentRow.Tag + "";

            Task task = Task.Factory.StartNew(() =>
            {
                List<UDT.ReportTemplate> templates = Access.Select<UDT.ReportTemplate>(string.Format("ref_survey_id = {0}", SurveyID));
                templates.ForEach(y => y.Deleted = true);
                templates.SaveAll();
            });
            task.ContinueWith((x) =>
            {
                if (x.Exception != null)
                {
                    MessageBox.Show(x.Exception.InnerException.Message);
                    goto TheEnd;
                }
                this.BindReceiptTemplate();
                TheEnd:
                    this.Delete.Enabled = true;
                    this.Update.Enabled = true;
                    this.Upload.Enabled = true;
                    this.CircleProgress.Visible = false;
                    this.CircleProgress.IsRunning = false;
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());  
        }
    }
}
