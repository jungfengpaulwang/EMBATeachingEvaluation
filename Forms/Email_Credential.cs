using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using System.Text.RegularExpressions;

namespace TeachingEvaluation.Forms
{
    /// <summary>
    /// 處理當要發送Email 時，使用者要輸入 SMTP 的認證資訊。
    /// </summary>
    public partial class Email_Credential : BaseForm
    {
        internal delegate void InputCredentialHandler(object sender, EmailCredentialEventArgs args) ;
        internal event InputCredentialHandler AfterInputCredential ;

        public Email_Credential()
        {
            InitializeComponent();
        }

        private void Email_Credential_Load(object sender, EventArgs e)
        {
            UDT.CSConfiguration conf = UDT.CSConfiguration.GetEmailSenderInfo();
            this.txtUserID.Text = conf.Content;
            toolTip1.SetToolTip(this.txtCC, "如有多位收件者，請以『,』分隔！");
        }

        private bool isValidEmail(string email)
        {
            string patternStrict = @"^(([^<>()[\]\\.,;:\s@\""]+"
                       + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                       + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                       + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                       + @"[a-zA-Z]{2,}))$";
            Regex reStrict = new Regex(patternStrict);

            //                      bool isLenientMatch = reLenient.IsMatch(emailAddress);
            //                      return isLenientMatch;

            bool isStrictMatch = reStrict.IsMatch(email);
            return isStrictMatch;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            bool isOk = true ;

            //Validate UserID
            if (string.IsNullOrWhiteSpace(this.txtUserID.Text))
            {
                this.errorProvider1.SetError(this.txtUserID, "請輸入帳號。");
                isOk = false;
            }
            else
            {
                if (!this.isValidEmail(this.txtUserID.Text.Trim()))
                {
                    this.errorProvider1.SetError(this.txtUserID, string.Format("帳號『{0}』是不正確的電子郵件格式。", this.txtUserID.Text));
                    isOk = false;
                }
                else
                    this.errorProvider1.SetError(this.txtUserID, "");
            }

            //  驗證副本是否符合電子郵件格式
            string[] ccs = this.txtCC.Text.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> err_emails = new List<string>();
            foreach (string cc in ccs)
            {
                if (cc.Trim().Length > 0)
                {
                    if (!this.isValidEmail(cc))
                        err_emails.Add(cc);
                }
            }
            if (err_emails.Count > 0)
            {
                this.errorProvider1.SetError(this.txtCC, string.Format("寄信副本『{0}』是不正確的電子郵件格式。", string.Join(",", err_emails)));
                isOk = false;
            }
            else
                this.errorProvider1.SetError(this.txtCC, "");

            if (isOk)
            {
                //Validate Password
                if (string.IsNullOrWhiteSpace(this.txtPassword.Text))
                {
                    if (MessageBox.Show("確定密碼是空白嗎？", "注意", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != System.Windows.Forms.DialogResult.Yes)
                    {
                        isOk = false;
                    }
                }
            }

            if (isOk)
            {
                if (this.AfterInputCredential != null)
                {
                    EmailCredentialEventArgs arg = new EmailCredentialEventArgs(this.txtUserID.Text, this.txtPassword.Text, this.txtCC.Text);
                    this.AfterInputCredential(this, arg);
                }
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.AfterInputCredential != null)
            {
                EmailCredentialEventArgs arg = new EmailCredentialEventArgs(string.Empty, string.Empty, string.Empty, true);
                this.AfterInputCredential(this, arg);
            }
            this.Close();
        }
    }

    //internal class EmailCredentialEventArgs : EventArgs
    //{
    //    public EmailCredentialEventArgs(string userID, string pwd, string cc)
    //        : base()
    //    {
    //        this.UserID = userID;
    //        this.Password = pwd ;
    //        this.CC = cc;
    //        this.Cancel = false;
    //    }

    //    public EmailCredentialEventArgs(string userID, string pwd, string cc, bool cancel)
    //        : base()
    //    {
    //        this.UserID = userID;
    //        this.Password = pwd;
    //        this.CC = cc;
    //        this.Cancel = cancel;
    //    }

    //    public string UserID { get; private set; }
    //    public string Password { get; private set; }
    //    public string CC { get; private set; }
    //    public bool Cancel { get; private set; }
    //}
}