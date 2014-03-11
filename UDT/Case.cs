using System;
using FISCA.UDT;

namespace TeachingEvaluation.UDT
{
    /// <summary>
    /// 個案
    /// </summary>
    [FISCA.UDT.TableName("ischool.emba.case_management.case")]
    public class Case : ActiveRecord
    {
        /// <summary>
        /// 個案英文名稱
        /// </summary>
        [Field(Field = "english_name", Indexed = false, Caption = "英文名稱")]
        public string EnglishName { get; set; }

        /// <summary>
        /// 個案中文名稱
        /// </summary>
        [Field(Field = "name", Indexed = true, Caption = "名稱")]
        public string Name { get; set; }

        /// <summary>
        /// 個案編號
        /// </summary>
        [Field(Field = "no", Indexed = false, Caption = "個案編號")]
        public string No { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [Field(Field = "author", Indexed = false, Caption = "作者")]
        public string Author { get; set; }

        /// <summary>
        /// 出版學校
        /// </summary>
        [Field(Field = "publish_school", Indexed = false, Caption = "出版學校")]
        public string PublishSchool { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        [Field(Field = "memo", Indexed = false, Caption = "備註")]
        public string Memo { get; set; }

        /// <summary>
        /// 個案文件連結
        /// </summary>
        [Field(Field = "url_list", Indexed = false, Caption = "文件連結")]
        public string UrlList { get; set; }

        /// <summary>
        /// 淺層複製物件
        /// </summary>
        /// <returns></returns>
        public Case Clone()
        {
            return this.MemberwiseClone() as Case;
        }
    }
}
