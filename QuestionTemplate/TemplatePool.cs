using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace TeachingEvaluation.QuestionTemplate
{
    public class TemplatePool
    {
        private static readonly Lazy<TemplatePool> LazyInstance = new Lazy<TemplatePool>(() => new TemplatePool());
        public static TemplatePool Instance { get { return LazyInstance.Value; } }

        private Dictionary<Type, List<TeachingEvaluation.PrivateControl.DetailContent>> _Templates;

        private TemplatePool() 
        {
            this._Templates = new Dictionary<Type, List<TeachingEvaluation.PrivateControl.DetailContent>>();
        }

        public static TeachingEvaluation.PrivateControl.DetailContent GetTemplate<T>() where T : TeachingEvaluation.PrivateControl.DetailContent
        {
            foreach (Type key in TemplatePool.Instance._Templates.Keys)
            {
                if (key != typeof(T))
                    continue;

                foreach (TeachingEvaluation.PrivateControl.DetailContent detail_content in TemplatePool.Instance._Templates[key])
                {
                    if (!detail_content.Dirty)
                    {
                        detail_content.Dirty = true;                        
                        return detail_content;
                    }
                }
            }
            var DetailContent = (T)Assembly.Load(typeof(T).Assembly.FullName).CreateInstance(typeof(T).FullName); 
            DetailContent.Dirty = true;
            DetailContent.Group = Guid.NewGuid().ToString();
            DetailContent.Tag = DetailContent.Group;
            DetailContent.DisplayOrder = null;
            if (!TemplatePool.Instance._Templates.ContainsKey(typeof(T)))
                TemplatePool.Instance._Templates.Add(typeof(T), new List<PrivateControl.DetailContent>());

            TemplatePool.Instance._Templates[typeof(T)].Add(DetailContent);

            return DetailContent;
        }

        public static void SetTemplate<T>(T DetailContent) where T : TeachingEvaluation.PrivateControl.DetailContent
        {
            DetailContent.Dirty = false;
            DetailContent.Group = Guid.NewGuid().ToString();
            DetailContent.Tag = DetailContent.Group;
            DetailContent.DisplayOrder = null;

            if (!TemplatePool.Instance._Templates.ContainsKey(typeof(T)))
                TemplatePool.Instance._Templates.Add(typeof(T), new List<PrivateControl.DetailContent>());

            TemplatePool.Instance._Templates[typeof(T)].Add(DetailContent);
        }

        public static void CleanTemplates()
        {
            foreach (Type key in TemplatePool.Instance._Templates.Keys)
            {
                foreach (TeachingEvaluation.PrivateControl.DetailContent detail_content in TemplatePool.Instance._Templates[key])
                {
                    detail_content.Dirty = false;
                }
            }
        }
    }
}
