using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using FISCA.UDT;
using FISCA.Data;
using DevComponents.Editors;
using System.Collections.Generic;

namespace TeachingEvaluation.Forms
{
    public partial class ImportReportDocumentTemplate : FISCA.Presentation.Controls.BaseForm
    {
        private AccessHelper Access;
        private QueryHelper Query;
        private UDT.ReportTemplate Template;
        private string TemplateName;

        public ImportReportDocumentTemplate(UDT.ReportTemplate template = null, string templateName = "")
        {
            InitializeComponent();

            Access = new AccessHelper();
            Query = new QueryHelper();
            this.Template = template;
            this.TemplateName = templateName;

            this.Load += new System.EventHandler(this.ImportReceiptDocumentTemplate_Load);
        }

        private void ImportReceiptDocumentTemplate_Load(object sender, EventArgs e)
        {
            DataBind();
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog sd1 = new OpenFileDialog();
            sd1.Title = "上載教學意見表樣版";
            sd1.Filter = "相容於 Excel 2003 檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
            if (sd1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    this.FileName.Text = sd1.FileName;
                }
                catch
                {
                    MessageBox.Show("指定檔案無法開啟。", "開啟檔案失敗", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
        }

        private void DataBind()
        {
            this.Survey.Items.Clear();
            DataTable myTable = Query.Select(@"select uid, name from $ischool.emba.teaching_evaluation.survey");
            //ComboItem ComboItem1 = new ComboItem("");
            //this.Survey.Items.Add(ComboItem1);
            //this.Survey.SelectedItem = ComboItem1;
            foreach (DataRow row in myTable.Rows)
            {
                ComboItem ComboItem = new ComboItem(row["name"] + "");
                ComboItem.Tag = row["uid"] + "";

                this.Survey.Items.Add(ComboItem);
                if (this.Template != null && this.Template.SurveyID.ToString() == (row["uid"] + ""))
                    this.Survey.SelectedItem = ComboItem;
            }
        }

        private string TransferFileToBase64String(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);

            byte[] tempBuffer = new byte[fs.Length];
            fs.Read(tempBuffer, 0, tempBuffer.Length);
            fs.Close();

            MemoryStream ms = new MemoryStream(tempBuffer);

            try
            {
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();

                wb.Open(ms, Aspose.Cells.FileFormatType.Excel2003);
            }
            catch
            {
                throw new Exception("教學意見表樣版限用相容於 Excel 2003 檔案。");
            }

            try
            {
                return Convert.ToBase64String(tempBuffer);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool WriteTemplate(string base64string, int SurveyID)
        {
            try
            {
                if (this.Template != null)
                {
                    if (!string.IsNullOrEmpty(base64string))
                    {
                        if (MessageBox.Show("樣版檔已存在，是否覆蓋？", "提示", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                            return false;
                    }
                }
                else
                {
                    this.Template = new UDT.ReportTemplate();
                }

                if (!string.IsNullOrEmpty(base64string))
                {
                    this.Template.Template = base64string;
                    this.Template.SurveyID = SurveyID;
                }

                this.Template.Save();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void Import_Click(object sender, EventArgs e)
        {
            bool OK = true;
            ErrorProvider errorProvider = new ErrorProvider();

            if (string.IsNullOrWhiteSpace(Survey.Text))
            {
                errorProvider.SetError(this.Survey, "必填。");
                OK = false;
            }
            else
                errorProvider.SetError(this.Survey, "");

            if (this.Template == null && string.IsNullOrWhiteSpace(FileName.Text))
            {
                errorProvider.SetError(this.FileName, "請選擇樣版檔。");
                OK = false;
            }
            else
            {
                errorProvider.SetError(this.FileName, "");
            }

            if (!OK)
                return;

            try
            {
                string base64string = string.Empty;
                    
                if (!string.IsNullOrEmpty(this.FileName.Text))
                    base64string = TransferFileToBase64String(this.FileName.Text);

                int SurveyID = int.Parse((this.Survey.Items[this.Survey.SelectedIndex] as ComboItem).Tag + "");

                if (this.WriteTemplate(base64string, SurveyID))
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    MessageBox.Show("儲存成功。");
                    this.Close();
                    return;
                }
                else
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }            
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}