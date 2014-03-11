using System;
using FISCA.UDT;
using System.Collections.Generic;
using System.Linq;

namespace TeachingEvaluation.UDT
{
    /// <summary>
    /// 評鑑樣版
    /// </summary>
    [FISCA.UDT.TableName("ischool.emba.cs_configuration")]
    public class CSConfiguration : ActiveRecord
    {
        //internal static void RaiseAfterUpdateEvent(object sender, ParameterEventArgs e)
        //{
        //    if (CSAttend.AfterUpdate != null)
        //        CSAttend.AfterUpdate(sender, e);
        //}

        //internal static event EventHandler<ParameterEventArgs> AfterUpdate;

        public enum TemplateName { 教學評鑑意見調查EMAIL提醒 = 0, 教學評鑑意見調查EMAIL再次提醒 = 1, 教學評鑑意見調查EMAIL提醒_subject = 2, 教學評鑑意見調查EMAIL再次提醒_subject = 3, 評鑑注意事項 = 4 };

        public static UDT.CSConfiguration GetEmailSenderInfo()
        {
            string key = "email_sender";
            return getConf(key, "");
        }

        /// <summary>
        /// 名稱
        /// <list type="string" caption="教學評鑑意見調查EMAIL提醒">teaching_evaluation_email_reminder_template_1</list>
        /// <list type="string" caption="教學評鑑意見調查EMAIL再次提醒">teaching_evaluation_email_reminder_template_2</list>
        /// <list type="string" caption="教學評鑑意見調查EMAIL提醒_subject">teaching_evaluation_email_reminder_template_2</list>
        /// <list type="string" caption="教學評鑑意見調查EMAIL再次提醒_subject">teaching_evaluation_email_reminder_template_2</list>
        /// <list type="string" caption="評鑑注意事項">teaching_evaluation_precautions</list>
        /// </summary>
        [Field(Field = "conf_name", Indexed = true, Caption = "名稱")]
        public string Name { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        [Field(Field = "conf_content", Indexed = false, Caption = "內容")]
        public string Content { get; set; }

        public static UDT.CSConfiguration GetTemplate(TemplateName key)
        {
            List<string> keys = new List<string>() { "teaching_evaluation_email_reminder_template_1", "teaching_evaluation_email_reminder_template_2", "teaching_evaluation_email_reminder_template_1_subject", "teaching_evaluation_email_reminder_template_2_subject", "teaching_evaluation_precautions" };

            return getConf(keys.ElementAt((int)key), "");
        }

        public static UDT.CSConfiguration GetTemplate(string key)
        {
            return getConf(key, "");
        }

        private static UDT.CSConfiguration getConf(string key, string defaultValue)
        {
            AccessHelper ah = new AccessHelper();

            List<UDT.CSConfiguration> configs = ah.Select<UDT.CSConfiguration>(string.Format("conf_name='{0}'", key.ToString()));

            if (configs.Count < 1)
            {
                UDT.CSConfiguration conf = new CSConfiguration();
                conf.Name = key.ToString();
                conf.Content = defaultValue;
                List<ActiveRecord> recs = new List<ActiveRecord>();
                recs.Add(conf);
                ah.SaveAll(recs);
                configs = ah.Select<UDT.CSConfiguration>(string.Format("conf_name='{0}'", key));
            }

            return configs[0];
        }

        /// <summary>
        /// 淺層複製自己
        /// </summary>
        /// <returns></returns>
        public CSConfiguration Clone()
        {
            return (this.MemberwiseClone() as CSConfiguration);
        }
    }
}


