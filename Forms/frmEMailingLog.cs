using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using FISCA.Data;
using System.Threading.Tasks;
using Aspose.Cells;

namespace TeachingEvaluation.Forms
{
    public partial class frmEMailingLog : BaseForm
    {
        private string BatchGUID { set; get; }
        private QueryHelper Query;
        private DataTable dataTable;

        public frmEMailingLog(string BatchGUID)
        {
            InitializeComponent();

            this.BatchGUID = BatchGUID;
            this.Query = new QueryHelper();
            this.dataTable = new DataTable();

            this.circularProgress.Visible = false;
            this.circularProgress.IsRunning = false;

            this.Load += new EventHandler(Form_Load);
        }

        private void Form_Load(object sender, EventArgs e)
        {
            this.circularProgress.Visible = true;
            this.circularProgress.IsRunning = true;

            Task task = Task.Factory.StartNew(() =>
            {
                dataTable = Query.Select(string.Format(@"select course_name as 開課, teacher_name as 授課教師, student_number as 學號, student_name as 姓名, recipient_email_address as 待發送之電子郵件, result_email as 已寄出之電子郵件, error_message as 失敗訊息, reject_reason as 退信原因, is_cc as 副本 from $ischool.emba.mandrill.send.log where guid='{0}'", this.BatchGUID));
            });
            task.ContinueWith((x)=>
            {
                foreach (DataRow row in this.dataTable.Rows)
                {
                    List<object> source = new List<object>();

                    source.Add(row["開課"] + "");
                    source.Add(row["授課教師"] + "");
                    source.Add(row["學號"] + "");
                    source.Add(row["姓名"] + "");
                    source.Add(row["待發送之電子郵件"] + "");
                    source.Add(row["已寄出之電子郵件"] + "");
                    source.Add(row["失敗訊息"] + "");
                    source.Add(row["退信原因"] + "");
                    source.Add(row["副本"] + "");

                    int idx = this.dgvData.Rows.Add(source.ToArray());
                }
                this.circularProgress.Visible = false;
                this.circularProgress.IsRunning = false;
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dataTable.Rows.Count == 0)
                return;

            Workbook wb = new Workbook();
            foreach (Worksheet sheet in wb.Worksheets.Cast<Worksheet>().ToList())
                wb.Worksheets.RemoveAt(sheet.Name);

            int sheet_index = wb.Worksheets.Add();
            wb.Worksheets[sheet_index].Cells.ImportDataTable(dataTable, true, "A1");
            wb.Worksheets[sheet_index].Name = "發送 Email 提醒通知 Log";

            wb.Worksheets.Cast<Worksheet>().ToList().ForEach(y => y.AutoFitColumns());
            SaveFileDialog sd = new SaveFileDialog();
            sd.Title = "另存新檔";
            sd.FileName = string.Format("{0}_{1}.xls", "發送 Email 提醒通知 Log", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Replace("/", "_").Replace(":", "_"));
            sd.Filter = "Excel 2003 相容檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
            if (sd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    wb.Save(sd.FileName, FileFormatType.Excel2003);
                    System.Diagnostics.Process.Start(sd.FileName);
                }
                catch
                {
                    MessageBox.Show("指定路徑無法存取。", "建立檔案失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //return;
                }
            }
        }
    }
}