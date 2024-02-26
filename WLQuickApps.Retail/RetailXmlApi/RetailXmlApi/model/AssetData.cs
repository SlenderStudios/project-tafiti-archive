using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace RetailXmlApi.model
{
    public class AssetData
    {
        protected string m_id;
        protected Uri m_url;

        public AssetData(string id, Uri url)
        {
            m_id = id;
            m_url = url;
        }
        public Uri Url
        {
            get
            {
                return m_url;
            }
        }
        public string Id
        {
            get
            {
                return m_id;
            }
        }
    }
}
