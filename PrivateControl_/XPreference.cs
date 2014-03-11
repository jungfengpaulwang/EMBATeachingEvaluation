using System;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace CateringService_Survey.PrivateControl
{
    /// <summary>
    /// Preference 的 XElement(XLinq) 版。
    /// </summary>
    internal class XPreference
    {
        /// <summary>
        /// 包含了 Configuration 的所有原始資料。
        /// </summary>
        public XElement Container { get; set; }

        private XElement StringContainer { get; set; }
        private XElement IntegerContainer { get; set; }
        private XElement DecimalContainer { get; set; }
        private XElement BooleanContainer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public XPreference()
        {
            Container = new XElement("Configuration");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        public XPreference(XElement container)
        {
            Container = container;
        }

        /// <summary>
        /// 名稱。
        /// </summary>
        public string Name { get { return Container.Name.LocalName; } }

        /// <summary>
        /// 取得或建立子組態。
        /// </summary>
        /// <param name="containerName">組態識別。</param>
        /// <returns></returns>
        public XPreference GetChild(string containerName)
        {
            string key = "Child_" + containerName.NormalizeKey();
            XElement child = Container.Element(key);

            if (child == null)
            {
                child = new XElement(key);
                Container.Add(child);
            }

            XPreference objchild = new XPreference(child);
            objchild.ContentChanged += delegate
            {
                OnContentChanged();
            };

            return objchild;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetString(string key)
        {
            return GetString(key, GetStringContainer());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetString(string key, string defaultValue)
        {
            string value = GetString(key);

            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;
            else
                return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetString(string key, string value)
        {
            SetString(key, value, GetStringContainer());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int GetInteger(string key, int defaultValue)
        {
            string strVal = GetString(key, GetIntegerContainer());
            int val;

            if (int.TryParse(strVal, out val))
                return val;
            else
                return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetInteger(string key, int value)
        {
            SetString(key, value.ToString(), GetIntegerContainer());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public decimal GetDecimal(string key, decimal defaultValue)
        {
            string strVal = GetString(key, GetDecimalContainer());
            decimal val;

            if (decimal.TryParse(strVal, out val))
                return val;
            else
                return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetDecimal(string key, decimal value)
        {
            SetString(key, value.ToString(), GetDecimalContainer());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public bool GetBoolean(string key, bool defaultValue)
        {
            string strVal = GetString(key, GetBooleanContainer());
            bool val;

            if (bool.TryParse(strVal, out val))
                return val;
            else
                return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetBoolean(string key, bool value)
        {
            SetString(key, value.ToString(), GetBooleanContainer());
        }

        /// <summary>
        /// 清除所有子元素與屬性。
        /// </summary>
        public void Clear()
        {
            Container.RemoveAll();
        }

        /// <summary>
        /// 有 Configuration 內容變動時。
        /// </summary>
        public event EventHandler ContentChanged;

        /// <summary>
        /// 
        /// </summary>
        protected void OnContentChanged()
        {
            if (ContentChanged != null)
                ContentChanged(this, EventArgs.Empty);
        }

        private string GetString(string key, XElement container)
        {
            XAttribute att = container.Attribute(key.NormalizeKey());
            if (att == null)
                return string.Empty;
            else
                return att.Value;
        }

        private void SetString(string key, string value, XElement container)
        {
            container.SetAttributeValue(key.NormalizeKey(), value);
            OnContentChanged();
        }

        private XElement GetBooleanContainer()
        {
            string Name = "BooleanContainer";
            if (Container.Element(Name) == null)
            {
                BooleanContainer = new XElement(Name);
                Container.Add(BooleanContainer);
            }
            else if (BooleanContainer == null)
                BooleanContainer = Container.Element(Name);

            return BooleanContainer;
        }

        private XElement GetDecimalContainer()
        {
            string Name = "DecimalContainer";
            if (Container.Element(Name) == null)
            {
                DecimalContainer = new XElement(Name);
                Container.Add(DecimalContainer);
            }
            else if (DecimalContainer == null)
                DecimalContainer = Container.Element(Name);

            return DecimalContainer;
        }

        private XElement GetIntegerContainer()
        {
            string Name = "IntegerContainer";

            if (Container.Element(Name) == null)
            {
                IntegerContainer = new XElement(Name);
                Container.Add(IntegerContainer);
            }
            else if (IntegerContainer == null)
                IntegerContainer = Container.Element(Name);

            return IntegerContainer;
        }

        private XElement GetStringContainer()
        {
            string Name = "StringContainer";
            if (Container.Element(Name) == null)
            {
                StringContainer = new XElement(Name);
                Container.Add(StringContainer);
            }
            else if (StringContainer == null)
                StringContainer = Container.Element(Name);

            return StringContainer;
        }
    }

    internal static class StringExtensions
    {
        public static string NormalizeKey(this string input)
        {
            //把所有非一般文字的字元，換成「_」。
            return Regex.Replace(input, @"\W", "_", RegexOptions.Singleline);
        }
    }
}