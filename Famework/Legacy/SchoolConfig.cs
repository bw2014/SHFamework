using System;
using System.Collections.Generic;
using System.Text;
using FISCA.DSAUtil;
using System.Xml;
using Framework.Feature;

namespace Framework.Legacy
{
    [Obsolete("程式碼轉移遺，為了相容留下來。")]
    public class SchoolConfig
    {
        private bool _loaded = false;
        //private XmlElement _default = null;

        public SchoolConfig()
        {
            Load();
        }

        private DSXmlHelper _source;
        private DSXmlHelper Source
        {
            get { return _source; }
            set { _source = value; }
        }

        public XmlElement Content
        {
            get { return Source.BaseElement; }
        }

        public void Load()
        {
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    _source = new DSXmlHelper(Config.GetSchoolConfig().GetElement("Content"));
                    _loaded = true;
                    if (_loaded)
                        break;
                }
                catch (Exception ex)
                {
                    BugReporter.ReportException(GlobalOld.SystemName, GlobalOld.SystemVersion, ex, false, string.Empty);
                }
            }
        }

        public void Save()
        {
            try
            {
                Config.SetSchoolConfig(Source.BaseElement);
            }
            catch (Exception ex)
            {
                //CurrentUser user = CurrentUser.Instance;
                BugReporter.ReportException(GlobalOld.SystemName, GlobalOld.SystemVersion, ex, false, string.Empty);
            }
        }
    }
}
