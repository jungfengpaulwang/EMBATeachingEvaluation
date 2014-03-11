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
using FISCA.UDT;

namespace TeachingEvaluation.Forms
{
    public partial class Email_Sender : BaseForm
    {
        private UDT.CSConfiguration conf;
        public Email_Sender()
        {
            InitializeComponent();
        }

        private void Email_Sender_Load(object sender, EventArgs e)
        {
            conf = UDT.CSConfiguration.GetEmailSenderInfo();
            this.textBoxX1.Text = conf.Content;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            conf.Content = this.textBoxX1.Text;
            List<ActiveRecord> recs = new List<ActiveRecord>();
            recs.Add(conf);
            (new AccessHelper()).SaveAll(recs);
            this.Close();
        }

        private void textBoxX1_TextChanged(object sender, EventArgs e)
        {
            if (isValidEmail(this.textBoxX1.Text))
            {
                this.errorProvider1.SetError(this.textBoxX1, "");
                //this.btnSave.Enabled = true;
            }
            else
            {
                this.errorProvider1.SetError(this.textBoxX1, "Email 格式不正確！");
                //this.btnSave.Enabled = false ;
            }
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

        }
    }

    internal class EmailCredentialEventArgs : EventArgs
    {
        public EmailCredentialEventArgs(string userID, string pwd, string cc)
            : base()
        {
            this.UserID = userID;
            this.Password = pwd;
            this.CC = cc;
            this.Cancel = false;
        }

        public EmailCredentialEventArgs(string userID, string pwd, string cc, bool cancel)
            : base()
        {
            this.UserID = userID;
            this.Password = pwd;
            this.CC = cc;
            this.Cancel = cancel;
        }

        public string UserID { get; private set; }
        public string Password { get; private set; }
        public string CC { get; private set; }
        public bool Cancel { get; private set; }
    }    
}
