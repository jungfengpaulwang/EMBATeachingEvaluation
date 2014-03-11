using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TeachingEvaluation.PrivateControl
{
    /// <summary>
    /// 延遲決定順序的物件集合
    /// </summary>
    internal interface ILazyOrder
    {
        /// <summary>
        /// Layout Description 在 Layout 定議檔中的 XPath，例如 Ribbons/Ribbon/@Title='學生'。
        /// </summary>
        string LayoutPath { get; }

        /// <summary>
        /// 設定 Layout 順序。
        /// </summary>
        /// <param name="layoutDesc"></param>
        void SetLayout(XElement layoutDesc);
    }

    internal class ItemComparer : IComparer<string>
    {
        /// <summary>
        /// 記錄每一個字串的前後順序。
        /// </summary>
        protected Dictionary<string, decimal> Firsts = new Dictionary<string, decimal>();
        protected Dictionary<string, decimal> Lasts = new Dictionary<string, decimal>();

        /// <summary>
        /// 從 1  開始。
        /// </summary>
        protected decimal FirstOrder = 1;
        /// <summary>
        /// 從 int.MaxValue - 1000 開始。
        /// </summary>
        protected decimal LastOrder = int.MaxValue - 10000; //預留 10000 位置，應該夠吧!

        public ItemComparer(XElement orders)
            : this(orders, new string[] { })
        {
        }

        public ItemComparer(XElement orders, IEnumerable<string> originOrders)
        {
            decimal mid = FirstOrder + 10000m;
            foreach (string item in originOrders)
            {
                if (!Firsts.ContainsKey(item))
                    Firsts.Add(item, mid++);
            }

            foreach (XElement item in orders.Elements("Item"))
            {
                XAttribute title = item.Attribute("Title");
                XAttribute last = item.Attribute("Last");

                if (Firsts.ContainsKey(title.Value.Trim()))
                    Firsts.Remove(title.Value.Trim());

                bool isLast = false;
                if (last != null && bool.TryParse(last.Value, out isLast))
                {
                    if (isLast)
                    {
                        Lasts.Add(title.Value.Trim(), LastOrder++);
                        continue; //是 Last 就不會是 First。
                    }
                }

                Firsts.Add(title.Value.Trim(), FirstOrder++);
            }
        }

        #region IComparer<string> 成員

        public int Compare(string x, string y)
        {
            return DefaultCompare(x, y);
        }

        protected virtual int DefaultCompare(string x, string y)
        {
            decimal X = decimal.MaxValue, Y = decimal.MaxValue;

            if (Lasts.ContainsKey(x)) //看是不是 Last。
                X = Lasts[x];
            else if (Firsts.ContainsKey(x)) //看是不是 First。
                X = Firsts[x];
            else  //都不是的話，就加入到 First。
            {
                X = FirstOrder;
                Firsts.Add(x, FirstOrder++);
            }

            if (Lasts.ContainsKey(y)) //看是不是 Last。
                Y = Lasts[y];
            else if (Firsts.ContainsKey(y)) //看是不是 First。
                Y = Firsts[y];
            else //都不是的話，就加入到 First。
            {
                Y = FirstOrder;
                Firsts.Add(y, FirstOrder++);
            }

            return X.CompareTo(Y);
        }

        #endregion
    }

    internal class ItemNameCorrector
    {
        private Dictionary<string, string> Corrects = new Dictionary<string, string>();

        public ItemNameCorrector(XElement correctList)
        {
            foreach (XElement item in correctList.Elements("Item"))
            {
                XAttribute title = item.Attribute("Title");
                XAttribute correctFrom = item.Attribute("CorrectFrom");

                if (correctFrom != null)
                    Corrects.Add(correctFrom.Value, title.Value);
            }
        }

        public string Correct(string oldName)
        {
            string name = oldName.Trim();
            if (Corrects.ContainsKey(name))
            {
                Console.WriteLine("項目更名：{0} -> {1}", name, Corrects[name]);
                return Corrects[name];
            }
            else
                return oldName;
        }
    }
}
