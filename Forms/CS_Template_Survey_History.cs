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
using mshtml;
using DevComponents.DotNetBar;
using TeachingEvaluation.DataItems;

namespace TeachingEvaluation.Forms
{
    public partial class CS_Template_Survey_History : BaseForm
    {
        private TeachingEvaluation.UDT.CSConfiguration conf;
        private string TemplateName_Subfix = "survey-history-description";
        private bool formload;

        public CS_Template_Survey_History()
        {
            InitializeComponent();

            this.formload = false;
        }

        private void InitSchoolYear()
        {
            int school_year;
            if (int.TryParse(K12.Data.School.DefaultSchoolYear, out school_year))
            {
                this.nudSchoolYear.Value = decimal.Parse(school_year.ToString());
            }
            else
            {
                this.nudSchoolYear.Value = decimal.Parse((DateTime.Today.Year - 1911).ToString());
            }
        }

        private void InitSemester()
        {
            this.cboSemester.DataSource = DataItems.SemesterItem.GetSemesterList();
            this.cboSemester.ValueMember = "Value";
            this.cboSemester.DisplayMember = "Name";

            this.cboSemester.SelectedValue = K12.Data.School.DefaultSemester;
        }

        private void Email_Content_Template_Load(object sender, EventArgs e)
        {
            this.formload = false;

            this.InitSchoolYear();
            this.InitSemester();

            this.formload = true;

            this.SetConf();

            this.Decorate(this.webBrowser1, conf.Content);
        }

        private void SetConf()
        {
            if (!this.formload)
                return;

            string TemplateName = this.nudSchoolYear.Value + "-" + (this.cboSemester.SelectedItem as SemesterItem).Value + "-" + this.TemplateName_Subfix;
            conf = TeachingEvaluation.UDT.CSConfiguration.GetTemplate(TemplateName);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ErrorProvider errorProvider1 = new ErrorProvider();

            try
            {
                this.btnSave.Enabled = false;
                conf.Content = webBrowser1.Document.Body.InnerHtml.Replace("<div id=\"editable\">", string.Empty);
                List<ActiveRecord> recs = new List<ActiveRecord>();
                recs.Add(conf);
                (new AccessHelper()).UpdateValues(recs);
                MessageBox.Show("儲存成功。");
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
            finally
            {
                this.btnSave.Enabled = true;
            }
        }

        private void colorPickerDropDown1_SelectedColorChanged(object sender, EventArgs e)
        {
            this.SetCss("color:" + ColorTranslator.ToHtml(colorPickerDropDown1.SelectedColor));
        }

        private void SetCss(string cssString)
        {
            IHTMLDocument2 htmlDocument = webBrowser1.Document.DomDocument as IHTMLDocument2;

            IHTMLSelectionObject currentSelection = htmlDocument.selection;

            if (currentSelection != null)
            {
                IHTMLTxtRange range = currentSelection.createRange() as IHTMLTxtRange;
                string content = string.Format("<span style='{0}'>{1}</span>", cssString, range.text);
                range.pasteHTML(content);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Decorate(System.Windows.Forms.WebBrowser browser, string content)
        {
            string html_code = string.IsNullOrEmpty(content) ? "&nbsp;" : content;
            browser.Stop();
            if (browser.Document != null)
            {
                browser.Document.OpenNew(true);
                browser.Document.Write(html_code);
            }
            else
            {
                browser.DocumentText = html_code;
            }
            Application.DoEvents();
            if (browser.Document != null)
            {
                browser.Document.Focus();
                browser.Document.DomDocument.GetType().GetProperty("designMode").SetValue(browser.Document.DomDocument, "On", null);
            }
            browser.IsWebBrowserContextMenuEnabled = false;
        }

        private void nudSchoolYear_ValueChanged(object sender, EventArgs e)
        {
            if (!this.formload)
                return;

            this.SetConf();
            this.Decorate(this.webBrowser1, conf.Content);
        }

        private void cboSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.formload)
                return;

            this.SetConf();
            this.Decorate(this.webBrowser1, conf.Content);
        }
    }
}
