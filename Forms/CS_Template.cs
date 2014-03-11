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

namespace TeachingEvaluation.Forms
{
    public partial class Email_Content_Template : BaseForm
    {
        private TeachingEvaluation.UDT.CSConfiguration conf;
        private TeachingEvaluation.UDT.CSConfiguration conf_subject;
        //private BrowserWrapper.BrowserProxy proxy = new BrowserWrapper.BrowserProxy();
        private TeachingEvaluation.UDT.CSConfiguration.TemplateName TemplateName;
        private object CurrentObject;

        public Email_Content_Template(TeachingEvaluation.UDT.CSConfiguration.TemplateName TemplateName)
        {
            InitializeComponent();

            this.TemplateName = TemplateName;
            this.CurrentObject = new object();

            webBrowser1.GotFocus += new EventHandler(webBrowser1_GotFocus);
            this.btnStudentName.Click += new System.EventHandler(this.btn_Click);
            this.btnTeacherName.Click += new System.EventHandler(this.btn_Click);
            this.btnSubjectName.Click += new System.EventHandler(this.btn_Click);
            this.btnCourseName.Click += new System.EventHandler(this.btn_Click);
            this.btnSchoolYear.Click += new System.EventHandler(this.btn_Click);
            this.btnSemester.Click += new System.EventHandler(this.btn_Click);
            this.btnBeginTime.Click += new System.EventHandler(this.btn_Click);
            this.btnEndTime.Click += new System.EventHandler(this.btn_Click);
        }

        private void Email_Content_Template_Load(object sender, EventArgs e)
        {
            conf = TeachingEvaluation.UDT.CSConfiguration.GetTemplate(this.TemplateName);
            this.Decorate(this.webBrowser1, conf.Content);
            conf_subject = TeachingEvaluation.UDT.CSConfiguration.GetTemplate(this.TemplateName.ToString() + "_subject");
            this.txtSubject.Text = conf_subject.Content;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ErrorProvider errorProvider1 = new ErrorProvider();
            if (string.IsNullOrWhiteSpace(this.txtSubject.Text))
            {
                errorProvider1.SetError(this.txtSubject, "必填。");
                return;
            }
            else
                errorProvider1.SetError(this.txtSubject, "");

            try
            {
                this.btnSave.Enabled = false;
                conf.Content = webBrowser1.Document.Body.InnerHtml.Replace("<div id=\"editable\">", string.Empty);
                conf_subject.Content = this.txtSubject.Text.Trim();
                List<ActiveRecord> recs = new List<ActiveRecord>();
                recs.Add(conf);
                recs.Add(conf_subject);
                (new AccessHelper()).UpdateValues(recs);
                MessageBox.Show("儲存成功。");
                //this.Close();
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
                //MessageBox.Show(range.htmlText);
                string content = string.Format("<span style='{0}'>{1}</span>", cssString, range.text);
                range.pasteHTML(content);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            if (this.CurrentObject.GetType() == this.webBrowser1.GetType())
            {
                IHTMLDocument2 htmlDocument = webBrowser1.Document.DomDocument as IHTMLDocument2;

                IHTMLSelectionObject currentSelection = htmlDocument.selection;

                if (currentSelection != null)
                {
                    IHTMLTxtRange range = currentSelection.createRange() as IHTMLTxtRange;
                    range.text = "[[" + (sender as ButtonItem).Text.Trim() + "]]";
                    //MessageBox.Show(range.htmlText);
                    //string content = string.Format("<span>{1}</span>", range.text);
                    range.pasteHTML(range.text);
                }
            }
            else
            {
                if (this.txtSubject.SelectedText.Length > 0)
                    this.txtSubject.Text = this.txtSubject.Text.Replace(this.txtSubject.SelectedText, "[[" + (sender as ButtonItem).Text.Trim() + "]]");
                else
                {
                    if (this.txtSubject.Text.Length > 0)
                    {
                        int selectionStart = this.txtSubject.SelectionStart;
                        int textLength = this.txtSubject.TextLength;

                        string leftSubject = string.Empty;
                        string rightSubject = string.Empty;

                        if (selectionStart == 0)
                        {
                            leftSubject = string.Empty;
                            rightSubject = txtSubject.Text;
                        }
                        else if (selectionStart == textLength)
                        {
                            leftSubject = txtSubject.Text;
                            rightSubject = string.Empty;
                        }
                        else
                        {
                            leftSubject = this.txtSubject.Text.Substring(0, selectionStart);
                            rightSubject = this.txtSubject.Text.Substring(selectionStart, textLength - leftSubject.Length);
                        }
                        this.txtSubject.Text = leftSubject + "[[" + (sender as ButtonItem).Text.Trim() + "]]" + rightSubject;

                        //this.txtSubject.SelectionStart = selectionStart;
                    }
                    else
                        this.txtSubject.Text = "[[" + (sender as ButtonItem).Text.Trim() + "]]";
                }
            }
        }

        private void webBrowser1_GotFocus(object sender, EventArgs e)
        {
            this.CurrentObject = this.webBrowser1;
        }

        private void txtSubject_Enter(object sender, EventArgs e)
        {
            this.CurrentObject = this.txtSubject;
        }

        private void Decorate(System.Windows.Forms.WebBrowser browser, string content)
        {
            System.Windows.Forms.WebBrowser WebBrowser1 = browser;

            WebBrowser1.Navigate("about:blank");
            WebBrowser1.Document.OpenNew(false).Write("<html><body><div id=\"editable\">" + content + "</div></body></html>");

            //'turns off document body editing
            foreach (System.Windows.Forms.HtmlElement el in WebBrowser1.Document.All)
            {
                el.SetAttribute("unselectable", "on");
                el.SetAttribute("contenteditable", "false");
            }
            WebBrowser1.Document.Body.All["editable"].SetAttribute("width", "100%");
            WebBrowser1.Document.Body.All["editable"].SetAttribute("height", "100%");
            WebBrowser1.Document.Body.All["editable"].SetAttribute("contenteditable", "true");
            WebBrowser1.Document.Body.All["editable"].Focus();
            this.CurrentObject = WebBrowser1;
            WebBrowser1.Document.DomDocument.GetType().GetProperty("designMode").SetValue(WebBrowser1.Document.DomDocument, "On", null);
            WebBrowser1.IsWebBrowserContextMenuEnabled = false;
        }
    }
}
