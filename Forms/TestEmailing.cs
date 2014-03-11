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
using System.Dynamic;
using Mandrill.Models;
using Mandrill;
using System.Threading.Tasks;
using Aspose.Cells;

namespace TeachingEvaluation.Forms
{
    /// <summary>
    /// 處理當要發送Email 時，使用者要輸入 SMTP 的認證資訊。
    /// </summary>
    public partial class TestEmailing : BaseForm
    {
    //    private AccessHelper Access;
    //    private string MandrillAPIKey;
    //    private BrowserWrapper.BrowserProxy proxy = new BrowserWrapper.BrowserProxy();

    //    BackgroundWorker bw;

    //    enum MandrillError
    //    {
    //        OK,
    //        WebException,
    //        HttpNotOk,
    //        Invalid,
    //        Rejected,
    //        Unknown
    //    }

    //    public TestEmailing()
    //    {
    //        InitializeComponent();

    //        this.Access = new AccessHelper();
    //        this.MandrillAPIKey = "jrnVqazuZLx7C-9Ya_Ee8Q"; // string.Empty;

    //        this.Load += new System.EventHandler(this.Form_Load);
    //        bw = new BackgroundWorker();
    //        bw.RunWorkerCompleted += (m, n) =>
    //        {
    //            this.btnSend.Enabled = true;
    //        };
    //        bw.DoWork += (m, n) =>
    //        {
    //            MailLog log = n.Argument as MailLog;
    //            log.SaveLogs();
    //        };
    //    }

    //    private void Form_Load(object sender, EventArgs e)
    //    {
    //        proxy.Decorate(this.webBrowser1, string.Empty);
    //    }

    //    private void btnSend_Click(object sender, EventArgs e)
    //    {
    //        this.btnSend.Enabled = false;

    //        MandrillApi mandrill = new MandrillApi(this.MandrillAPIKey, false);

    //        string pong = string.Empty;

    //        try
    //        {
    //            pong = mandrill.Ping();
    //        }
    //        catch (Exception ex)
    //        {
    //            MessageBox.Show("未正確設定「MandrillAPIKey」, 錯誤訊息：" + ex.Message);
    //            return;
    //        }
    //        if (!pong.ToUpper().Contains("PONG!"))
    //        {
    //            MessageBox.Show("請先設定「MandrillAPIKey」。");
    //            return;
    //        }

    //        List<EmailMessage> EmailMessages = new List<EmailMessage>();

    //        Workbook wb = new Workbook();
    //        OpenFileDialog ofd = new OpenFileDialog();
    //        ofd.ShowDialog();
    //        wb.Open(ofd.FileName);

    //        int i = 1;
    //        while (!string.IsNullOrEmpty(wb.Worksheets[0].Cells[i, 0].Value + ""))
    //        {
    //            EmailMessage message = new EmailMessage();
    //            message.auto_text = true;
    //            message.from_email = "paul.wang@ischool.com.tw";
    //            message.from_name = "test";
    //            string[] mail_tos = new string[] { wb.Worksheets[0].Cells[i, 0].Value + "", wb.Worksheets[0].Cells[i, 1].Value + "", wb.Worksheets[0].Cells[i, 2].Value + "" };
    //            List<EmailAddress> EmailAddresss = new List<EmailAddress>();
    //            foreach (string mail_to in mail_tos)
    //            {
    //                EmailAddress mt = new EmailAddress();

    //                mt.email = mail_to;
    //                mt.name = string.Empty;

    //                EmailAddresss.Add(mt);
    //            }
    //            message.to = EmailAddresss;

    //            message.track_clicks = true;
    //            message.track_opens = true;
    //            message.html = webBrowser1.Document.Body.InnerHtml;
    //            message.important = true;
    //            message.merge = false;
    //            message.preserve_recipients = true;
    //            message.subject = this.txtSubject.Text.Trim();

    //            EmailMessages.Add(message);

    //            i++;
    //        }

    //        string GUID = Guid.NewGuid().ToString();
    //        MailLog MailLog = new MailLog();
    //        MailLog.MailCount = EmailMessages.Count;
    //        Parallel.ForEach(EmailMessages, (message) =>
    //        {
    //            //lock (message)
    //            //{
    //            //    mandrill.SendMessageAsync(message).ContinueWith((x) =>
    //            //    {
    //            //        lock(MailLog)
    //            //        {
    //            //            MailLog.CurrentCount += 1;

    //            //            if (x.Exception == null)
    //            //            {
    //            MailLog.AddLogs(this.AddLog(mandrill.SendMessageSync(message), GUID, false, 102, 1, "paul.wang@ischool.com.tw"));
    //            //            }

    //            //            if (MailLog.MailCount == MailLog.CurrentCount)
    //            //            {
    //            //                bw.RunWorkerAsync(MailLog);
    //            //            }
    //            //        }
    //            //    });
    //            //}                
    //        });
    //    }

    //    private void AddLogs(List<EmailResult> EmailResults)
    //    {

    //    }

    //    private List<UDT.MailLog> AddLog(List<EmailResult> results, string GUID, bool CC, int SchoolYear, int Semester, string Sender)
    //    {
    //        List<UDT.MailLog> MailLogs = new List<UDT.MailLog>();
    //        foreach (EmailResult result in results)
    //        {
    //            UDT.MailLog MailLog = new UDT.MailLog();

    //            MailLog.EmailCategory = "缺課通知";
    //            MailLog.GUID = GUID;
    //            MailLog.IsCC = CC;
    //            MailLog.Log = string.Empty;
    //            MailLog.RecipientEmailAddress = result.Email;
    //            MailLog.SchoolYear = SchoolYear;
    //            MailLog.Semester = Semester;
    //            MailLog.Sender = Sender;
    //            MailLog.TimeStamp = DateTime.Now;
    //            MailLog.Status = result.Status + "";

    //            MailLogs.Add(MailLog);
    //            //MessageBox.Show(string.Format(@"AsyncState：{0}\nCreationOptions：{1}\nException：{2}\nIsCanceled：{3}\nIsCompleted：{4}\nIsFaulted：{5}\nResult：{6}\nStatus：{7}", x.AsyncState, x.CreationOptions.ToString(), x.Exception == null ? string.Empty : x.Exception.Message, x.Id, x.IsCanceled, x.IsCompleted, x.IsFaulted, x.Result.ToString(), x.Status.ToString()));
    //        }

    //        return MailLogs;
    //    }

    //    private void btnExit_Click(object sender, EventArgs e)
    //    {
    //        this.Close();
    //    }
    //}
    
    //public class MailLog
    //{
    //    public int MailCount { get; set; }
    //    public int CurrentCount { get; set; }

    //    private List<UDT.MailLog> _MailLogs;

    //    public MailLog() 
    //    {
    //        _MailLogs = new List<UDT.MailLog>();
    //    }

    //    public void AddLogs(List<UDT.MailLog> MailLogs)
    //    {
    //        _MailLogs.AddRange(MailLogs);
    //    }

    //    public void SaveLogs()
    //    {
    //        _MailLogs.SaveAll();
    //    }
    }
}