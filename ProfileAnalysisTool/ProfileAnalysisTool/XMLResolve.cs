using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Threading;

namespace ProfileAnalysisTool
{
    class XMLResolve
    {
        private static XMLResolve instance;
        private XMLResolve() { }
        private static object _lockObj = new object();
        private static XmlDocument _xmlDoc = new XmlDocument();
        private static XmlElement root = null;
        private static string xmlFilePath = AppDomain.CurrentDomain.BaseDirectory + @"Config.xml";
        public static XMLResolve GetInstance()
        {
            lock (_lockObj)
            {
                if (instance == null)
                {

                    _xmlDoc.Load(xmlFilePath);
                    root = _xmlDoc.DocumentElement;
                    instance = new XMLResolve();
                }
                return instance;
            }

        }

        public static string getXmlValue(string _path, string TagName)
        {

            //XmlElement _node = (XmlElement)root.SelectSingleNode("/Config/GoSystem/GoSensor[BuddyNo='2']");       
            //string str = _node.GetElementsByTagName("IPAddr").Item(0).InnerText;
            XmlElement _node = (XmlElement)root.SelectSingleNode(_path);
            return _node.GetElementsByTagName(TagName).Item(0).InnerText;
        }

        public static void setXmlValue(string _path, string TagName, string Value)
        {
            XmlElement _node = (XmlElement)root.SelectSingleNode(_path);
            _node.GetElementsByTagName(TagName).Item(0).InnerText = Value;
        }


        public static string getIPAddr()
        {
            string xPath = "/Config/GoSystem/GoSensor[BuddyNo='0']";
            string TagName = "IPAddr";
            return getXmlValue(xPath, TagName);
        }
    }
}
